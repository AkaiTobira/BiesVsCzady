using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private const float INPUT_LOCK = 0.05f;
    private float inputLock = 0.05f;
    float timeOfJumpForceRising   = 0;
    private bool swipeOn = false;


    float JumpForce    = 0.0f;

    float accelerationSpeed = 2;

    float GravityForce = 0.0f;

    public CatWallJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        velocity    = CatUtils.MinWallJumpForce;
        JumpForce   = velocity.y;

        Debug.Log( dir );

        m_dir = dir;


        velocity.x *= (int)dir;
        name = "CatWallJump";
        PlayerFallOfWallHelper.ResetCounter();
        CatUtils.ResetStamina();


        m_detector.CheatMove(  new Vector2( 40 * (int)dir, 0) );

        Debug.Log( velocity );

        timeOfJumpForceRising    = CatUtils.JumpMaxTime;
        CommonValues.PlayerVelocity = velocity;
    }


    private void checkIfShouldBeOver(){
        if( inputLock > 0.0f){ return; }


        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && CommonValues.PlayerVelocity.y < 0 ){ 
            m_isOver = true;
            m_nextState = new CatFall( m_controllabledObject, m_detector.GetCurrentDirection());
        }

        if( m_detector.isOnCelling()){
            CommonValues.PlayerVelocity = new Vector2();
            m_isOver   = true;
            timeOfJumpForceRising = 0.0f;
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
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );

        if( swipeOn ){
            CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(  -CatUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x - CatUtils.MoveSpeedInAir * Time.deltaTime * accelerationSpeed) : 
                            Mathf.Min(  CatUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime * accelerationSpeed);

            // if velocity.x > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        checkIfShouldBeOver();
    }

    public override void OnExit(){
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
            m_nextState = new CatWallSlide( m_controllabledObject, m_dir);//GlobalUtils.ReverseDirection(m_dir));
        }
    }
}
