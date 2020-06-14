using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFall : BaseState
{
    private bool isMovingLeft = false;

    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    private float WallSlideDelay = 0.1f;

    public CatFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "CatFall";
        
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet() ) {
            CommonValues.PlayerVelocity.x = CatUtils.FallOffWallFactor * ((isMovingLeft)? -CatUtils.MoveSpeedInAir : CatUtils.MoveSpeedInAir);
        }
    }

    public override void Process(){
        WallSlideDelay -= Time.deltaTime;
        CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        if( swipeOn ){
            CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(   -CatUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x - CatUtils.MoveSpeedInAir * Time.deltaTime): 
                            Mathf.Min(   CatUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x + CatUtils.MoveSpeedInAir * Time.deltaTime);

            // if velocity.x  > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        
        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        if( m_detector.isOnGround() ) m_isOver = true;
    }

    public override void OnExit(){
        CommonValues.PlayerVelocity.x = 0;
        CommonValues.PlayerVelocity.y = 0;
    }

    public override void HandleInput(){
    //     if(PlayerJumpHelper.JumpRequirementsMeet( CatUtils.isJumpKeyJustPressed(), 
    //                                               m_detector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_detector.isWallClose() && WallSlideDelay < 0){
            m_isOver = true;
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new CatWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            /*
            if( m_swipe == GlobalUtils.Direction.Left && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }else 
            if ( m_swipe == GlobalUtils.Direction.Right && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }
            */
        }

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

}
