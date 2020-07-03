using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPlayerDetected : EnemyBaseState
{
    public CzadPlayerDetected( GameObject controllable ) : base( controllable ){
        name = "CzadPlayerDetected";
    }

    public void SelectNextState(){
        if( entityScript.velocity.x != 0 ) return; 

        float distance = Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position);

        if( distance  < 300){
            m_nextState = new CzadAttack( m_controllabledObject );
        }else{
            
            float direction = (GlobalUtils.PlayerObject.transform.position.x < 
                                m_controllabledObject.transform.position.x) ? -1 : 1;

            m_nextState = new CzadMove(m_controllabledObject, new Vector2( distance * direction, 0) );
        }

    }

    public override void Process(){
        base.Process();
        HandleStopping();
        SelectNextState();
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
