using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesIdle : PlayerBaseState
{

    public BiesIdle( GameObject controllable ) : base( controllable ) {
        name = "BiesIdle";
        m_dir = GlobalUtils.Direction.Left;
    }

    private void HandleStopping(){
        float acceleration = (BiesUtils.PlayerSpeed / BiesUtils.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_detector.GetCurrentDirection();
    }

    private void HandleInputMoveState(GlobalUtils.Direction dir){
            m_dir = m_detector.GetCurrentDirection();
            if( m_dir !=  dir ) CommonValues.needChangeDirection = true;
            m_dir = dir;
            m_nextState = new BiesMove(m_controllabledObject, m_dir); 
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround() ) ){
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new BiesFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if( PlayerInput.isAttack3KeyPressed() ){
            m_nextState = new BiesAttack3(m_controllabledObject);
        }else if( PlayerInput.isMoveLeftKeyHold() ){
            HandleInputMoveState( GlobalUtils.Direction.Left);
        }else if( PlayerInput.isMoveRightKeyHold() ){
            HandleInputMoveState( GlobalUtils.Direction.Right);
        }else if(m_detector.isCollideWithRightWall()){
            m_nextState = new BiesWallHold( m_controllabledObject, GlobalUtils.Direction.Right );
        }else if(m_detector.isCollideWithLeftWall()){
            m_nextState = new BiesWallHold( m_controllabledObject, GlobalUtils.Direction.Left );        
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);     
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    private void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", 0);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));
    }

    public override void Process(){
        HandleStopping();
        ProcessAnimationUpdate();

        if( ! m_detector.isOnGround() ){
            CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        }else{
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }
        m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
    }
}
