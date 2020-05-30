using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : BaseState
{    private bool isMovingLeft = false;
    private PlayerUtils.Direction m_swipe;

    private bool swipeOn = false;

    private  int timeToCheckJump = 180;

    public PlayerJump( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == PlayerUtils.Direction.Left;
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

      //  timeToCheckJump --;
      //  if( timeToCheckJump > 0 ) return;

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && velocity.y < 0 ){ 
            m_isOver = true;
        }

    }

    public override void Process(){
        
        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        velocity.y = Mathf.Max( velocity.y, -500 );

        if( swipeOn ){
            velocity.x = ( m_swipe == PlayerUtils.Direction.Left ) ? 
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
            m_nextState = new PlayerSlide( m_controllabledObject, PlayerUtils.ReverseDirection(m_dir));
        }

        if( PlayerInput.isMoveLeftKeyHold() ){
            swipeOn = true;
            m_swipe = PlayerUtils.Direction.Left;
        }else if( PlayerInput.isMoveRightKeyHold() ){
            swipeOn = true;
            m_swipe = PlayerUtils.Direction.Right;
        }else{
            swipeOn = false;
        }


    }
}
