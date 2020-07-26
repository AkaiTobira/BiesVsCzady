using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : PlayerBaseState{    
    protected float timeToEnd;
    protected AnimationTransition m_transition;
    protected float velocitXFriction = 0.0f;

    protected float knocBackDirection;

    protected bool isFaceLocked = false;

    protected GlobalUtils.Direction savedDir;

    private GlobalUtils.Direction m_swipe;
    private bool swipeOn = false;

    private bool disableSwipe = false;

    private float swipeLock = 0.2f;

    public PlayerHurt(  GameObject controllable, 
                        GlobalUtils.AttackInfo infoPack,
                        ICharacterSettings settings
                        ) : base( controllable ){

        PlayerFallHelper.FallRequirementsMeet( true );
        m_settings = settings;

        fillKnockbackInfo( infoPack );
        m_FloorDetector.CheatMove( new Vector2(0,40.0f));

    }

    protected override void UpdateDirection(){
        if( !isFaceLocked ) base.UpdateDirection();
    }


    protected override void SetUpAnimation(){
        m_transition = m_controllabledObject.
               GetComponent<Player>().animationNode.
               GetComponent<AnimationTransition>();
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        isFaceLocked = infoPack.lockFaceDirectionDuringKnockback;
        if( isFaceLocked ){
            knocBackDirection             = (int)infoPack.fromCameAttack;
            CommonValues.PlayerVelocity   = infoPack.knockBackValue;
            CommonValues.PlayerVelocity.x *= (int)infoPack.fromCameAttack;
            disableSwipe                  = true;
            savedDir                      = GlobalUtils.ReverseDirection(infoPack.fromCameAttack);
        }else{
            savedDir                      = m_FloorDetector.GetCurrentDirection();
            knocBackDirection             = Mathf.Sign(CommonValues.PlayerVelocity.x);
            CommonValues.PlayerVelocity   = infoPack.knockBackValue;
        }
        
        if( velocitXFriction > 0){
            m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        }
    }

    protected virtual void ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        swipeLock -= Time.deltaTime;
        if( timeToEnd < 0 ){
            m_isOver = true;
//            Debug.Log( savedDir.ToString() + m_FloorDetector.GetCurrentDirection().ToString() );
            if( isFaceLocked ) CommonValues.PlayerVelocity.x = 0;
            //if( isFaceLocked )m_FloorDetector.Move( new Vector2( 0.001f, 0) * (float)savedDir);
        }
    }

    private void ProcessMove(){
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        m_animator.SetBool(  "isGrounded", m_FloorDetector.isOnGround());
        PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround() );

        if( knocBackDirection == -1 ) {
            CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x - velocitXFriction * Time.deltaTime;
        }else{
            CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x + velocitXFriction * Time.deltaTime;
        }

        m_FloorDetector.Move(CommonValues.PlayerVelocity*Time.deltaTime);
    }

    private void ProcessSwipe(){
        if( (!swipeOn || !disableSwipe) && swipeLock > 0 ) return;

        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(   -m_settings.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x - m_settings.MoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(    m_settings.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x + m_settings.MoveSpeedInAir * Time.deltaTime);
                                    
    
    //    m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }

    private void HandleInputSwipe(){
        if( PlayerSwipeLock.SwipeUnlockRequirementsMeet() ){
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

    private void ProcessMove2(){
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        m_animator.SetBool(  "isGrounded", m_FloorDetector.isOnGround());
        PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround() );

     //   if( knocBackDirection == -1 ) {
    //        CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x - velocitXFriction * Time.deltaTime;
    //    }else{
    //        CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x + velocitXFriction * Time.deltaTime;
    //    }

        m_FloorDetector.Move(CommonValues.PlayerVelocity*Time.deltaTime, false);
    }

    public override void Process(){
        ProcessStateEnd();

        if( !isFaceLocked ) {
            ProcessMove();
            ProcessSwipe();
        }
        else{
            ProcessMove2();
        }
        m_dir = savedDir;
    }
    public override void HandleInput(){
        HandleInputSwipe();
    }

    public override string GetTutorialAdvice(){
        return "";
    }


}
