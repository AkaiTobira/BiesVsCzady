using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFall : PlayerBaseState
{
    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    private float WallSlideDelay = 0.1f;

    public CatFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        m_dir = dir;
        name = "CatFall";
        HandleSpecialBehaviour();
//        Debug.Log( CommonValues.PlayerVelocity);
        CommonValues.PlayerVelocity.y = 0;
        m_animator.SetFloat( "FallVelocity", -1000);
        //distanceToFixAnimation = new Vector3( (isLeftOriented())? -126 : 125, 0 , 0);
    }

    private void HandleSpecialBehaviour(){
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet() || 
            PlayerMoveOfWallHelper.MoveOfWallRequirementsMeet() ) {
            CommonValues.PlayerVelocity.x = CatUtils.FallOffWallFactor * 
                                            ( isLeftOriented() ? 
                                                - CatUtils.MoveSpeedInAir : 
                                                  CatUtils.MoveSpeedInAir);
        }
    }

    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left)  ? 
                        Mathf.Max(   -CatUtils.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x - CatUtils.MoveSpeedInAir * Time.deltaTime): 
                        Mathf.Min(   CatUtils.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime);
                                    
        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }

    public override void Process(){
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -63 : 63, 0 , 0);
        m_animator.SetFloat( "FallVelocity", -1000);
        WallSlideDelay -= Time.deltaTime;
        CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        ProcessSwipe();
        m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        if( m_FloorDetector.isOnGround() ) m_isOver = true;
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
        if( m_isOver ) return;
    //     if(PlayerJumpHelper.JumpRequirementsMeet( CatUtils.isJumpKeyJustPressed(), 
    //                                               m_FloorDetector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }

        if( m_ObjectInteractionDetector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_WallDetector.isWallClose() && WallSlideDelay < 0){
            m_isOver = true;
            CommonValues.PlayerVelocity.x = 0; // To check if is needed;
            m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
        }

        HandleInputSwipe();
    }
}
