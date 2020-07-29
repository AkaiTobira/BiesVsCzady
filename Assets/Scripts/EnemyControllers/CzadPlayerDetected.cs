using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPlayerDetected : EnemyBaseState
{
    float meeleCombatTimer;
    float rangeCombatTimer;

    float jumpAttackTimer;

    public CzadPlayerDetected( GameObject controllable ) : base( controllable ){
        name = "CzadPlayerDetected";
        meeleCombatTimer = entityScript.delayOfFirstAttack;
        rangeCombatTimer = entityScript.delayOfFirstAttack;
        GlobalUtils.TaskMaster.EnemyTriggered();
    }
    
    private bool CanMeeleAttack(){
        float distance = Mathf.Abs( GlobalUtils.PlayerObject.transform.position.x - m_FloorDetector.GetComponent<Transform>().position.x) - 4;
        Debug.Log( distance > entityScript.combatRange );
        if( distance > entityScript.combatRange ) return false;
        if( meeleCombatTimer > 0 )                return false;
        meeleCombatTimer = entityScript.breakBeetweenAttacks;
        return entityScript.canMeeleAttack;
    }

    private bool CanRageJump(){
        if( jumpAttackTimer > 0) return false;
        jumpAttackTimer = entityScript.jumpAttackBreak;
        return entityScript.canJumpOnPlayer;
    }

    private bool CanShot(){
        if( rangeCombatTimer > 0 ) return false;
        rangeCombatTimer = entityScript.breakBeetweenShots;
        return entityScript.canShot;
    }

    public void SelectNextState(){
        meeleCombatTimer -= Time.deltaTime;
        rangeCombatTimer -= Time.deltaTime;
        jumpAttackTimer  -= Time.deltaTime;

        Vector3 playerPosition   = GlobalUtils.PlayerObject.transform.position;
        Vector3 detectorPosition = m_FloorDetector.GetComponent<Transform>().position;
        Vector3 playerCombatSide = playerPosition + new Vector3 (entityScript.combatRange * 
                                            (float)GlobalUtils.GetClosestSideToPosition(playerPosition,
                                                                                        detectorPosition), 0);


        float distance = Vector3.Distance( playerPosition, detectorPosition);

        if( distance > 3000 ) {
            entityScript.ResetPatrolValues();
            m_isOver                       = true;
            entityScript.isAlreadyInCombat = false;
            GlobalUtils.TaskMaster.EnemyIsOutOfCombat();

        }else if( distance < entityScript.shotRange && entityScript.probabilityOfShot/Random.Range( 1.0f, 11.0f) > 1.0f  ){
            if( CanShot()){
                m_nextState = new CzadShot( m_controllabledObject );
            }
        }
        else if( CanMeeleAttack() ){
                m_nextState = new CzadAttackMelee( m_controllabledObject );
            }
        else if( CanRageJump()){

        }else if( Mathf.Abs(playerPosition.x  - detectorPosition.x) > entityScript.combatRange && entityScript.canMeeleAttack ){

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
