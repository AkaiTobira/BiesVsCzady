using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesMove : PlayerBaseState
{
    private bool isAccelerating = true;
    public BiesMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "BiesMove";
//        CommonValues.PlayerVelocity.x = 0;
        m_dir = dir;
        SetUpRotation();
    }

    protected override void  SetUpAnimation(){
        if( CommonValues.needChangeDirection ){
            m_animator.SetTrigger( "BiesChangingDirection");
            CommonValues.needChangeDirection = false;
        }
    }

    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    private void ProcessGravity(){
        CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }
    }


    private void ProcessAcceleration(){
        if( ! isAccelerating ) return;
        float acceleration = (BiesUtils.PlayerSpeed / BiesUtils.MoveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( CommonValues.PlayerVelocity.x) + acceleration, BiesUtils.PlayerSpeed );
        CommonValues.PlayerVelocity.x = currentValue * (int)m_dir;
    }

    private void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", 0);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));
    }

    private void ProcessWallDetectiong(){
        if( isLeftOriented()   && m_detector.isCollideWithLeftWall() ) CommonValues.PlayerVelocity.x = 0.0f;
        if( isRightOriented()  && m_detector.isCollideWithRightWall()) CommonValues.PlayerVelocity.x = 0.0f;
    }

    public override void Process(){
        ProcessAcceleration();
        ProcessAnimationUpdate();
        ProcessWallDetectiong();
        ProcessGravity();


        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        ProcessStateEnd();
    }

    private void ProcessStateEnd(){
        if(  m_detector.isWallClose() && 
            ( m_detector.isCollideWithLeftWall() || m_detector.isCollideWithRightWall() ) ){
            m_nextState = new BiesWallHold( m_controllabledObject, 
                                         ( isLeftOriented() ) ? 
                                                GlobalUtils.Direction.Left : 
                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
        if( m_isOver ){
            m_animator.ResetTrigger( "BiesChangingDirection");
        }
    }
    public override void HandleInput(){
        if( PlayerInput.isMoveLeftKeyHold() || PlayerInput.isMoveRightKeyHold()) {
            isAccelerating = true;
        }

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if( PlayerInput.isAttack3KeyPressed() ){
            m_nextState = new BiesAttack3(m_controllabledObject);
        }else if( isLeftOriented()  && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( isRightOriented() && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() && m_detector.canFallByFloor() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

}
