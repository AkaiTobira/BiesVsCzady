using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadPatrolMove : CzadMoveBase
{

    public CzadPatrolMove( GameObject controllable, Vector2 moveVector ) : base( controllable, moveVector ){
        name = "CzadPatrolMove";
    }

    public override void SelectNextState(){

        if( Mathf.Abs( leftToMove.x ) < 10 ){
            m_isOver = true;
        }
        if( m_edgeDetector.hasReachedPlatformEdge( ) ) {
            entityScript.velocity.x = 0;
        //    AdaptPatrolRange();
            m_isOver = true;
        }

        if( m_wallDetector.isCollideWithRightWall() && isRightOriented()  ){
        //    AdaptPatrolRange();
            m_isOver = true;
            entityScript.velocity.x = 0;
        }

        if( m_wallDetector.isCollideWithLeftWall() && isLeftOriented()  ){
        //    AdaptPatrolRange();
            m_isOver = true;
            entityScript.velocity.x = 0;
        }
/*
        if( m_wallDetector.GetDistanceToClosestWallFront() < 50 ){
            m_isOver = true;
            Debug.Log("WallIsToClose");
        }
*/

    }

    private void AdaptPatrolRange(){
        if( entityScript.canPatrol ){
            if( isRightOriented() ){ entityScript.autoCorrectionLeft  = leftToMove.x; }
            if( isLeftOriented () ){ entityScript.autoCorrectionRight = leftToMove.x; }
            entityScript.velocity.x = 0;
        }
    }

}
