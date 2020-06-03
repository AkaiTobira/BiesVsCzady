using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesMove : BaseState
{
    private bool isMovingLeft = false;

    public BiesMove( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "BiesMove";
    //    m_dir = dir;
    }

    public override void Process(){
        velocity.x = BiesUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
        if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) velocity.x = 0.0f;
        if( !isMovingLeft && m_detector.isCollideWithRightWall()) velocity.x = 0.0f;

        velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            CatUtils.ResetStamina();
            velocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }

        m_detector.Move(velocity * Time.deltaTime);

        if( m_detector.isWallClose() ){
            m_nextState = new BiesWallHold( m_controllabledObject, 
                                              ( isMovingLeft )? GlobalUtils.Direction.Left : 
                                                                GlobalUtils.Direction.Right );
            m_isOver = true;
        }
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if( PlayerInput.isAttack3KeyPressed() ){
            m_nextState = new BiesAttack3(m_controllabledObject);
        }else if( isMovingLeft && !PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

}
