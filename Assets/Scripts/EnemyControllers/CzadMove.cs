using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadMove : EnemyBaseState
{

    Vector2 leftToMove = new Vector2();

    IPlatformEdgeDetector  m_edgeDetector;
    ICollisionWallDetector m_wallDetector;

    public CzadMove( GameObject controllable, Vector2 moveVector ) : base( controllable ){
        name = "CzadMove";
        
        m_edgeDetector = controllable.GetComponent<Transform>().Find("Detector").GetComponent<IPlatformEdgeDetector>();
        m_wallDetector = controllable.GetComponent<Transform>().Find("Detector").GetComponent<ICollisionWallDetector>();
        m_FloorDetector.Move( new Vector2(0.01f * Mathf.Sign( moveVector.x ), 0 ));
        leftToMove = moveVector;
    }

    public void SelectNextState(){
        if( Mathf.Abs( leftToMove.x ) < 100 ) m_isOver = true;
        if( m_edgeDetector.hasReachedPlatformEdge( ) ) {
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
            if( isLeftOriented()  ){ entityScript.patrolRangeLeft  = entityScript.patrolRangeLeft  - leftToMove.x; }
            if( isRightOriented() ){ entityScript.patrolRangeRight = entityScript.patrolRangeRight - leftToMove.x; }
        }
        entityScript.velocity.x = 0;
    }

    public override void Process(){
        base.Process();
        ProcessAcceleration();

        leftToMove -= entityScript.velocity * Time.deltaTime;
        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);

        SelectNextState();
    }

    public override void UpdateAnimator(){
        base.UpdateAnimator();
        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs( entityScript.velocity.x ));
    }

    private void ProcessAcceleration(){
        if( Mathf.Abs( entityScript.velocity.x ) == entityScript.maxMoveSpeed) return;
        float acceleration = (entityScript.maxMoveSpeed / entityScript.moveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( entityScript.velocity.x ) + acceleration,  entityScript.maxMoveSpeed );
        entityScript.velocity.x = currentValue * (int)m_dir;
    }

}
