using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;



    float timeOfIgnoringWallStick = 0;
    float timeOfJumpForceRising   = 0;

    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;

    public CatJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        JumpForce    = CatUtils.PlayerJumpForceMin;

        name = "CatJump";
        PlayerFallOfWallHelper.ResetCounter();
        m_detector.CheatMove( new Vector2(0,40.0f));
        timeOfJumpForceRising   = CatUtils.JumpMaxTime;
        timeOfIgnoringWallStick = m_controllabledObject.GetComponent<CatBalance>().timeToJumpApex / 2.0f;
        CommonValues.PlayerVelocity.x = 0;
    }


    private void checkIfShouldBeOver(){
        if( m_detector.isOnCelling()){
            CommonValues.PlayerVelocity.y = 0;
            CommonValues.PlayerVelocity.x = 0;

            velocity.y = 0;
            timeOfJumpForceRising = 0.0f;
            m_isOver   = true;
        }

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && CommonValues.PlayerVelocity.y < 0 ){ 
            m_isOver = true;
            timeOfJumpForceRising = 0.0f;
            m_nextState = new CatFall( m_controllabledObject, m_detector.GetCurrentDirection());
        }
    }

    public override void OnExit(){
        GravityForce = 0;
        JumpForce    = 0;
    }

    public override void Process(){
        timeOfIgnoringWallStick -= Time.deltaTime;

        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
        //    Debug.Log(JumpForce.ToString());
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (CatUtils.PlayerJumpForceMax - CatUtils.PlayerJumpForceMin) * Time.deltaTime //* Time.deltaTime
                            + CatUtils.GravityForce * Time.deltaTime,
                            CatUtils.PlayerJumpForceMax
                            );
        }else{
            timeOfJumpForceRising = 0.0f;
        }
        timeOfJumpForceRising   -= Time.deltaTime;
        GravityForce += -CatUtils.GravityForce * Time.deltaTime;
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce;
        CommonValues.PlayerVelocity.y = velocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );

        velocity.y = JumpForce + GravityForce; 
        velocity.y = Mathf.Max( velocity.y, -500 );

        if( swipeOn ){

            CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(  -CatUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x -CatUtils.MoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(  CatUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime);

            velocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(  -CatUtils.maxMoveDistanceInAir,
                                        velocity.x -CatUtils.MoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(  CatUtils.maxMoveDistanceInAir,
                                        velocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime);
        }

        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        checkIfShouldBeOver();
    }

    public override void HandleInput(){

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_detector.isWallClose() && timeOfIgnoringWallStick < 0 ){
            if( m_swipe == GlobalUtils.Direction.Left && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }else 
            if ( m_swipe == GlobalUtils.Direction.Right && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
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
