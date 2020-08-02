using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttack4 : PlayerBaseState
{ 

    private float animationTime;    
    private float timeToEnd;
    private AnimationTransition m_transition;
    float ANNIMATION_SPEED = 1.7f;
    public BiesAttack4( GameObject controllable) : base( controllable ){
        name = "BiesAttack4";
        distanceToFixAnimation = new Vector3(0, 7.5f , 0);
        m_animator.SetBool("Attack4", true);
        animationTime = getAnimationLenght("PlayerAttack4") / (ANNIMATION_SPEED);// * 3.0f);

        m_animator.SetFloat("AnimationSpeed", ANNIMATION_SPEED );// 3);
        timeToEnd     = animationTime;
    }


    public override void OnExit(){
        m_animator.SetBool("Attack4", false);
    }

    protected override void SetUpAnimation(){


        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool("Attack4", false);
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
            m_nextState = new BiesAttack5( m_controllabledObject);
        }

    }
}
