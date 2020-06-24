using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : BaseState
{
    private bool isMovingLeft = false;

    private bool isAccelerating = false;
    private GlobalUtils.Direction savedDir;

    public CatMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == GlobalUtils.Direction.Left;

    //    m_dir = dir;

        name = "CatMove";
        CommonValues.PlayerVelocity.x = 0;
        m_dir = dir;
        savedDir = m_dir;

        if( CommonValues.PlayerFaceDirection != m_dir ){
            m_animator.SetTrigger( "CatChangingDirection");
        }
        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
        CommonValues.PlayerFaceDirection = m_dir;
    }


    public override void OnExit(){}

    private void ProcessGravity(){
        CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -CatUtils.GravityForce * Time.deltaTime;
        }
    }


    protected override void UpdateDirection(){

        if( savedDir != m_detector.GetCurrentDirection() ){
            savedDir = m_detector.GetCurrentDirection();
            m_animator.SetTrigger( "CatChangingDirection");
        }

        base.UpdateDirection();
    }

    private void HandleAcceleration(){
        if( ! isAccelerating ) return;
        float acceleration = (CatUtils.PlayerSpeed / CatUtils.MoveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( CommonValues.PlayerVelocity.x) + acceleration, CatUtils.PlayerSpeed);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_dir;
    }

    public override void Process(){

        HandleAcceleration();

        m_animator.SetFloat( "FallVelocity", 0);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));

        if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) CommonValues.PlayerVelocity.x = 0.0f;
        if( !isMovingLeft && m_detector.isCollideWithRightWall()) CommonValues.PlayerVelocity.x = 0.0f;

        ProcessGravity();

        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        if( m_detector.isWallClose() && 
           (m_detector.isCollideWithLeftWall() || m_detector.isCollideWithRightWall() ) ){
            m_nextState = new CatWallHold( m_controllabledObject, 
                                              ( isMovingLeft )? GlobalUtils.Direction.Left : 
                                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
    }

    public override void HandleInput(){
        if( PlayerInput.isMoveLeftKeyHold() || PlayerInput.isMoveRightKeyHold()) {
            isAccelerating = true;
        }

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new CatFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if(  isMovingLeft && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerInput.isMoveRightKeyHold()){
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
