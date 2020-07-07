using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesIdle : PlayerBaseState
{

    public BiesIdle( GameObject controllable ) : base( controllable ) {
        name = "BiesIdle";
        m_dir = m_FloorDetector.GetCurrentDirection();
    //    m_dir = GlobalUtils.Direction.Left;
    }

    private void HandleStopping(){
        float acceleration = (BiesUtils.PlayerSpeed / BiesUtils.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

    private void HandleInputMoveState(GlobalUtils.Direction dir){
            m_dir = m_FloorDetector.GetCurrentDirection();
            if( m_dir !=  dir ) CommonValues.needChangeDirection = true;
            m_dir = dir;
            m_nextState = new BiesMove(m_controllabledObject, m_dir); 
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround() ) ){
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new BiesFall(m_controllabledObject, m_FloorDetector.GetCurrentDirection());
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
        }else if(m_WallDetector.isCollideWithRightWall()){
            m_nextState = new BiesWallHold( m_controllabledObject, GlobalUtils.Direction.Right );
        }else if(m_WallDetector.isCollideWithLeftWall()){
            m_nextState = new BiesWallHold( m_controllabledObject, GlobalUtils.Direction.Left );        
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_FloorDetector.isOnGround() )
        ){
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);     
        }else if( PlayerInput.isFallKeyHold() ) {
            m_ObjectInteractionDetector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    private void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", 0);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));


        m_animator.SetBool("Attack1", false);
        m_animator.SetBool("Attack2", false);
        m_animator.SetBool("Attack3", false);
        m_animator.SetBool("Attack4", false);
        m_animator.SetBool("Attack5", false);
    }

    public override void Process(){
        HandleStopping();
        ProcessAnimationUpdate();

        if( ! m_FloorDetector.isOnGround() ){
            CommonValues.PlayerVelocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        }else{
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }
        m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
    }
}
