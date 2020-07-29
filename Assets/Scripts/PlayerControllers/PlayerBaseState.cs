using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : IBaseState, IInputProcessingState
{
    protected Vector2    velocity = new Vector2(0,0); 
    protected float slopeAngle    = 0.0f;
    protected float rotationAngle = 0;

    protected Vector3 distanceToFixAnimation = new Vector3();

    private protected ICharacterSettings m_settings;

    protected ICollisionWallDetector         m_WallDetector;
    protected ICollisionInteractableDetector m_ObjectInteractionDetector;


    public PlayerBaseState( GameObject controllableObject ){
        m_controllabledObject       = controllableObject;
        m_FloorDetector             = controllableObject.GetComponent<CollisionDetectorPlayer>();
        m_WallDetector              = controllableObject.GetComponent<CollisionDetectorPlayer>();
        m_ObjectInteractionDetector = controllableObject.GetComponent<CollisionDetectorPlayer>();
        m_animator                  = controllableObject.transform.GetComponent<Player>().
                                      animationNode.
                                      gameObject.GetComponent<Animator>();
        SetUpAnimation();
    }

    public override GlobalUtils.Direction GetDirection(){
        return m_FloorDetector.GetCurrentDirection();
    }

    protected Vector3 animationVel = Vector3.zero;
    protected float m_smoothTime = 0.03f;

    public override void UpdateAnimator(){
        UpdateDirection();
        UpdateAnimatorPosition();
        UpdateFloorAligment();
    }

    public virtual void HandleInput(){}

    public override void Process(){}

    protected virtual void UpdateAnimatorPosition(){

        m_controllabledObject.GetComponent<Player>().animationNode.position = 
            Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                m_controllabledObject.transform.position + distanceToFixAnimation + new Vector3(CommonValues.tempModulator2, CommonValues.tempModulator, 0), ref animationVel, m_smoothTime);

    }
    protected virtual void UpdateFloorAligment(){
        m_controllabledObject.GetComponent<Player>().animationNode.transform.up = m_FloorDetector.GetSlopeAngle();
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
            if (ac.animationClips[i].name == animationName){
                return ac.animationClips[i].length;
            }
        }
        return 0.0f;
    }

}
