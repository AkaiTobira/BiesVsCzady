using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatIdle : PlayerIdle
{
    public CatIdle( GameObject controllable ) : base( controllable, CatUtils.infoPack ) {
        name = "CatIdle";
        m_dir = m_FloorDetector.GetCurrentDirection();
    }

    private void HandleInputMoveState(GlobalUtils.Direction dir){
            m_dir = m_FloorDetector.GetCurrentDirection();
            if( m_dir !=  dir ) CommonValues.needChangeDirection = true;
            m_dir = dir;
            m_nextState = new CatMove(m_controllabledObject, m_dir); 
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround() ) ){
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
                                                   m_FloorDetector.isOnGround() )
        ){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if(m_WallDetector.isCollideWithRightWall()){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.Direction.Right );
        }else if(m_WallDetector.isCollideWithLeftWall()){
            CommonValues.PlayerVelocity.y = 0;

            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.Direction.Left );        
        }else if( PlayerInput.isFallKeyHold() ) {
            CommonValues.PlayerVelocity.y = 0;

            m_ObjectInteractionDetector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime; 
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

    protected override void ProcessAnimationUpdate(){
        base.ProcessAnimationUpdate();

        if( m_animator.GetBool("SneakySneaky") ){
            distanceToFixAnimation = new Vector3(0, -145.51f , 0);
        }else{
            distanceToFixAnimation = new Vector3(0, -60 , 0);
        }
    }

}
