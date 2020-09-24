using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadDead : EnemyBaseState{    
    private float velocitXFriction = 0.0f;

    private float knocBackDirection;

    public CzadDead( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        name = "CzadDead";

        m_dir = infoPack.fromCameAttack;
        knocBackDirection = (int)infoPack.fromCameAttack;

        fillKnockbackInfo( infoPack );
        entityScript.toDeadTimer = 1f;
        m_animator.SetTrigger( "isDead" );


        GlobalUtils.TaskMaster.EnemyIsOutOfCombat();
        Debug.Log( "DeadStateCalled ");
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        entityScript.velocity   = infoPack.knockBackValue * entityScript.massFactor;
        entityScript.velocity.x *= (int)infoPack.fromCameAttack;

     //   if(Mathf.Abs( CommonValues.PlayerVelocity.x ) > infoPack.knockBackValue.x ) 
    //        velocity.x = (int)infoPack.fromCameAttack * Mathf.Abs( CommonValues.PlayerVelocity.x );
 
        velocitXFriction  = infoPack.knockBackFrictionX;

        if( velocitXFriction > 0){
            m_FloorDetector.CheatMove( new Vector2(0,4.0f));
        }
    }

    private void ProcessMove(){
//  /       m_animator.SetFloat( "FallVelocity", m.PlayerVelocity.y);
    //  /   PlayerFallHelper.FallRequirementsMeet( true );
        entityScript.velocity.y += -entityScript.gravityForce * Time.deltaTime;

        if( knocBackDirection == -1 ) {
            entityScript.velocity.x = entityScript.velocity.x - velocitXFriction * Time.deltaTime;
        }else{
            entityScript.velocity.x = entityScript.velocity.x + velocitXFriction * Time.deltaTime;
        }

        m_FloorDetector.Move(entityScript.velocity *Time.deltaTime);
    }

    public override void Process(){
        ProcessMove();
    }
}
