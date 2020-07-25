using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCzadAttackMove : CzadMoveBase
{
    //Should be divided in two : for flying up, and following navPoint
    new protected FlyingAkaiController entityScript = null;

    int targetPostion;
    public FlyingCzadAttackMove( GameObject controllable, Vector2 moveVector, int airNavPositionIndex ) : base( controllable, moveVector ){
        name = "FlyingCzadAttackMove";
        if( airNavPositionIndex != -1){
            targetPostion = airNavPositionIndex;
        }
        entityScript = controllable.GetComponent<FlyingAkaiController>();
    }

    public override void SelectNextState(){
        if( leftToMove.magnitude < 10){
            entityScript.velocity = new Vector2();
            m_isOver = true;
        }
    }
    public override void UpdateAnimator(){
        m_dir = (GlobalUtils.Direction)Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x);;
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
    }

    public override void Process(){

        m_animator.SetBool("isGliding", false);

        ProcessAcceleration();

        if( targetPostion != -1){
            leftToMove = entityScript.airNavPoints[targetPostion] - (Vector2)m_FloorDetector.GetComponent<Transform>().position;
        }else{
            leftToMove -= entityScript.velocity * Time.deltaTime;
        }

        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);

        SelectNextState();
    }

    protected override void ProcessAcceleration(){
        float acceleration = (entityScript.maxFlyingSpeed / entityScript.moveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( entityScript.velocity.magnitude + acceleration,  entityScript.maxFlyingSpeed );
        entityScript.velocity = leftToMove.normalized * currentValue;
    }
}
