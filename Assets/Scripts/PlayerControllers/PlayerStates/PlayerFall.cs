using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : PlayerBaseState
{
    private GlobalUtils.Direction m_swipe;

    protected bool swipeOn = false;

    public PlayerFall( GameObject controllable, GlobalUtils.Direction dir,
                        ICharacterSettings settings
    ) : base( controllable ) {
        m_settings = settings;
        m_dir = dir;
        name = "PlayerFall";
        CommonValues.PlayerVelocity.y = 0;
        m_animator.SetFloat( "FallVelocity", -1000);
    }

    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left)  ? 
                        Mathf.Max(   -m_settings.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x - m_settings.MoveSpeedInAir * Time.deltaTime): 
                        Mathf.Min(   m_settings.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x + m_settings.MoveSpeedInAir * Time.deltaTime);
                                    
        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }

    public override void Process(){
        m_animator.SetFloat( "FallVelocity", -1000);
        CommonValues.PlayerVelocity.y += -m_settings.GravityForce * Time.deltaTime;
        ProcessSwipe();
        m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        if( m_FloorDetector.isOnGround() ) m_isOver = true;
    }

    public override void OnExit(){
        if( !PlayerInput.isMoveLeftKeyHold() && !PlayerInput.isMoveRightKeyHold()) CommonValues.PlayerVelocity = new Vector2();
    }

    protected void HandleInputSwipe(){
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
        HandleInputSwipe();
    }
}
