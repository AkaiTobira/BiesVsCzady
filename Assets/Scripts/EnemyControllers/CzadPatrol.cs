using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPatrol : EnemyBaseState
{

    enum Stage{
        StartLeft,
        StartRight,
        BackL,
        BackR,
    }

    List<Stage> nextMoves = new List<Stage>();

    public CzadPatrol( GameObject controllable ) : base( controllable ){
        name = "CzadPatrol";

        nextMoves.Add ( (Random.Range( 0, 2) == 1) ? Stage.StartLeft : Stage.StartRight );

    }

    public void SelectNextState(){
        if( nextMoves.Count == 0 ) m_isOver = true;
        else if( entityScript.velocity.x != 0 ) HandleStopping();
        else{
            if( nextMoves[0] == Stage.StartLeft ){
                m_nextState = new CzadMove( m_controllabledObject, new Vector2( entityScript.patrolRangeLeft, 0 ) );
                nextMoves.Add( Stage.BackL );
            }else if( nextMoves[0] == Stage.StartRight  ){
                m_nextState = new CzadMove( m_controllabledObject, new Vector2( entityScript.patrolRangeRight, 0 ) );
                nextMoves.Add( Stage.BackR );
            }else if( nextMoves[0] == Stage.BackL  ){
                m_nextState = new CzadMove( m_controllabledObject, new Vector2( entityScript.patrolRangeRight - entityScript.patrolRangeLeft, 0 ) );
                nextMoves.Add( Stage.BackR );
            }else if( nextMoves[0] == Stage.BackR  ){
                m_nextState = new CzadMove( m_controllabledObject, new Vector2( -entityScript.patrolRangeRight + entityScript.patrolRangeLeft, 0 ) );
                nextMoves.Add( Stage.BackL );
            }
            nextMoves.RemoveAt(0);
        }

     //   Debug.Log(  Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position) );

     //   if( Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position) < 400){
     //       m_nextState = new CzadPlayerDetected( m_controllabledObject );
    //    }

    }

    public override void Process(){
        base.Process();
        SelectNextState();
    }

    private void HandleStopping(){
        float acceleration      = (entityScript.maxMoveSpeed / entityScript.moveBrakingTime) * Time.deltaTime;
        float currentValue      = Mathf.Max( Mathf.Abs( entityScript.velocity.x ) - acceleration, 0);
        entityScript.velocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

    public override void UpdateAnimator(){
        base.UpdateAnimator();
        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs( entityScript.velocity.x ));
    }

}
