using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private const float INPUT_LOCK = 0.1f;
    private float inputLock = 0.1f;


    float timeOfJumpForceRising   = 0;

    private bool swipeOn = false;


    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;

    public CatWallJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        velocity    = CatUtils.MinWallJumpForce;
        JumpForce   = velocity.y;
        velocity.x *= (dir == GlobalUtils.Direction.Left)? -1 : 1;
        name = "CatWallJump";
        PlayerFallOfWallHelper.ResetCounter();
        CatUtils.ResetStamina();

    //    velocity.x = Mathf.Max(CatUtils.PlayerWallJumpForce.x * inputLock/INPUT_LOCK, 
    //                           CatUtils.maxMoveDistanceInAir) *
     //                          velocity.y/CatUtils.PlayerWallJumpForce.y * 
     //                           ((dir == GlobalUtils.Direction.Left)? -1 : 1);
        CatUtils.swipeSpeedValue = velocity.x;
        timeOfJumpForceRising    = CatUtils.JumpMaxTime;
    }


    private void checkIfShouldBeOver(){
        if( inputLock > 0.0f){ return; }


        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && velocity.y < 0 ){ 
            m_isOver = true;
            CatUtils.swipeSpeedValue = velocity.x;
            m_nextState = new CatFall( m_controllabledObject, m_detector.GetCurrentDirection());
        }

        if( m_detector.isOnCelling()){
            velocity.y = 0;
            m_isOver   = true;
        }        
    }

    public override void Process(){
        
        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
        //    Debug.Log(JumpForce.ToString());
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (CatUtils.MaxWallJumpForce.y - CatUtils.MinWallJumpForce.y) * Time.deltaTime //* Time.deltaTime
                            + CatUtils.GravityForce * Time.deltaTime,
                            CatUtils.MaxWallJumpForce.y
                            );
        }else{
            timeOfJumpForceRising = 0.0f;
        }
        timeOfJumpForceRising   -= Time.deltaTime;
        GravityForce += -CatUtils.GravityForce * Time.deltaTime;
        velocity.y = JumpForce + GravityForce; 
        velocity.y = Mathf.Max( velocity.y, -500 );

        if( swipeOn ){
            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(  -CatUtils.maxMoveDistanceInAir,
                                        velocity.x -CatUtils.MoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(  CatUtils.maxMoveDistanceInAir,
                                        velocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime);

            

            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(velocity * Time.deltaTime);
        checkIfShouldBeOver();
    }

    public override void OnExit(){
        CatUtils.swipeSpeedValue = velocity.x;
    }

    public override void HandleInput(){
        inputLock -= Time.deltaTime;

        if( PlayerInput.isMoveLeftKeyHold() ){
            swipeOn = true;
            m_swipe = GlobalUtils.Direction.Left;
        }else if( PlayerInput.isMoveRightKeyHold() ){
            swipeOn = true;
            m_swipe = GlobalUtils.Direction.Right;
        }else{
            swipeOn = false;
        }

        if( inputLock > 0.0f){ return; }

        if( m_detector.isWallClose() ){
            m_isOver = true;
            m_nextState = new CatWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }
    }
}
