using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesFall : PlayerBaseState
{
    private GlobalUtils.Direction m_swipe;
    private bool swipeOn = false;
    public BiesFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "BiesFall";
    }

    public override void Process(){
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        ProcessSwipe();
        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        if( m_detector.isOnGround() ) m_isOver = true;
    }

    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(   -BiesUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x -BiesUtils.MoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(    BiesUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x + BiesUtils.MoveSpeedInAir * Time.deltaTime);
                                    
        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }

    public override void OnExit(){
        if( !PlayerInput.isMoveLeftKeyHold() && !PlayerInput.isMoveRightKeyHold()) CommonValues.PlayerVelocity = new Vector2();
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

    public override void HandleInput(){
    //     if(PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
    //                                               m_detector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
        }

        HandleInputSwipe();
    }
}
