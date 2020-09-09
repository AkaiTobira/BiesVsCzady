using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserPlayerDetected : EnemyBaseState
{
    float chaseCombatTimer;

    public ChaserPlayerDetected( GameObject controllable ) : base( controllable ){
        name = "ChaserPlayerDetected";
        chaseCombatTimer = entityScript.delayOfFirstAttack;
        GlobalUtils.TaskMaster.EnemyTriggered();
    }
    

    private bool CanCharge(){
        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( chaseCombatTimer > 0 ) return false;
        if( distance - entityScript.combatRange > 5 ) return false;
        
        chaseCombatTimer  = entityScript.breakBeetweenAttacks;
        forcedChargeTimer = 0;
        return true;
    }

    private float forcedChargeTimer = 0;

    public void SelectNextState(){
        chaseCombatTimer -= Time.deltaTime;


        Vector3 playerPosition   = GlobalUtils.PlayerObject.transform.position;
        Vector3 detectorPosition = m_FloorDetector.GetComponent<Transform>().position;
        Vector3 playerCombatSide = playerPosition + new Vector3 (entityScript.combatRange * 
                                            (float)GlobalUtils.GetClosestSideToPosition(playerPosition,
                                                                                        detectorPosition), 0);

        float distance = Vector3.Distance( playerPosition, detectorPosition);

        if( !entityScript.isAlreadyInCombat ) {
        //    entityScript.ResetPatrolValues();
            m_isOver                       = true;
        //    entityScript.isAlreadyInCombat = false;
            GlobalUtils.TaskMaster.EnemyIsOutOfCombat();

        }else if( distance > 300 ){
            forcedChargeTimer += Time.deltaTime;
            if( forcedChargeTimer > 2.5f){
                m_nextState      = new ChaserChaseAttack(m_controllabledObject);
                chaseCombatTimer = entityScript.breakBeetweenAttacks;
            }
        }else if( CanCharge() ){
//
            m_nextState = new ChaserChaseAttack(m_controllabledObject);

        }else if( Mathf.Abs(playerPosition.x  - detectorPosition.x) > entityScript.combatRange ){

            Vector3 targetPosition = playerPosition + 
                                new Vector3( entityScript.combatRange * 
                                            (float)GlobalUtils.GetClosestSideToPosition(playerPosition,
                                                                                        detectorPosition ), 0 );
            targetPosition = targetPosition - detectorPosition;

            m_nextState = new CzadAttackMove( m_controllabledObject, targetPosition );
        }
    }
    public override void Process(){
        m_FloorDetector.Move( new Vector2(0.0000001f, 0 ) * Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x));
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
