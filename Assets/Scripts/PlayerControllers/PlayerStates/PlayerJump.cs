using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerBaseState
{    
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    float timeOfIgnoringWallStick = 0;
    float timeOfJumpForceRising   = 0;

    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;
    float startAnimationDelay = 0.0f;

    ICharacterSettings m_settings;

    public PlayerJump( GameObject controllable, 
                        GlobalUtils.Direction dir,
                        ICharacterSettings settings
                     )  : base( controllable ) {
        m_settings = settings;
        JumpForce    = m_settings.PlayerJumpForceMin;
        m_animator.SetFloat( "FallVelocity", JumpForce);
        m_dir = dir;
        SetUpCounters();
        m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 

        timeOfJumpForceRising   = m_settings.JumpMaxTime;
        timeOfIgnoringWallStick = 2;
    }

    private void SetUpCounters(){
        PlayerFallOfWallHelper.ResetCounter();
        PlayerMoveOfWallHelper.ResetCounter();
    }

    protected override void SetUpAnimation(){}

    protected virtual IEnumerator StartJump( float time ){ 
        yield break;
    }

    protected bool isOnCelling(){
        if( m_FloorDetector.isOnCelling()){
            CommonValues.PlayerVelocity.y = 0;
            timeOfJumpForceRising = 0.0f;
            return true;
        }
        return false;
    }



    protected virtual void ProcessStateEnd(){

    }

    public override void OnExit(){
        GravityForce = 0;
        JumpForce    = 0;
    }

    public override void Process(){
        timeOfIgnoringWallStick -= Time.deltaTime;
        startAnimationDelay     -= Time.deltaTime;
        
        ProcessJumpAcceleration();
        ProcessMove();
        ProcessStateEnd();
    }

    private void ProcessJumpAcceleration(){
        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (m_settings.PlayerJumpForceMax - m_settings.PlayerJumpForceMin) * Time.deltaTime
                            + m_settings.GravityForce * Time.deltaTime,
                            m_settings.PlayerJumpForceMax
                            );
            timeOfJumpForceRising -= Time.deltaTime;
        }else{
            timeOfJumpForceRising  = 0.0f;
        }
    }

    private void ProcessMove(){
        if( startAnimationDelay > 0 ) return;
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        GravityForce += -CatUtils.GravityForce * Time.deltaTime;
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce;
        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );
        ProcessSwipe();
        m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        CommonValues.PlayerFaceDirection = m_FloorDetector.GetCurrentDirection();
    }

    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left)  ? 
                        Mathf.Max(  -m_settings.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x - m_settings.MoveSpeedInAir * Time.deltaTime): 
                        Mathf.Min(  m_settings.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x + m_settings.MoveSpeedInAir * Time.deltaTime);

        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }

    protected void HandleInputSwipe(){
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
    public override void HandleInput(){
        if( m_ObjectInteractionDetector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_WallDetector.isWallClose() && timeOfIgnoringWallStick < 0 ){
            if( isLeftOriented() && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            }else if ( isRightOriented() && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            }else if( PlayerInput.isClimbKeyHold() ){
                m_isOver = true;
                m_nextState = new CatWallClimb( m_controllabledObject, m_dir);
            }
        }
        HandleInputSwipe();

        if( m_isOver ){
            m_animator.ResetTrigger("CatJumpPressed");
        }
    }
}
