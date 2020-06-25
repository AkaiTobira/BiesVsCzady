using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAttack2 : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;

    private AnimationTransition m_transition = null;


    public CatAttack2( GameObject controllable) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == GlobalUtils.Direction.Left;
    //    name = "CatAttack2";
    //    m_animator.SetBool("Attack2", true);
    //    timeToEnd = getAnimationLenght("PlayerAttack2");

    //    GlobalUtils.cameraShake.TriggerShake(timeToEnd);

    //    m_transition = m_controllabledObject.
    //                   GetComponent<Player>().animationNode.
    //                   GetComponent<AnimationTransition>();

    // As Long as Solution is not accepted
        m_isOver = true;
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool("Attack2", false);
        }
    }
    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity = (int)m_detector.GetCurrentDirection() * m_transition.MoveSpeed;
        m_detector.Move(velocity*Time.deltaTime);
    }
    public override void Process(){
    //    ProcessStateEnd();
    //    ProcessMove();
    }
    public override void HandleInput(){
    }
}
