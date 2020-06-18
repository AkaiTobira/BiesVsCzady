using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesMove : BaseState
{
    private bool isMovingLeft = false;
    private bool isAccelerating = true;

    public BiesMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "BiesMove";
        CommonValues.PlayerVelocity.x = 0;
        m_dir = dir;

        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }
   public override void OnExit(){}

    public override void Process(){
        HandleAcceleration();
    //    CommonValues.PlayerVelocity.x = BiesUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
        if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) CommonValues.PlayerVelocity.x = 0.0f;
        if( !isMovingLeft && m_detector.isCollideWithRightWall()) CommonValues.PlayerVelocity.x = 0.0f;

        CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }

        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        if( m_detector.isWallClose() ){
            m_nextState = new BiesWallHold( m_controllabledObject, 
                                              ( isMovingLeft )? GlobalUtils.Direction.Left : 
                                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
    }

    private void HandleAcceleration(){
        if( ! isAccelerating ) return;
        float acceleration = (BiesUtils.PlayerSpeed / BiesUtils.MoveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( CommonValues.PlayerVelocity.x) + acceleration, BiesUtils.PlayerSpeed );
        CommonValues.PlayerVelocity.x = currentValue * (int)m_dir;
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
        }else if( isMovingLeft && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerInput.isMoveRightKeyHold()){
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
