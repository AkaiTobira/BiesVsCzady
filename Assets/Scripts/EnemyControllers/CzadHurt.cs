using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadHurt : EnemyBaseState{    
    private float timeToEnd;

    private float knocBackDirection;

    GlobalUtils.Direction saveDir;


    public CzadHurt( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "CzadHurt";

        saveDir = m_FloorDetector.GetCurrentDirection();

        m_dir = infoPack.fromCameAttack;
        knocBackDirection = (int)infoPack.fromCameAttack;

        fillKnockbackInfo( infoPack );
        timeToEnd = entityScript.timeOfBeeingHurt;
        m_animator.SetTrigger( "GetHit" );
    }

    public override void UpdateAnimator(){
        m_dir = saveDir;
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        entityScript.velocity   = infoPack.knockBackValue * entityScript.massFactor;
        entityScript.velocity.x *= (int)infoPack.fromCameAttack;

        if( entityScript.hurtSpeedDropFrictionX > 0){
            m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        }
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.ResetTrigger( "GetHit" );

        }
    }

    private void ProcessMove(){
        entityScript.velocity.y += -entityScript.gravityForce * Time.deltaTime;
        m_FloorDetector.Move(entityScript.velocity *Time.deltaTime);

        if( m_FloorDetector.isOnGround() ){
            if( knocBackDirection == -1  ) {
                entityScript.velocity.x = Mathf.Min(entityScript.velocity.x + (entityScript.hurtSpeedDropFrictionX * Time.deltaTime), 0);
            }else{
                entityScript.velocity.x = Mathf.Max(entityScript.velocity.x - (entityScript.hurtSpeedDropFrictionX * Time.deltaTime), 0);
            }
        }
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
}
