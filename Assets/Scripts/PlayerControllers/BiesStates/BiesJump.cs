using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesJump : PlayerBaseState
{    
    private GlobalUtils.Direction m_swipe;
    private bool swipeOn = false;

    float timeOfIgnoringWallStick = 0;
    float timeOfJumpForceRising   = 0;

    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;

    float startAnimationDelay = 0.0f;

    public BiesJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "BiesJump";
        JumpForce = BiesUtils.PlayerJumpForceMin;
        m_dir = dir;
        SetUpCounters();
    }

    protected override void SetUpAnimation(){
        startAnimationDelay = getAnimationLenght( "BiesJumpPreparation");
        m_animator.SetTrigger("BiesJumpPressed");

        GlobalUtils.PlayerObject.GetComponent<Player>().StartCoroutine(StartJump(startAnimationDelay));
    }


    private void SetUpCounters(){
        PlayerFallOfWallHelper.ResetCounter();
        PlayerMoveOfWallHelper.ResetCounter();

        timeOfJumpForceRising   = BiesUtils.JumpMaxTime;
        timeOfIgnoringWallStick = m_controllabledObject.GetComponent<BiesBalance>().timeToJumpApex / 2.0f;
    }

    IEnumerator StartJump( float time ){
        yield return new WaitForSeconds(time);
        if( m_isOver ) yield break;
        m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        m_animator.ResetTrigger("BiesJumpPressed");
    }


    private bool isOnCelling(){
        if( m_FloorDetector.isOnCelling()){
            timeOfJumpForceRising = 0.0f;
            m_animator.ResetTrigger("BiesJumpPressed");
            return true;
        }
        return false;
    }

    private bool isFalling(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) && CommonValues.PlayerVelocity.y <= 0 ){ 
            timeOfJumpForceRising = 0.0f;
            m_nextState = new BiesFall( m_controllabledObject, m_FloorDetector.GetCurrentDirection());
            return true;
        }
        return false;
    }

    private void ProcessStateEnd(){
        m_isOver |= isOnCelling() || isFalling();
        if( m_isOver ){
            m_animator.ResetTrigger("BiesJumpPressed");
        }
    }

    public override void OnExit(){
        GravityForce = 0;
        JumpForce    = 0;
    }

    private void ProcessJumpAcceleration(){
        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (BiesUtils.PlayerJumpForceMax - BiesUtils.PlayerJumpForceMin) * Time.deltaTime //* Time.deltaTime
                            + BiesUtils.GravityForce * Time.deltaTime,
                            BiesUtils.PlayerJumpForceMax
                            );
            timeOfJumpForceRising -= Time.deltaTime;
        }else{
            timeOfJumpForceRising  = 0.0f;
        }
    }

    private void ProcessMove(){
        if( startAnimationDelay > 0 ) return;
            GravityForce += -BiesUtils.GravityForce * Time.deltaTime;
            CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
            CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );
            ProcessSwipe();
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime);
            CommonValues.PlayerFaceDirection = m_FloorDetector.GetCurrentDirection();
    }

    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                        Mathf.Max(  -BiesUtils.maxMoveDistanceInAir,
                                     CommonValues.PlayerVelocity.x -BiesUtils.MoveSpeedInAir * Time.deltaTime) : 
                        Mathf.Min(  BiesUtils.maxMoveDistanceInAir,
                                     CommonValues.PlayerVelocity.x + BiesUtils.MoveSpeedInAir * Time.deltaTime);

        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }


    public override void Process(){
        timeOfIgnoringWallStick -= Time.deltaTime;
        startAnimationDelay     -= Time.deltaTime;

        ProcessJumpAcceleration();
        ProcessMove();
        ProcessStateEnd();
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
        if( m_ObjectInteractionDetector.canClimbLedge() ){
            m_isOver = true;
            m_animator.ResetTrigger("BiesJumpPressed");
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
        }
        HandleInputSwipe();
    }
}
