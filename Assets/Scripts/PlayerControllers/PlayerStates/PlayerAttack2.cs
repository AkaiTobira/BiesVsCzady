using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2 : PlayerBaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;

    public PlayerAttack2( GameObject controllable) : base( controllable ){
        name = "PlayerAttack2";
        m_animator.SetBool("Attack2", true);
        timeToEnd = getAnimationLenght("PlayerAttack2");

        GlobalUtils.cameraShake.TriggerShake(timeToEnd);

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
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
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
