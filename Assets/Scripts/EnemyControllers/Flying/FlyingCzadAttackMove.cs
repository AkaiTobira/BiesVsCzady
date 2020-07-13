using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCzadAttackMove : CzadMoveBase
{
    public FlyingCzadAttackMove( GameObject controllable, Vector2 moveVector ) : base( controllable, moveVector ){
        name = "FlyingCzadAttackMove";
        Debug.Log( moveVector);
    //    Debug.Log( leftToMove + "  :: " + moveVector + " :: " + m_FloorDetector.GetCurrentDirection().ToString() + " INHErIt");
    }

    public override void SelectNextState(){
        if( leftToMove.magnitude < 10){
            entityScript.velocity = new Vector2();
            m_isOver = true;
        } 
    
    //    if( Mathf.Abs( leftToMove.x ) < 10 ) m_isOver = true;
    //    if( m_edgeDetector.hasReachedPlatformEdge( ) ) {
            //AdaptPatrolRange();
  //          m_isOver = true;
//        }

    }

    public override void Process(){
        ProcessAcceleration();

        leftToMove -= entityScript.velocity * Time.deltaTime;
        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);

        SelectNextState();
    }

    protected virtual void ProcessAcceleration(){
    //    if( Mathf.Abs( entityScript.velocity.x ) == entityScript.maxMoveSpeed) return;
        float acceleration = (entityScript.maxMoveSpeed / entityScript.moveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( entityScript.velocity.magnitude + acceleration,  entityScript.maxMoveSpeed );

        entityScript.velocity = leftToMove.normalized * currentValue;

        //entityScript.velocity.x = currentValue * (int)m_dir;
    }

}
