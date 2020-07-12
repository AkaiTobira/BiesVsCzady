using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadStun : EnemyBaseState{    
    private float timeToEnd;

    private float knocBackDirection;

    GlobalUtils.Direction saveDir;


    public CzadStun( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "CzadStun";

        Debug.Log( "CZAD STUUN ");

        saveDir = m_FloorDetector.GetCurrentDirection();

        m_dir = infoPack.fromCameAttack;
        knocBackDirection = (int)infoPack.fromCameAttack;

        fillKnockbackInfo( infoPack );
        timeToEnd = infoPack.stunDuration;
        m_animator.SetTrigger( "GetStun" );
        m_animator.SetFloat( "StunDuration", timeToEnd);

    }

    public override void UpdateAnimator(){
        m_dir = saveDir;
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
    //    Debug.Log( entityScript.velocity + " fillKnockback before ");
        entityScript.velocity   = infoPack.knockBackValue * entityScript.massFactor;
        entityScript.velocity.x *= (int)infoPack.fromCameAttack;
    //    Debug.Log( entityScript.velocity + " fillKnockback after " + infoPack.fromCameAttack.ToString());
     //   if(Mathf.Abs( CommonValues.PlayerVelocity.x ) > infoPack.knockBackValue.x ) 
    //        velocity.x = (int)infoPack.fromCameAttack * Mathf.Abs( CommonValues.PlayerVelocity.x );


        if( entityScript.hurtSpeedDropFrictionX > 0){
            m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        }
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        m_animator.SetFloat( "StunDuration", timeToEnd);
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.ResetTrigger( "GetStun" );

        }
    }

    private void ProcessMove(){
        entityScript.velocity.y += -entityScript.gravityForce * Time.deltaTime;

    // /    Debug.Log( entityScript.velocity.ToString() + "Before if");

        m_FloorDetector.Move(entityScript.velocity *Time.deltaTime);

        if( m_FloorDetector.isOnGround() ){
            if( knocBackDirection == -1  ) {
                entityScript.velocity.x = Mathf.Min(entityScript.velocity.x + (entityScript.hurtSpeedDropFrictionX * Time.deltaTime), 0);
            }else{
                entityScript.velocity.x = Mathf.Max(entityScript.velocity.x - (entityScript.hurtSpeedDropFrictionX * Time.deltaTime), 0);
            }
        }

        

  //      Debug.Log( entityScript.velocity.ToString() + "afeter if");




//        Debug.Log( entityScript.velocity.ToString() + "afeter move");

    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
}
