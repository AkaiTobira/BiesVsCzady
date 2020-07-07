using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPlayerDetected : EnemyBaseState
{
    public CzadPlayerDetected( GameObject controllable ) : base( controllable ){
        name = "CzadPlayerDetected";
    }

    float meeleCombatTimer = 3;

    private bool CanMeeleAttack(){
        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( distance > entityScript.combatRange ) return false;
        if( meeleCombatTimer > 0 )                return false;
        meeleCombatTimer = entityScript.breakBeetweenAttacks;
        return true;
    }


    public void SelectNextState(){
        meeleCombatTimer -= Time.deltaTime;
        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, 
                                            m_FloorDetector.GetComponent<Transform>().position);

        if( distance > 2000 ) {
            entityScript.ResetPatrolValues();
            m_isOver                       = true;
            entityScript.isAlreadyInCombat = false;
        }else if(distance > entityScript.combatRange ){
            var direction = (GlobalUtils.PlayerObject.transform.position - m_FloorDetector.GetComponent<Transform>().position).normalized;
            m_nextState = new CzadAttackMove( m_controllabledObject, direction * 100);
        }else{
            if( CanMeeleAttack() ){
                m_nextState = new CzadAttackMelee( m_controllabledObject );
            }
        }
    }

    public override void Process(){
        base.Process();
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
//        Debug.Log( currentValue * (int)m_FloorDetector.GetCurrentDirection() );
        entityScript.velocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

}
