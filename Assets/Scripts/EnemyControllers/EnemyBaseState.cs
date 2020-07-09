using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : IBaseState
{

    protected AkaiController entityScript = null;

    protected Vector3 animationVel = Vector3.zero;
    protected float m_smoothTime = 0.03f;

    public EnemyBaseState( GameObject controllable ){
        m_controllabledObject = controllable;
        m_FloorDetector = controllable.transform.Find("Detector").
                            GetComponent<ICollisionFloorDetector>();
        m_animator      = controllable.transform.Find("Animator")
                            .GetComponent<Animator>();
        m_nextState = null;
        entityScript = controllable.GetComponent<AkaiController>();
    }

    public override void Process(){
        if( !m_FloorDetector.isOnGround() ){
            entityScript.velocity.y -= entityScript.gravityForce * Time.deltaTime;
            m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);
        }else{
            entityScript.velocity.y = 0;
        }
    }

    protected float getAnimationLenght(string animationName){
        RuntimeAnimatorController ac = m_animator.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }

    public override void UpdateAnimator(){
        m_dir = m_FloorDetector.GetCurrentDirection();
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
    }

    protected virtual void UpdateAnimatorAligment(){
        Vector3 lScale  = m_animator.transform.localScale;
        lScale.x        = Mathf.Abs( lScale.x) * -(int)m_dir;
        m_animator.transform.localScale = lScale;
    }

    protected virtual void UpdateAnimatorPosition(){
        m_animator.transform.position = 
            Vector3.SmoothDamp( m_animator.transform.position, 
                                m_FloorDetector.GetComponent<Transform>().position, 
                                ref animationVel, 
                                m_smoothTime);
    }

    protected virtual void UpdateFloorAligment(){
        m_animator.transform.up = m_FloorDetector.GetSlopeAngle();
    }

}
