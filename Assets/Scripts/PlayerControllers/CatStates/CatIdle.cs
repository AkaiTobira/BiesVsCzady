using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdle : BaseState
{

    public CatIdle( GameObject controllable ) : base( controllable ) {
        name = "CatIdle";
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround() ) ){
            m_nextState = new CatFall(m_controllabledObject, GlobalUtils.Direction.Left);
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
            velocity.y += -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

    public override void Process(){
        if( ! m_detector.isOnGround() ){
            velocity.y += -CatUtils.GravityForce * Time.deltaTime;
        }else{
            CatUtils.ResetStamina();
            velocity.y = -CatUtils.GravityForce * Time.deltaTime;
        }
        m_detector.Move( velocity * Time.deltaTime );
    }
}
