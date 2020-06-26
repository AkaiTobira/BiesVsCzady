using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallJump : BaseState
{    
    private GlobalUtils.Direction m_swipe;
    private float inputLock = 0.05f;
    float timeOfJumpForceRising   = 0;
    private bool swipeOn = false;

    float JumpForce    = 0.0f;

    float GravityForce = 0.0f;

    float startAnimationDelay = 0;

    bool MoveDisabled = true;

    public CatWallJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "CatWallJump";
        m_dir = dir;
        SetUpVariables();
    }

    protected override void SetUpAnimation(){
        startAnimationDelay = getAnimationLenght( "CatJumpPreparation");
        m_animator.SetTrigger("isWallJumpPressed");

        GlobalUtils.PlayerObject.GetComponent<Player>().StartCoroutine(StartJump(startAnimationDelay));
    }


    IEnumerator StartJump( float time ){
        yield return new WaitForSeconds(time);
        if( m_isOver ) yield break;
        m_detector.CheatMove(  new Vector2( 40 * (int)m_dir, 0) );
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        m_animator.ResetTrigger("isWallJumpPressed");
        MoveDisabled = false;
    }

    private void SetUpVariables(){
        CommonValues.PlayerVelocity    = CatUtils.MinWallJumpForce;
        JumpForce                      = CommonValues.PlayerVelocity.y;
        CommonValues.PlayerVelocity.x *= (int)m_dir;
        
        PlayerFallOfWallHelper.ResetCounter();
        CatUtils.ResetStamina();

        timeOfJumpForceRising       = CatUtils.JumpMaxTime;
    }

    private bool isFalling(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && CommonValues.PlayerVelocity.y < 0 ){
            m_nextState = new CatFall( m_controllabledObject, m_detector.GetCurrentDirection());
            return true;
        }
        return false;
    }

    private bool isOnCelling(){
        if( m_detector.isOnCelling()){
            CommonValues.PlayerVelocity = new Vector2();
            timeOfJumpForceRising = 0.0f;
            return true;
        }  
        return false;
    }

    private bool isCloseToWall(){
        if( m_detector.isWallClose() ){
            m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            return true;
        }
        return false;
    }

    private void  ProcessStateEnd(){
        if( inputLock > 0.0f){ return; }
        m_isOver |= isFalling() || isOnCelling() || isCloseToWall();
        if( m_isOver){
             m_animator.ResetTrigger("isWallJumpPressed");
        }
    }

    public override void Process(){
        //        m_animator.SetBool("isWallJumpPressed", true);
        ProcessJumpAcceleration();

        if( MoveDisabled ) return;

        GravityForce += -CatUtils.GravityForce * Time.deltaTime;
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );

        ProcessSwipe();

        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        ProcessStateEnd();
    }

    private void ProcessJumpAcceleration(){
        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (CatUtils.MaxWallJumpForce.y - CatUtils.MinWallJumpForce.y) * Time.deltaTime //* Time.deltaTime
                            + CatUtils.GravityForce * Time.deltaTime,
                            CatUtils.MaxWallJumpForce.y
                            );
            timeOfJumpForceRising   -= Time.deltaTime;
        }else{
            timeOfJumpForceRising = 0.0f;
        }
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

    public override void OnExit(){}

    public override void HandleInput(){
        if( MoveDisabled ) return;
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
    }
}
