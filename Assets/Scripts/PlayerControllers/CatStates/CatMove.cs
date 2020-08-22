using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : PlayerMove
{

    public CatMove( GameObject controllable, GlobalUtils.Direction dir) : 
        base( controllable, dir, CatUtils.infoPack, "Cat" ) 
    {
        name = "CatMove";
        distanceToFixAnimation = new Vector3(0, -6 , 0);
        m_animator.SetFloat("AnimationSpeed", 2.0f * CommonValues.PlayerVelocity.x/BiesUtils.PlayerSpeed);
    }

    protected override void ProcessStateEnd(){

        if( m_animator.GetBool("SneakySneaky") ){
           distanceToFixAnimation = new Vector3(0, -14.551f , 0);
        }else{
            distanceToFixAnimation = new Vector3(0, -6 , 0);
        }

        if(  m_WallDetector.isWallClose() && 
            ( m_WallDetector.isCollideWithLeftWall() || m_WallDetector.isCollideWithRightWall() ) ){
            m_nextState = new CatWallHold( m_controllabledObject, 
                                         ( isLeftOriented() ) ? 
                                                GlobalUtils.Direction.Left : 
                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
        base.ProcessStateEnd();
    }

    public override void HandleInput(){
        base.HandleInput();

        m_animator.SetFloat("AnimationSpeed", 2.0f * CommonValues.PlayerVelocity.x/BiesUtils.PlayerSpeed);

        Debug.Log( m_lowerEdgeDetector.hasReachedPlatformEdge() + " " + !LockAreaOverseer.autoJumpLock );

        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) ){
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new CatFall(m_controllabledObject, m_FloorDetector.GetCurrentDirection());
        }else if( m_lowerEdgeDetector.hasReachedPlatformEdge() && !LockAreaOverseer.autoJumpLock && m_FloorDetector.isOnGround() ){
            
            m_isOver = true;
            m_nextState = new CatAutoJump(m_controllabledObject, m_dir);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( isLeftOriented()  && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( isRightOriented() && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_FloorDetector.isOnGround() )
        ){ 
            m_isOver    = true;
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() && m_ObjectInteractionDetector.canFallByFloor() ) {
            m_ObjectInteractionDetector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.infoPack.GravityForce * Time.deltaTime;
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    public override string GetCombatAdvice(){
        return "E - Change Form";
    }

}
