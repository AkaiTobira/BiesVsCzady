﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private const float INPUT_LOCK = 0.15f;
    private float inputLock = 0.15f;

    private bool swipeOn = false;

    public PlayerWallJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        velocity    = PlayerUtils.PlayerWallJumpForce;
        velocity.x *= (dir == GlobalUtils.Direction.Left)? -1 : 1;
        name = "WallJump";
        PlayerFallOfWallHelper.ResetCounter();
        PlayerUtils.ResetStamina();

        velocity.x = Mathf.Max(PlayerUtils.PlayerWallJumpForce.x * inputLock/INPUT_LOCK, 
                               PlayerUtils.maxMoveDistanceInAir) *
                               velocity.y/PlayerUtils.PlayerWallJumpForce.y * 
                               ((dir == GlobalUtils.Direction.Left)? -1 : 1);

    }


    private void checkIfShouldBeOver(){
        if( inputLock > 0.0f){ return; }
        if( m_detector.isOnCelling()){
            velocity.y = 0;
            m_isOver   = true;
        }

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && velocity.y < 0 ){ 
            m_isOver = true;
        }

    }

    public override void Process(){
        checkIfShouldBeOver();
        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( swipeOn ){
            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max( -PlayerUtils.maxMoveDistanceInAir,
                                        velocity.x - PlayerUtils.MoveSpeedInAir * Time.deltaTime): 
                            Mathf.Min(  PlayerUtils.maxMoveDistanceInAir,
                                        velocity.x + PlayerUtils.MoveSpeedInAir * Time.deltaTime);

            PlayerUtils.swipeSpeedValue = velocity.x;

            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){
        inputLock -= Time.deltaTime;
        if( inputLock > 0.0f){ return; }

        if( m_detector.isWallClose() ){
            m_isOver = true;
            m_nextState = new PlayerWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }

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
