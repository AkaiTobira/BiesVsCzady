using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPlayerDetected : EnemyBaseState
{
    float meeleCombatTimer;
    float rangeCombatTimer;
    public CzadPlayerDetected( GameObject controllable ) : base( controllable ){
        name = "CzadPlayerDetected";
        meeleCombatTimer = entityScript.delayOfFirstAttack;
        rangeCombatTimer = entityScript.delayOfFirstAttack;
    }
    
    private bool CanMeeleAttack(){
        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( distance > entityScript.combatRange ) return false;
        if( meeleCombatTimer > 0 )                return false;
        meeleCombatTimer = entityScript.breakBeetweenAttacks;
        return entityScript.canMeeleAttack;
    }

    private bool CanShot(){
        if( rangeCombatTimer > 0 ) return false;
        meeleCombatTimer = entityScript.breakBeetweenShots;
        return entityScript.canShot;
    }

    public void SelectNextState(){
        meeleCombatTimer -= Time.deltaTime;
        rangeCombatTimer -= Time.deltaTime;

        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( distance > 4000 ) {
            entityScript.ResetPatrolValues();
            m_isOver                       = true;
            entityScript.isAlreadyInCombat = false;
        }else if( distance < entityScript.shotRange && entityScript.probabilityOfShot/Random.Range( 1.0f, 11.0f) > 1.0f  ){
            if( CanShot()){
                m_nextState = new CzadShot( m_controllabledObject );
            }
        }else if(distance > entityScript.combatRange && entityScript.canMeeleAttack ){
            var direction = (GlobalUtils.PlayerObject.transform.position - m_FloorDetector.GetComponent<Transform>().position).normalized;
            m_nextState = new CzadAttackMove( m_controllabledObject, direction * 100);
        }else{
            if( CanMeeleAttack() ){
                m_nextState = new CzadAttackMelee( m_controllabledObject );
            } 
        }
    }

    public override void Process(){
        if( entityScript.isPositionLocked ){
            m_FloorDetector.Move( new Vector2(0.0000001f, 0 ) * Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x));
        }

        HandleStopping();
        SelectNextState();
        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);
    }

    public override void UpdateAnimator(){
        base.UpdateAnimator();
        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs( entityScript.velocity.x ));
    }

    private void HandleStopping(){
        float acceleration      = (entityScript.maxMoveSpeed / entityScript.moveBrakingTime) * Time.deltaTime;
        float currentValue      = Mathf.Max( Mathf.Abs( entityScript.velocity.x ) - acceleration, 0);
        entityScript.velocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

}
