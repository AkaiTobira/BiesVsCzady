using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesIdle : BaseState
{

    public BiesIdle( GameObject controllable ) : base( controllable ) {
        name = "BiesIdle";
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround() ) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if( PlayerInput.isAttack3KeyPressed() ){
            m_nextState = new BiesAttack3(m_controllabledObject);
        }else if( PlayerInput.isMoveLeftKeyHold() ){
            m_nextState = new BiesMove(m_controllabledObject, GlobalUtils.Direction.Left); 
        }else if( PlayerInput.isMoveRightKeyHold() ){
            m_nextState = new BiesMove(m_controllabledObject, GlobalUtils.Direction.Right); 
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
            velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

    public override void Process(){
        if( ! m_detector.isOnGround() ){
            velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        }else{
            CatUtils.ResetStamina();
            velocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }
        m_detector.Move( velocity * Time.deltaTime );
    }
}
