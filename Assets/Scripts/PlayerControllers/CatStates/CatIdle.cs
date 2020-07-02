using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdle : PlayerBaseState
{
    public CatIdle( GameObject controllable ) : base( controllable ) {
        name = "CatIdle";
        m_dir = GlobalUtils.Direction.Left;
    }

    private void HandleStopping(){
        float acceleration = (CatUtils.PlayerSpeed / CatUtils.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_detector.GetCurrentDirection();
    }

    private void HandleInputMoveState(GlobalUtils.Direction dir){
            m_dir = m_detector.GetCurrentDirection();
            if( m_dir !=  dir ) CommonValues.needChangeDirection = true;
            m_dir = dir;
            m_nextState = new CatMove(m_controllabledObject, m_dir); 
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround() ) ){
            CommonValues.PlayerVelocity.y = 0;
            m_nextState = new CatFall(m_controllabledObject, m_dir);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( PlayerInput.isMoveLeftKeyHold() ){
            CommonValues.PlayerVelocity.y = 0;

            HandleInputMoveState( GlobalUtils.Direction.Left);
        }else if( PlayerInput.isMoveRightKeyHold() ){
            CommonValues.PlayerVelocity.y = 0;

            HandleInputMoveState( GlobalUtils.Direction.Right);
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if(m_detector.isCollideWithRightWall()){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.Direction.Right );
        }else if(m_detector.isCollideWithLeftWall()){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.Direction.Left );        
        }else if( PlayerInput.isFallKeyHold() ) {
            CommonValues.PlayerVelocity.y = 0;

            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime; 
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    private void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", -2);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));
    }

    public override void Process(){
        HandleStopping();
        ProcessAnimationUpdate();

        CommonValues.PlayerVelocity.y = 0;

        if( ! m_detector.isOnGround() ){
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        }else{
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = 0;
        }

        m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
    }
}
