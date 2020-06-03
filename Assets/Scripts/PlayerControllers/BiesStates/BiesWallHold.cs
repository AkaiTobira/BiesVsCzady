using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesWallHold : BaseState
{
    private bool isMovingLeft = false;

    public BiesWallHold( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        CatUtils.ResetStamina();
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "BiesWallHold" + ((isMovingLeft)? "L": "R");

    }

    public override void Process(){
    //    velocity.x = BiesUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
    //    if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) velocity.x = 0.0f;
    //    if( !isMovingLeft && m_detector.isCollideWithRightWall()) velocity.x = 0.0f;

        if( !m_detector.isWallClose()) m_isOver = true;

        velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            velocity.y = -BiesUtils.GravityForce * Time.deltaTime;
        }
        
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void OnExit(){
        if( m_dir == GlobalUtils.Direction.Left){
            velocity.x = BiesUtils.PlayerSpeed * Time.deltaTime;
        }else{
            velocity.x = -BiesUtils.PlayerSpeed * Time.deltaTime;
        }
        m_detector.Move(velocity * Time.deltaTime);
        velocity = new Vector2();
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if ( m_detector.IsWallPullable() && PlayerInput.isSpecialKeyHold() ){

            if( isMovingLeft ){
                if( PlayerInput.isMoveRightKeyHold()){
                    m_nextState = new BiesPullObj( m_controllabledObject, GlobalUtils.Direction.Left);
                }else if( PlayerInput.isMoveLeftKeyHold() ){
                    m_nextState = new BiesPushObj( m_controllabledObject, GlobalUtils.Direction.Left);
                }
            }else{
                if( PlayerInput.isMoveRightKeyHold()){
                    m_nextState = new BiesPushObj( m_controllabledObject, GlobalUtils.Direction.Right);
                }else if( PlayerInput.isMoveLeftKeyHold() ){
                    m_nextState = new BiesPullObj( m_controllabledObject, GlobalUtils.Direction.Right);
                }
            } 
        }else if( isMovingLeft && PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
        //    m_nextState = new PlayerMove(m_controllabledObject, GlobalUtils.Direction.Right); 
        }else if( !isMovingLeft && PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
        //    m_nextState = new PlayerMove(m_controllabledObject, GlobalUtils.Direction.Left); 
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
