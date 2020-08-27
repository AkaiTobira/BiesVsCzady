using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesMove : PlayerMove
{
    public BiesMove( GameObject controllable, GlobalUtils.Direction dir) : 
        base( controllable, dir, BiesUtils.infoPack, "Bies" ) 
    {
        name = "BiesMove";

        m_animator.SetFloat("AnimationSpeed", 1.0f * CommonValues.PlayerVelocity.x/BiesUtils.PlayerSpeed);
    }

    protected override void ProcessStateEnd(){
        if(  m_WallDetector.isWallClose() && 
            ( m_WallDetector.isCollideWithLeftWall() || m_WallDetector.isCollideWithRightWall() ) ){
            m_nextState = new BiesWallHold( m_controllabledObject, 
                                         ( isLeftOriented() ) ? 
                                                GlobalUtils.Direction.Left : 
                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
        base.ProcessStateEnd();
    }
    public override void HandleInput(){
        base.HandleInput();

        m_animator.SetFloat("AnimationSpeed", 1.0f * CommonValues.PlayerVelocity.x/BiesUtils.PlayerSpeed);

        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if(m_lowerEdgeDetector.canClimbLedgeFromUpSite()){
            m_isOver = true;
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_lowerEdgeDetector.hasReachedPlatformEdge() && !LockAreaOverseer.autoJumpLock && m_FloorDetector.isOnGround() ){
            m_isOver = true;
            m_nextState = new BiesAutoJump(m_controllabledObject, m_dir);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if( PlayerInput.isBlockKeyJustPressed() && m_animator.GetCurrentAnimatorStateInfo(0).IsName("BiesWalk") ){
            m_nextState = new BiesBlock(m_controllabledObject);
        //}else if( PlayerInput.isAttack3KeyPressed() ){
         //   m_nextState = new BiesAttack3(m_controllabledObject);
        }else if( isLeftOriented()  && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( isRightOriented() && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_FloorDetector.isOnGround() )
        ){ 
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() && m_ObjectInteractionDetector.canFallByFloor() ) {
            m_ObjectInteractionDetector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -BiesUtils.infoPack.GravityForce * Time.deltaTime;
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    public override string GetTutorialAdvice(){
        return base.GetTutorialAdvice() + "\nC or RMB - stun/break the stalactits\nX or LMB - Smash\nR - Block";
    }
}
