using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : BaseState
{
    private bool isMovingLeft = false;

    public PlayerMove( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == PlayerUtils.Direction.Left;
        name = "Move";
    //    m_dir = dir;
    }

    public override void Process(){
        velocity.x = PlayerUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
        if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) velocity.x = 0.0f;
        if( !isMovingLeft && m_detector.isCollideWithRightWall()) velocity.x = 0.0f;

        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            velocity.y = -PlayerUtils.GravityForce * Time.deltaTime;
        }

        m_detector.Move(velocity * Time.deltaTime);

        if( m_detector.isWallClose() ){
            m_nextState = new PlayerWallHold( m_controllabledObject, 
                                              ( isMovingLeft )? PlayerUtils.Direction.Left : 
                                                                PlayerUtils.Direction.Right );
            m_isOver = true;
        }

    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new PlayerFall(m_controllabledObject, PlayerUtils.Direction.Left);
        }else if( isMovingLeft && !PlayerUtils.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerUtils.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new PlayerJump(m_controllabledObject, PlayerUtils.Direction.Left);
        }else if( PlayerUtils.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

}
