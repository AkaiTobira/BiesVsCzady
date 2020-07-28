using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadAttackMove : CzadMoveBase
{
    public CzadAttackMove( GameObject controllable, Vector2 moveVector ) : base( controllable, moveVector ){
        name = "CzadAttackMove";
    }

    public override void SelectNextState(){
        if( Mathf.Abs( leftToMove.x ) < 1 ) m_isOver = true;
        if( m_edgeDetector.hasReachedPlatformEdge( ) ) {
            m_isOver = true;
        }
    }

}
