using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatJump : BaseState
{    
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    float timeOfIgnoringWallStick = 0;
    float timeOfJumpForceRising   = 0;

    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;
    float startAnimationDelay = 0.0f;

    public CatJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "CatJump";
        JumpForce    = CatUtils.PlayerJumpForceMin;
        m_dir = dir;
        SetUpCounters();
    }

    private void SetUpCounters(){
        PlayerFallOfWallHelper.ResetCounter();
        PlayerMoveOfWallHelper.ResetCounter();

        timeOfJumpForceRising   = CatUtils.JumpMaxTime;
        timeOfIgnoringWallStick = m_controllabledObject.GetComponent<CatBalance>().timeToJumpApex / 2.0f;
    }

    protected override void SetUpAnimation(){
        startAnimationDelay = getAnimationLenght( "CatJumpPreparation");
        m_animator.SetTrigger("CatJumpPressed");

        GlobalUtils.PlayerObject.GetComponent<Player>().StartCoroutine(StartJump(startAnimationDelay));
    }

    IEnumerator StartJump( float time ){
        yield return new WaitForSeconds(time);
        if( m_isOver ) yield break;
        m_detector.CheatMove( new Vector2(0,40.0f));
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        m_animator.ResetTrigger("CatJumpPressed");
    }

    private bool isOnCelling(){
        if( m_detector.isOnCelling()){
            CommonValues.PlayerVelocity =  new Vector2();
            timeOfJumpForceRising = 0.0f;
            m_animator.ResetTrigger("CatJumpPressed");
            return true;
        }
        return false;
    }

    private bool isFalling(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && CommonValues.PlayerVelocity.y < 0 ){ 
            m_animator.ResetTrigger("CatJumpPressed");
            timeOfJumpForceRising = 0.0f;
            m_nextState = new CatFall( m_controllabledObject, m_detector.GetCurrentDirection());
            return true;
        }
        return false;
    }

    private void ProcessStateEnd(){
        m_isOver |= isOnCelling() || isFalling();
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
                            + (CatUtils.PlayerJumpForceMax - CatUtils.PlayerJumpForceMin) * Time.deltaTime
                            + CatUtils.GravityForce * Time.deltaTime,
                            CatUtils.PlayerJumpForceMax
                            );
            timeOfJumpForceRising -= Time.deltaTime;
        }else{
            timeOfJumpForceRising  = 0.0f;
        }
    }

    private void ProcessMove(){
        if( startAnimationDelay > 0 ) return;
        GravityForce += -CatUtils.GravityForce * Time.deltaTime;
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce;
        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );
        ProcessSwipe();
        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        CommonValues.PlayerFaceDirection = m_detector.GetCurrentDirection();
    }

    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left)  ? 
                        Mathf.Max(  -CatUtils.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x - CatUtils.MoveSpeedInAir * Time.deltaTime): 
                        Mathf.Min(  CatUtils.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime);

        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }

    private void HandleInputSwipe(){
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
        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_animator.ResetTrigger("CatJumpPressed");
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_detector.isWallClose() && timeOfIgnoringWallStick < 0 ){
            if( isLeftOriented() && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_animator.ResetTrigger("CatJumpPressed");
                m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            }else 
            if ( isRightOriented() && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_animator.ResetTrigger("CatJumpPressed");
                m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            }
        }
        HandleInputSwipe();
    }
}
