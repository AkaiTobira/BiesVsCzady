using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadIdle : EnemyBaseState
{
    public CzadIdle( GameObject controllable ) : base( controllable ){
        name = "CzadIdle";
    }

    public void SelectNextState(){
        if( entityScript.velocity.x != 0 ) return; 

        //TODO Magnificent random behaviour selector based on random list - in future


        if( entityScript.canPatrol ){
            m_nextState = new CzadPatrol( m_controllabledObject );
        }else{
            float direction = Random.Range( -1, 2);
            while( direction == 0 ) direction = Random.Range( -1, 2);
            float distance  = Random.Range( 0, entityScript.maxMoveDistance);
            m_nextState = new CzadMove( m_controllabledObject, new Vector2( direction * distance, 0));
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
