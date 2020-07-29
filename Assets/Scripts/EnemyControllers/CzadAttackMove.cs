using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadAttackMove : CzadMoveBase
{

    Vector3 targetPosition = new Vector3();

    public CzadAttackMove( GameObject controllable, Vector2 moveVector ) : base( controllable, moveVector ){
        name = "CzadAttackMove";
    }

    public override void SelectNextState(){

        if( Mathf.Abs(leftToMove.x) < 3){
            targetPosition.y =  m_FloorDetector.GetComponent<Transform>().position.y;
            m_FloorDetector.GetComponent<Transform>().position = targetPosition;

            entityScript.velocity = new Vector2();
            m_isOver = true;
        }
        if( m_edgeDetector.hasReachedPlatformEdge()){
            entityScript.velocity = new Vector2();
            m_isOver = true;
        }
    }

    private void  ProcessGravity(){
        if( !m_FloorDetector.isOnGround() ){
            entityScript.velocity.y -= entityScript.gravityForce * Time.deltaTime;
            m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);
        }else{
            entityScript.velocity.y = 0;
        }
    }

    public override void Process(){

        ProcessGravity();
        ProcessAcceleration();

        float sidePosition = (float)GlobalUtils.GetClosestSideToPosition( GlobalUtils.PlayerObject.transform.position, 
                                                                    m_FloorDetector.GetComponent<Transform>().position);

        targetPosition = GlobalUtils.PlayerObject.transform.position + new Vector3( entityScript.combatRange * sidePosition, 0 );
        leftToMove = targetPosition - m_FloorDetector.GetComponent<Transform>().position;

        m_dir = (GlobalUtils.Direction)  Mathf.Sign( leftToMove.x );


        DebugDrawHelper.DrawX( targetPosition);
        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);
        SelectNextState();
    }

}
