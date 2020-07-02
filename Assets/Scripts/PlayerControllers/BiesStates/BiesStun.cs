using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesStun : PlayerBaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;
    private float velocitXFriction = 0.0f;

    public BiesStun( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "BiesStun";
        fillKnockbackInfo(infoPack);
    }

    protected override void SetUpAnimation(){
        m_animator.SetBool("isStunOver", false);
        m_animator.SetTrigger( "BiesStun" );

        m_transition = m_controllabledObject.
               GetComponent<Player>().animationNode.
               GetComponent<AnimationTransition>();
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        timeToEnd = infoPack.stunDuration;
        velocity          = infoPack.knockBackValue;
        velocity.x        *= (int)infoPack.fromCameAttack;
        velocitXFriction  = infoPack.knockBackFrictionX * (int)infoPack.fromCameAttack;
    }        

    private void  ProcessStateEnd(){
        m_animator.SetBool("isStunOver", false);
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.ResetTrigger( "BiesStun" );
            m_animator.SetBool("isStunOver", true);
        }
    }

    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        velocity.x = Mathf.Max( velocity.x - velocitXFriction, 0 );
        m_detector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
