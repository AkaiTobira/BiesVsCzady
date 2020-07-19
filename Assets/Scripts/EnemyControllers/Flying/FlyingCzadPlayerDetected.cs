using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCzadPlayerDetected : FlyingEnemyBaseState
{

    Vector2 currentActivePosition;
int arrayIndex;
    float meeleCombatTimer;
    public FlyingCzadPlayerDetected( GameObject controllable ) : base( controllable ){
        name = "FlyingCzadPlayerDetected";
        meeleCombatTimer = entityScript.delayOfFirstAttack;
        flyToAirPoint();
    }

    private void flyToAirPoint(){
        entityScript.lockedInAirPostion.x = m_FloorDetector.GetComponent<Transform>().position.x;
        currentActivePosition = entityScript.lockedInAirPostion;
        m_nextState = new FlyingCzadAttackMove( m_controllabledObject, entityScript.lockedInAirPostion - (Vector2)m_FloorDetector.GetComponent<Transform>().position, -1 );
    }


    private bool CanMeeleAttack(){
        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( distance > entityScript.combatRange ) return false;
        if( meeleCombatTimer > 0 ) return false;
        meeleCombatTimer = entityScript.breakBeetweenAttacks;
        return true;
    }

    private int triesToFlyToTarget = 0;
    private float moveCooldown     = 0;

    public override void UpdateAnimator(){
        m_dir = (GlobalUtils.Direction)Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x);
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs( entityScript.velocity.x ));
    }

    public void SelectMoveState(){
        if( moveCooldown > 0) return;

        if( Vector2.Distance( m_FloorDetector.GetComponent<Transform>().position, currentActivePosition ) < 300 || 
            triesToFlyToTarget > 3 
        ){
            arrayIndex = Random.Range(0, entityScript.posiblePositions.Count );
            currentActivePosition = entityScript.airNavPoints[arrayIndex];
            m_nextState = new FlyingCzadAttackMove( m_controllabledObject, 
                                                    currentActivePosition - 
                                                    (Vector2)m_FloorDetector.GetComponent<Transform>().position,
                                                    arrayIndex );
            triesToFlyToTarget = 0;
            moveCooldown = entityScript.moveCooldown;
        }else{
            triesToFlyToTarget += 1;
            m_nextState = new FlyingCzadAttackMove( m_controllabledObject, 
                                                    currentActivePosition - 
                                                    (Vector2)m_FloorDetector.GetComponent<Transform>().position,
                                                    arrayIndex );
        }
        skipFirst = false;
    }
    
    private bool skipFirst = true;

    public void SelectNextState(){
        meeleCombatTimer -= Time.deltaTime;
        moveCooldown     -= Time.deltaTime;
        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( distance > entityScript.playerDropRange){
            entityScript.ResetPatrolValues();
            m_isOver                       = true;
            entityScript.isAlreadyInCombat = false;
        }else{
            if(skipFirst) 
            {
                SelectMoveState();
            }else{
                m_nextState = new FlyingEnemyGliding(m_controllabledObject);
                skipFirst = true;
            }
        }
    }

    public override void Process(){
        HandleStopping();
        SelectNextState();
        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);
    }

    private void HandleStopping(){
        float acceleration      = (entityScript.maxMoveSpeed / entityScript.moveBrakingTime) * Time.deltaTime;
        float currentValue      = Mathf.Max( Mathf.Abs( entityScript.velocity.x ) - acceleration, 0);
        entityScript.velocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

}
