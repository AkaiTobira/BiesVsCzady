using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdle : BaseState
{
    GlobalUtils.Direction lastFacingDir;

    public CatIdle( GameObject controllable ) : base( controllable ) {
        name = "CatIdle";
        lastFacingDir = m_detector.GetCurrentDirection();
    }

    private void HandleStopping(){
        float acceleration = (CatUtils.PlayerSpeed / CatUtils.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_detector.GetCurrentDirection();
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround() ) ){
            m_nextState = new CatFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( PlayerInput.isMoveLeftKeyHold() ){
            m_nextState = new CatMove(m_controllabledObject, GlobalUtils.Direction.Left); 
        }else if( PlayerInput.isMoveRightKeyHold() ){
            m_nextState = new CatMove(m_controllabledObject, GlobalUtils.Direction.Right); 
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if(m_detector.isCollideWithRightWall()){
            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.Direction.Right );
        }else if(m_detector.isCollideWithLeftWall()){
            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.Direction.Left );        
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime; 
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    public override void Process(){
        HandleStopping();
        m_animator.SetFloat( "FallVelocity", 0);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));

        if( ! m_detector.isOnGround() ){
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        }else{
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -CatUtils.GravityForce * Time.deltaTime;
        }
        m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
    }
}
