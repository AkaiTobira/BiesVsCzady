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
        velocity.x = PlayerUtils.swipeSpeedValue;
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet() ) {
            velocity.x = PlayerUtils.FallOffWallFactor * ((isMovingLeft)? -PlayerUtils.PlayerMoveSpeedInAir : PlayerUtils.PlayerMoveSpeedInAir);
        }
    }

    public override void Process(){
        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( swipeOn ){
            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(   -PlayerUtils.MaxPlayerMoveSpeedInAir,
                                        velocity.x -PlayerUtils.PlayerMoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(   PlayerUtils.MaxPlayerMoveSpeedInAir,
                                        velocity.x + PlayerUtils.PlayerMoveSpeedInAir * Time.deltaTime);

            // if velocity.x  > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(velocity * Time.deltaTime);
        if( m_detector.isOnGround() ) m_isOver = true;
    }

    public override void OnExit(){
        velocity = new Vector2();
        PlayerUtils.swipeSpeedValue = 0;
    }

    public override void HandleInput(){
    //     if(PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
    //                                               m_detector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new PlayerLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_detector.isWallClose() ){
            if( m_swipe == GlobalUtils.Direction.Left && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new PlayerSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }else 
            if ( m_swipe == GlobalUtils.Direction.Right && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new PlayerSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }
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
