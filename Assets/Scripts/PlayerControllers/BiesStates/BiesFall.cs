using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesFall : BaseState
{
    private bool isMovingLeft = false;

    private GlobalUtils.Direction m_swipe;

    private bool swipeOn = false;

    public BiesFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "BiesFall";

        

    }

    public override void Process(){
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        if( swipeOn ){
            CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left ) ? 
                            Mathf.Max(   -BiesUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x -BiesUtils.MoveSpeedInAir * Time.deltaTime) : 
                            Mathf.Min(    BiesUtils.maxMoveDistanceInAir,
                                        CommonValues.PlayerVelocity.x + BiesUtils.MoveSpeedInAir * Time.deltaTime);

            // if velocity.x  > 0 => m_direction = Direction.Left
            // else velocity.x < 0 => m_direction = Direction.Right czy jakoś tak.
        }
        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        CommonValues.PlayerFaceDirection = m_detector.GetCurrentDirection();
        if( m_detector.isOnGround() ) m_isOver = true;
    }

    public override void OnExit(){
        CommonValues.PlayerVelocity = new Vector2();
        BiesUtils.swipeSpeedValue = 0;
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
        /*
        else if( m_detector.isWallClose() ){
            if( m_swipe == GlobalUtils.Direction.Left && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new PlayerWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }else 
            if ( m_swipe == GlobalUtils.Direction.Right && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new PlayerWallSlide( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }
        }
        */
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
