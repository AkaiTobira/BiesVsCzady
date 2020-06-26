using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallHold : BaseState
{

    public CatWallHold( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        CatUtils.ResetStamina();
        m_dir = dir;
        name = "CatWallHold" + ((isLeftOriented())? "L": "R");
        CommonValues.PlayerVelocity.y =0;
    }

    public override void Process(){
        if( !m_detector.isWallClose()) m_isOver = true;

        m_animator.SetFloat( "FallVelocity", 0);
        if( !m_detector.isOnGround() ){
            CommonValues.PlayerVelocity.y = -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        }else{
            CommonValues.PlayerVelocity.y = 0.0f;
        }
    }

    public override void OnExit(){}

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            //m_isOver = true;
            m_nextState = new CatFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( isLeftOriented() && PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
            CommonValues.needChangeDirection = true;
            m_nextState = new CatMove(m_controllabledObject, GlobalUtils.Direction.Right); 
        }else if( isRightOriented() && PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
            CommonValues.needChangeDirection = true;
            m_nextState = new CatMove(m_controllabledObject, GlobalUtils.Direction.Left); 
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }else if( PlayerInput.isClimbKeyPressed() ){
            m_isOver = true;
            m_nextState = new CatWallClimb( m_controllabledObject, m_dir);
        }
    }

}
