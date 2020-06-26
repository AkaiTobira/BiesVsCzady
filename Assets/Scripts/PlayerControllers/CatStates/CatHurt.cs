using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHurt : BaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;
    private float velocitXFriction = 0.0f;

    public CatHurt( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "CatHurt";

        fillKnockbackInfo( infoPack );
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        velocity          = infoPack.knockBackValue * 1.75f;
        velocity.x        *= (int)infoPack.fromCameAttack;
        velocitXFriction  = infoPack.knockBackFrictionX;

        if( velocitXFriction > 0){
            m_detector.CheatMove( new Vector2(0,40.0f));
        }
    }

    protected override void SetUpAnimation(){
        m_animator.SetTrigger( "CatHurt" );
        timeToEnd = getAnimationLenght("CatHurt");

        m_transition = m_controllabledObject.
               GetComponent<Player>().animationNode.
               GetComponent<AnimationTransition>();
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.ResetTrigger( "CatHurt" );
        }
    }

    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity.y += -CatUtils.GravityForce * Time.deltaTime;
        float xMax = Mathf.Max(Mathf.Abs( velocity.x ) - (velocitXFriction * Time.deltaTime), 0 );
        velocity.x = Mathf.Sign(velocity.x) * xMax;
        m_detector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }

    public override void HandleInput(){}
}
