using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesHurt : PlayerBaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;
    private float velocitXFriction = 0.0f;

    private float knocBackDirection;

    private bool isFaceLocked = false;

    private GlobalUtils.Direction savedDir;

    public BiesHurt( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "BiesHurt";

        savedDir     = m_FloorDetector.GetCurrentDirection();
        isFaceLocked = infoPack.lockFaceDirectionDuringKnockback;
        if( !isFaceLocked ) m_dir = infoPack.fromCameAttack;
        knocBackDirection = (int)infoPack.fromCameAttack;

        fillKnockbackInfo( infoPack );
    }

    protected override void UpdateDirection(){
        if( !isFaceLocked ) base.UpdateDirection();
    }


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
            m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        }
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            CommonValues.PlayerVelocity = new Vector2();
            if( isFaceLocked ) m_FloorDetector.Move( new Vector2( 0.001f, 0) * (int)savedDir);
            Debug.Log( isFaceLocked );

            m_animator.ResetTrigger( "BiesHurt" );

            if( !m_FloorDetector.isOnGround() ){
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

        m_FloorDetector.Move(CommonValues.PlayerVelocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
