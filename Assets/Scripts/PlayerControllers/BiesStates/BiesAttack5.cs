using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttack5 : BaseState
{ 
    private float timeToEnd;
    private AnimationTransition m_transition;

    public BiesAttack5( GameObject controllable) : base( controllable ){
        name = "BiesAttack5";
    }

    protected override void SetUpAnimation(){
        m_animator.SetBool("Attack5", true);
        timeToEnd = getAnimationLenght("PlayerAttack5");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool("Attack1", false);
            m_animator.SetBool("Attack5", false);
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
