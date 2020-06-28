using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1 : BaseState{    
    private float timeToEnd;

    private AnimationTransition m_transition;


    public PlayerAttack1( GameObject controllable) : base( controllable ){
        name = "PlayerAttack1";
        m_animator.SetBool("Attack1", true);
        timeToEnd = getAnimationLenght("PlayerAttack1");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool("Attack1", false);
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
