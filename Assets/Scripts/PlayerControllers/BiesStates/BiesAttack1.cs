using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttack1 : PlayerBaseState
{ 

    private float animationTime;
    private float timeToEnd;
    private AnimationTransition m_transition;

    public BiesAttack1( GameObject controllable) : base( controllable ){
        name = "BiesAttack1";
        distanceToFixAnimation = new Vector3(0, 75 , 0);
    }

    protected override void SetUpAnimation(){
        m_animator.SetBool("Attack1", true);
        animationTime = getAnimationLenght("PlayerAttack1");
        timeToEnd     = animationTime;

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }


    public override void OnExit(){
        m_animator.SetBool("Attack1", false);
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
        velocity = (int)m_FloorDetector.GetCurrentDirection() * m_transition.MoveSpeed;
        m_FloorDetector.Move(velocity*Time.deltaTime);
    }
    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){

        if( PlayerInput.isAttack1KeyPressed() && timeToEnd < 0.5 * animationTime ){
            m_isOver = true;
            m_nextState = new BiesAttack4( m_controllabledObject);
        }


    }
}
