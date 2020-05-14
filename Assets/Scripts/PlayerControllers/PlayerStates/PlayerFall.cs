using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : BaseState
{
    private bool isMovingLeft = false;
    private PlayerUtils.Direction m_dir;
    private PlayerUtils.Direction m_swipe;

    private bool swipeOn = false;

    public PlayerFall( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == PlayerUtils.Direction.Left;
        name = "Fall";
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet() ) 
            velocity.x = (isMovingLeft)? - PlayerUtils.PlayerSpeedInAir : PlayerUtils.PlayerSpeedInAir;
    }

    public override void Process(){
        if( m_detector.isOnGround() ) m_isOver = true;

        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( swipeOn ){
            velocity.x = ( m_swipe == PlayerUtils.Direction.Left ) ? 
                            -PlayerUtils.PlayerSpeedInAir : 
                             PlayerUtils.PlayerSpeedInAir;

            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){
    //     if(PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
    //                                               m_detector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, PlayerUtils.Direction.Left);
    //    }

        if( m_detector.isWallClose() && !PlayerFallOfWallHelper.FallOfWallRequirementsMeet() ){
            m_isOver = true;
            m_nextState = new PlayerSlide( m_controllabledObject, m_dir);
        }

        if( PlayerSwipeLock.SwipeUnlockRequirementsMeet() ){

            if( PlayerUtils.isMoveLeftKeyHold() ){
                swipeOn = true;
                m_swipe = PlayerUtils.Direction.Left;
            }else if( PlayerUtils.isMoveRightKeyHold() ){
                swipeOn = true;
                m_swipe = PlayerUtils.Direction.Right;
            }else{
                swipeOn = false;
            }
        }

    }

}
