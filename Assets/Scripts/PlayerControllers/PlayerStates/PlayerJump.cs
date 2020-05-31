using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    public PlayerJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        velocity.y = PlayerUtils.PlayerJumpForce;
        name = "Jump";
        PlayerFallOfWallHelper.ResetCounter();

        m_detector.CheatMove( new Vector2(0,40.0f));

    }


    private void checkIfShouldBeOver(){
        if( m_detector.isOnCelling()){
            velocity = new Vector2();
            m_isOver = true;
        }

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && velocity.y < 0 ){ 
            m_isOver = true;
        }

    }

    public override void Process(){
        
        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        velocity.y = Mathf.Max( velocity.y, -500 );

        if( swipeOn ){
            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            -PlayerUtils.PlayerSpeedInAir : 
                             PlayerUtils.PlayerSpeedInAir;

            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(velocity * Time.deltaTime);
        checkIfShouldBeOver();
    }

    public override void HandleInput(){

        if( m_detector.isWallClose() ){
            m_isOver = true;
            m_nextState = new PlayerSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
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
