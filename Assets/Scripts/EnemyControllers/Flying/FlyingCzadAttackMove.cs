﻿using System.Collections;
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
    public override void UpdateAnimator(){
        m_dir = (GlobalUtils.Direction)Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x);;
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
    }

    public override void Process(){
        ProcessAcceleration();

        Debug.Log( targetPostion);

        if( targetPostion != -1){
            leftToMove = entityScript.airNavPoints[targetPostion] - (Vector2)m_FloorDetector.GetComponent<Transform>().position;
        }else{
            leftToMove -= entityScript.velocity * Time.deltaTime;
        }

        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);

        SelectNextState();
    }

    protected override void ProcessAcceleration(){
    //    if( Mathf.Abs( entityScript.velocity.x ) == entityScript.maxMoveSpeed) return;
        float acceleration = (entityScript.maxFlyingSpeed / entityScript.moveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( entityScript.velocity.magnitude + acceleration,  entityScript.maxFlyingSpeed );

        entityScript.velocity = leftToMove.normalized * currentValue;

        //entityScript.velocity.x = currentValue * (int)m_dir;
    }

}
