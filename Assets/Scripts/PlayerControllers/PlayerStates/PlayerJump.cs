using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;



    float timeOfIgnoringWallStick = 0;
    float timeOfJumpForceRising   = 0;
    float  MaxJUMPRISING = 0;

    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;

    public PlayerJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        JumpForce    = PlayerUtils.PlayerJumpForceMin;

        name = "Jump";
        PlayerFallOfWallHelper.ResetCounter();

        m_detector.CheatMove( new Vector2(0,40.0f));
        MaxJUMPRISING           = PlayerUtils.JumpMaxTime;
        timeOfJumpForceRising   = MaxJUMPRISING;
        timeOfIgnoringWallStick = m_controllabledObject.GetComponent<Player>().timeToJumpApex / 2.0f;
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
        timeOfIgnoringWallStick -= Time.deltaTime;

        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
        //    Debug.Log(JumpForce.ToString());
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (PlayerUtils.PlayerJumpForceMax - PlayerUtils.PlayerJumpForceMin) * Time.deltaTime //* Time.deltaTime
                            + PlayerUtils.GravityForce * Time.deltaTime,
                            PlayerUtils.PlayerJumpForceMax
                            );
        }else{
            timeOfJumpForceRising = 0.0f;
        }
        timeOfJumpForceRising   -= Time.deltaTime;
        GravityForce += -PlayerUtils.GravityForce * Time.deltaTime;
        velocity.y = JumpForce + GravityForce; 
        velocity.y = Mathf.Max( velocity.y, -500 );

        if( swipeOn ){
            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(  -PlayerUtils.MaxPlayerMoveSpeedInAir,
                                        velocity.x -PlayerUtils.PlayerMoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(  PlayerUtils.MaxPlayerMoveSpeedInAir,
                                        velocity.x + PlayerUtils.PlayerMoveSpeedInAir * Time.deltaTime);
            PlayerUtils.swipeSpeedValue = velocity.x;
            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }

        m_detector.Move(velocity * Time.deltaTime);
        checkIfShouldBeOver();
    }

    public override void HandleInput(){

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new PlayerLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_detector.isWallClose() && timeOfIgnoringWallStick < 0 ){
            if( m_swipe == GlobalUtils.Direction.Left && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new PlayerSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }else 
            if ( m_swipe == GlobalUtils.Direction.Right && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new PlayerSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }
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
