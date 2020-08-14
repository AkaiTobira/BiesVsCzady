using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttack5 : PlayerBaseState
{ 

    private float timeToEnd;

    private float animationTime;    
    private AnimationTransition m_transition;
    float ANNIMATION_SPEED = 1.7f;

    GlobalUtils.Direction m_lockedDirection;
    public BiesAttack5( GameObject controllable) : base( controllable ){
        name = "BiesAttack5";
        distanceToFixAnimation = new Vector3(0, 7.5f , 0);
        m_animator.SetBool("Attack5", true);
        animationTime = getAnimationLenght("PlayerAttack5") / (ANNIMATION_SPEED);// * 3.0f);

        m_animator.SetFloat("AnimationSpeed", ANNIMATION_SPEED );// 3);
        timeToEnd     = animationTime;


        m_lockedDirection = m_FloorDetector.GetCurrentDirection();


        velocity = new Vector2( 80, 0 );
    }

    public override void OnExit(){
        m_animator.SetBool("Attack5", false);
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
            m_animator.SetBool("Attack1", false);
            m_animator.SetBool("Attack4", false);
            m_animator.SetBool("Attack5", false);
        }
    }
    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        m_FloorDetector.Move((float)m_lockedDirection * velocity * Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
