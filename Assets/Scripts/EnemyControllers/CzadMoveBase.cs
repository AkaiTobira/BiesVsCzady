using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadMoveBase : EnemyBaseState
{

    protected Vector2 leftToMove = new Vector2();

    protected IPlatformEdgeDetector  m_edgeDetector;
    protected ICollisionWallDetector m_wallDetector;

    public CzadMoveBase( GameObject controllable, Vector2 moveVector ) : base( controllable ){

        m_edgeDetector = controllable.GetComponent<Transform>().Find("Detector").GetComponent<IPlatformEdgeDetector>();
        m_wallDetector = controllable.GetComponent<Transform>().Find("Detector").GetComponent<ICollisionWallDetector>();
        m_FloorDetector.Move( new Vector2(0.01f * Mathf.Sign( moveVector.x ), 0 ));
        leftToMove = moveVector;

        m_dir = (GlobalUtils.Direction)  Mathf.Sign( leftToMove.x );

        if( Mathf.Abs(entityScript.velocity.x) > entityScript.maxMoveSpeed ){
            entityScript.velocity.x = entityScript.maxMoveSpeed * Mathf.Sign( entityScript.velocity.x);
        }

//        Debug.Log( leftToMove + "  :: " + moveVector + " :: " + m_FloorDetector.GetCurrentDirection().ToString() + " BaseClass");
    }

    public virtual void SelectNextState(){}

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

    protected virtual void ProcessAcceleration(){
    //    if( Mathf.Abs( entityScript.velocity.x ) == entityScript.maxMoveSpeed) return;
        float acceleration = (entityScript.maxMoveSpeed / entityScript.moveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( entityScript.velocity.x ) + acceleration,  entityScript.maxMoveSpeed );
        entityScript.velocity.x = currentValue * (int)m_dir;
    }
}
