using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : PlayerBaseState
{
    private bool isAccelerating = false;

    public CatMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "CatMove";
        CommonValues.PlayerVelocity.y = 0;
        m_dir = dir;
        SetUpRotation();
    }

    protected override void  SetUpAnimation(){
        if( CommonValues.needChangeDirection ){
            m_animator.SetTrigger( "CatChangingDirection");
            CommonValues.needChangeDirection = false;
        }
    }

    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    private void ProcessGravity(){
        CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -CatUtils.GravityForce * Time.deltaTime;
        }
    }

    private void ProcessAcceleration(){
        if( ! isAccelerating ) return;
        float acceleration = (CatUtils.PlayerSpeed / CatUtils.MoveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( CommonValues.PlayerVelocity.x) + acceleration, CatUtils.PlayerSpeed);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_dir;
    }

    private void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", -2);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));
    }
    public override void Process(){
        ProcessAcceleration();
        ProcessAnimationUpdate();
        ProcessWallDetectiong();
        ProcessGravity();

        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        ProcessStateEnd();
    }

    private void ProcessWallDetectiong(){
        if( isLeftOriented()   && m_detector.isCollideWithLeftWall() ) CommonValues.PlayerVelocity.x = 0.0f;
        if( isRightOriented()  && m_detector.isCollideWithRightWall()) CommonValues.PlayerVelocity.x = 0.0f;
    }

    private void ProcessStateEnd(){
        if(  m_detector.isWallClose() && 
            ( m_detector.isCollideWithLeftWall() || m_detector.isCollideWithRightWall() ) ){
            m_nextState = new CatWallHold( m_controllabledObject, 
                                         ( isLeftOriented() ) ? 
                                                GlobalUtils.Direction.Left : 
                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
        if( m_isOver ){
            m_animator.ResetTrigger( "CatChangingDirection");
        }
    }

    private void HandleInputAcceleration(){
        if( PlayerInput.isMoveLeftKeyHold() || PlayerInput.isMoveRightKeyHold()) {
            isAccelerating = true;
        }
    }

    public override void HandleInput(){
        HandleInputAcceleration();

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new CatFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( isLeftOriented()  && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( isRightOriented() && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_isOver    = true;
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() && m_detector.canFallByFloor() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

}
