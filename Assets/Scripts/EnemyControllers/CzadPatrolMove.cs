using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPatrolMove : CzadMoveBase
{

    public CzadPatrolMove( GameObject controllable, Vector2 moveVector ) : base( controllable, moveVector ){
        name = "CzadPatrolMove";
    }

    public override void SelectNextState(){

        if( Mathf.Abs( leftToMove.x ) < 100 ){
            m_isOver = true;
        }
        if( m_edgeDetector.hasReachedPlatformEdge( ) ) {
            entityScript.velocity.x = 0;
            AdaptPatrolRange();
            m_isOver = true;
        }

        if( m_wallDetector.isCollideWithRightWall() && isRightOriented()  ){
            AdaptPatrolRange();
            m_isOver = true;
        }

        if( m_wallDetector.isCollideWithLeftWall() && isLeftOriented()  ){
            AdaptPatrolRange();
            m_isOver = true;
        }

       // if( Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position) < 400){
      //      m_nextState = new CzadPlayerDetected( m_controllabledObject );
    //    }else if( Vector3.Distance( GlobalUtils.PlayerObject.transform.position, m_controllabledObject.transform.position) < 100){
  //          m_nextState = new CzadAttack( m_controllabledObject );
//        }


    }

    private void AdaptPatrolRange(){
        if( entityScript.canPatrol ){
            if( isRightOriented() ){ entityScript.autoCorrectionLeft  = leftToMove.x; }
            if( isLeftOriented () ){ entityScript.autoCorrectionRight = leftToMove.x; }
            entityScript.velocity.x = 0;
        }
    }

}
