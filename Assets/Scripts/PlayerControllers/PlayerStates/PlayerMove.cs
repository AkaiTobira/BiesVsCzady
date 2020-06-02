using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : BaseState
{
    private bool isMovingLeft = false;

    public PlayerMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "Move";
    //    m_dir = dir;
    }

    public override void Process(){
        velocity.x = PlayerUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
        if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) velocity.x = 0.0f;
        if( !isMovingLeft && m_detector.isCollideWithRightWall()) velocity.x = 0.0f;

        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            PlayerUtils.ResetStamina();
            velocity.y = -PlayerUtils.GravityForce * Time.deltaTime;
        }

        m_detector.Move(velocity * Time.deltaTime);

        if( m_detector.isWallClose() ){
            m_nextState = new PlayerWallHold( m_controllabledObject, 
                                              ( isMovingLeft )? GlobalUtils.Direction.Left : 
                                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }

    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new PlayerFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new PlayerAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new PlayerAttack2(m_controllabledObject);
        }else if( PlayerInput.isAttack3KeyPressed() ){
            m_nextState = new PlayerAttack3(m_controllabledObject);
        }else if( isMovingLeft && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

}
