using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : BaseState
{
    private bool isMovingLeft = false;

    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    public PlayerFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "Fall";
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet() ) 
            velocity.x = (isMovingLeft)? - PlayerUtils.PlayerSpeedInAir : PlayerUtils.PlayerSpeedInAir;
    }

    public override void Process(){
        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( swipeOn ){
            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            -PlayerUtils.PlayerSpeedInAir : 
                             PlayerUtils.PlayerSpeedInAir;

            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(velocity * Time.deltaTime);
        if( m_detector.isOnGround() ) m_isOver = true;
    }

    public override void HandleInput(){
    //     if(PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
    //                                               m_detector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }

        if( m_detector.isWallClose() && !PlayerFallOfWallHelper.FallOfWallRequirementsMeet() ){
            m_isOver = true;
            m_nextState = new PlayerSlide( m_controllabledObject,  GlobalUtils.ReverseDirection(m_dir));
        }

        if( PlayerSwipeLock.SwipeUnlockRequirementsMeet() ){

            if( PlayerInput.isMoveLeftKeyHold() ){
                swipeOn = true;
                m_swipe = GlobalUtils.Direction.Left;
            }else if( PlayerInput.isMoveRightKeyHold() ){
                swipeOn = true;
                m_swipe = GlobalUtils.Direction.Right;
            }else{
                swipeOn = false;
            }
        }

    }

}
