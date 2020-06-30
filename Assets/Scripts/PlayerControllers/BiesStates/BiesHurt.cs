using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesHurt : BaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;
    private float velocitXFriction = 0.0f;

    private float knocBackDirection;

    public BiesHurt( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "BiesHurt";

        m_dir = infoPack.fromCameAttack;
        knocBackDirection = (int)infoPack.fromCameAttack;

        fillKnockbackInfo( infoPack );
    }

  //  protected override void UpdateDirection(){


 //   }


    protected override void SetUpAnimation(){
        m_animator.SetTrigger( "BiesHurt" );
        timeToEnd = getAnimationLenght("BiesHurt");

        m_transition = m_controllabledObject.
               GetComponent<Player>().animationNode.
               GetComponent<AnimationTransition>();
    }



    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        CommonValues.PlayerVelocity   = infoPack.knockBackValue;
        CommonValues.PlayerVelocity.x *= (int)infoPack.fromCameAttack;

     //   if(Mathf.Abs( CommonValues.PlayerVelocity.x ) > infoPack.knockBackValue.x ) 
    //        velocity.x = (int)infoPack.fromCameAttack * Mathf.Abs( CommonValues.PlayerVelocity.x );
 
        velocitXFriction  = infoPack.knockBackFrictionX;

        if( velocitXFriction > 0){
            m_detector.CheatMove( new Vector2(0,40.0f));
        }
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.ResetTrigger( "BiesHurt" );

            
            if( !m_detector.isOnGround() ){
                m_nextState = new BiesFall( m_controllabledObject, m_dir);
            }
        }
    }

    private void ProcessMove(){
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        PlayerFallHelper.FallRequirementsMeet( true );
        CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;

        if( knocBackDirection == -1 ) {
            CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x - velocitXFriction * Time.deltaTime;
        }else{
            CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x + velocitXFriction * Time.deltaTime;
        }

        m_detector.Move(CommonValues.PlayerVelocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
