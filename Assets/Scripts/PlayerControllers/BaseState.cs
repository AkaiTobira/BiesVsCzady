using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected GameObject m_controllabledObject;
    protected CollisionDetectorPlayer m_detector;
    protected Animator m_animator;
    
    protected BaseState  m_nextState = null;
    protected GlobalUtils.Direction m_dir;

    public string name = ""; 
    protected Vector2    velocity = new Vector2(0,0); 

    protected float slopeAngle   = 0.0f;
    protected float rotationAngle = 0;

    public BaseState( GameObject controllableObject ){
        m_controllabledObject = controllableObject;
        m_detector            = controllableObject.GetComponent<CollisionDetectorPlayer>();
        m_animator            = controllableObject.transform.GetComponent<Player>().
                                animationNode.
                                gameObject.GetComponent<Animator>();
        SetUpAnimation();
    }

    protected bool m_isOver = false;

    public bool isOver(){
        return m_isOver;
    }

    public virtual void HandleInput(){
    }

    public BaseState NextState(){
        BaseState temp = m_nextState;
        m_nextState = null;
        return temp;
    }

    protected Vector3 animationVel = Vector3.zero;
    protected float m_smoothTime = 0.03f;


    public void UpdateAnimator(){
        UpdateDirection();
        UpdateAnimatorPosition();
        UpdateFloorAligment();
    }


    protected virtual void UpdateAnimatorPosition(){

        m_controllabledObject.GetComponent<Player>().animationNode.position = 
            Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                m_controllabledObject.transform.position, ref animationVel, m_smoothTime);

    }


    protected virtual void UpdateFloorAligment(){
        float newSlopeAngle = m_detector.GetSlopeAngle(); 
        m_controllabledObject.GetComponent<Player>().animationNode.transform.up = m_detector.GetSlopeAngle2();
    //    Debug.Log( m_detector.GetSlopeAngle2());
    //    Debug.Log( m_controllabledObject.GetComponent<Player>().animationNode.transform.up );

    }

    protected bool isRightOriented(){
        return m_dir == GlobalUtils.Direction.Right;
    }

    protected bool isLeftOriented(){
        return m_dir == GlobalUtils.Direction.Left;
    }

    protected virtual void UpdateDirection(){
        if( CommonValues.PlayerVelocity.x != 0){

            m_dir = (GlobalUtils.Direction) Mathf.Sign( CommonValues.PlayerVelocity.x );

            Vector3 lScale =  m_controllabledObject.GetComponent<Player>().animationNode.localScale;
            lScale.x       = Mathf.Abs( lScale.x) * (int)m_dir;
            m_controllabledObject.GetComponent<Player>().animationNode.localScale = lScale;
        }
    }

    protected virtual void SetUpAnimation(){}
    protected float getAnimationLenght(string animationName){
        RuntimeAnimatorController ac = m_animator.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }

    public GlobalUtils.Direction GetDirection(){
        return m_detector.GetCurrentDirection();
    }

    public  virtual void Process(){}
    public virtual void OnExit(){}
    public virtual void OnEnter(){}
}
