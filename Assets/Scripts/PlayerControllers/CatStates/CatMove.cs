using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : BaseState
{
    private bool isMovingLeft = false;

    public CatMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "CatMove";
        CommonValues.PlayerVelocity.x = 0;
    //    m_dir = dir;
    }

    public override void Process(){
        CommonValues.PlayerVelocity.x = CatUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
        if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) CommonValues.PlayerVelocity.x = 0.0f;
        if( !isMovingLeft && m_detector.isCollideWithRightWall()) CommonValues.PlayerVelocity.x = 0.0f;

        CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -CatUtils.GravityForce * Time.deltaTime;
        }

        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        if( m_detector.isWallClose() && 
           (m_detector.isCollideWithLeftWall() || m_detector.isCollideWithRightWall() ) ){
            m_nextState = new CatWallHold( m_controllabledObject, 
                                              ( isMovingLeft )? GlobalUtils.Direction.Left : 
                                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
    }

    public override void OnExit(){
        CommonValues.PlayerVelocity.x = 0;
    }


    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            CommonValues.PlayerVelocity.x = 0;
            m_nextState = new CatFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( isMovingLeft && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_isOver    = true;
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() && m_detector.canFallByFloor() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }
    }

}
