using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadAttackMove : CzadMoveBase
{
    public CzadAttackMove( GameObject controllable, Vector2 moveVector ) : base( controllable, moveVector ){
        name = "CzadAttackMove";
        Debug.Log( entityScript.velocity);
    }

    public override void SelectNextState(){
        if( Mathf.Abs( leftToMove.x ) < 10 ) m_isOver = true;
        if( m_edgeDetector.hasReachedPlatformEdge( ) ) {
            //AdaptPatrolRange();
            m_isOver = true;
        }

       // if( Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position) < 400){
      //      m_nextState = new CzadPlayerDetected( m_controllabledObject );
    //    }else if( Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position) < 100){
  //          m_nextState = new CzadAttack( m_controllabledObject );
//        }


    }

}
