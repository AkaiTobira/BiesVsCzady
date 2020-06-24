using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesJump : BaseState
{    private bool isMovingLeft = false;
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;



    float timeOfIgnoringWallStick = 0;
    float timeOfJumpForceRising   = 0;
    float  MaxJUMPRISING = 0;

    float JumpForce    = 0.0f;
    float GravityForce = 0.0f;

    float startAnimationDelay = 0.0f;

    public BiesJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        
        JumpForce    = BiesUtils.PlayerJumpForceMin;

        name = "BiesJump";
        PlayerFallOfWallHelper.ResetCounter();

        startAnimationDelay = getAnimationLenght( "BiesJumpPreparation");

        m_animator.SetTrigger("BiesJumpPressed");

        MaxJUMPRISING           = BiesUtils.JumpMaxTime;
        timeOfJumpForceRising   = MaxJUMPRISING;
        timeOfIgnoringWallStick = m_controllabledObject.GetComponent<BiesBalance>().timeToJumpApex / 2.0f;
    //    CommonValues.PlayerVelocity.x = 0;
        
        GlobalUtils.PlayerObject.GetComponent<Player>().StartCoroutine(StartJump(startAnimationDelay));
    }

    IEnumerator StartJump( float time ){
        yield return new WaitForSeconds(time);
        if( m_isOver ) yield break;
        m_detector.CheatMove( new Vector2(0,40.0f));
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        m_animator.ResetTrigger("BiesJumpPressed");
        Debug.Log("ALL IS FINE");
    }

    private float getAnimationLenght(string animationName){
        RuntimeAnimatorController ac = m_animator.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }



    private void checkIfShouldBeOver(){
        if( m_detector.isOnCelling()){
            CommonValues.PlayerVelocity.y = 0;
            CommonValues.PlayerVelocity.x = 0;
            velocity.y = 0;
            timeOfJumpForceRising = 0.0f;
            m_isOver   = true;
            m_animator.ResetTrigger("BiesJumpPressed");

        }

        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) && CommonValues.PlayerVelocity.y < 0 ){ 
            m_animator.ResetTrigger("BiesJumpPressed");
            m_isOver = true;
            timeOfJumpForceRising = 0.0f;
            m_nextState = new BiesFall( m_controllabledObject, m_detector.GetCurrentDirection());
        }
    }

    public override void OnExit(){
        GravityForce = 0;
        JumpForce    = 0;
    }

    public override void Process(){
        timeOfIgnoringWallStick -= Time.deltaTime;
        startAnimationDelay     -= Time.deltaTime;

        if( timeOfJumpForceRising > 0 && PlayerInput.isJumpKeyHold() ){
        //    Debug.Log(JumpForce.ToString());
            JumpForce  = Mathf.Min(
                            JumpForce
                            + (BiesUtils.PlayerJumpForceMax - BiesUtils.PlayerJumpForceMin) * Time.deltaTime //* Time.deltaTime
                            + BiesUtils.GravityForce * Time.deltaTime,
                            BiesUtils.PlayerJumpForceMax
                            );
        }else{
            timeOfJumpForceRising = 0.0f;
        }
        timeOfJumpForceRising   -= Time.deltaTime;
        if( startAnimationDelay < 0 ){

            GravityForce += -BiesUtils.GravityForce * Time.deltaTime;
            CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
            CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y, -500 );
            if( swipeOn ){
                 CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                                Mathf.Max(  -BiesUtils.maxMoveDistanceInAir,
                                             CommonValues.PlayerVelocity.x -BiesUtils.MoveSpeedInAir * Time.deltaTime) : 
                                Mathf.Min(  BiesUtils.maxMoveDistanceInAir,
                                             CommonValues.PlayerVelocity.x + BiesUtils.MoveSpeedInAir * Time.deltaTime);
                BiesUtils.swipeSpeedValue = velocity.x;
                // if velocity.x > 0 => m_direction = Direction.Left
                // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
            }

            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime);
            CommonValues.PlayerFaceDirection = m_detector.GetCurrentDirection();
            checkIfShouldBeOver();
        }
    }

    public override void HandleInput(){

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_animator.ResetTrigger("BiesJumpPressed");
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
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
