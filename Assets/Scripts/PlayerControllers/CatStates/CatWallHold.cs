using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallHold : BaseState
{
    private bool isMovingLeft = false;

    public CatWallHold( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        CatUtils.ResetStamina();
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "CatWallHold" + ((isMovingLeft)? "L": "R");

    }

    public override void Process(){
    //    velocity.x = CatUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
    //    if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) velocity.x = 0.0f;
    //    if( !isMovingLeft && m_detector.isCollideWithRightWall()) velocity.x = 0.0f;

        if( !m_detector.isWallClose()) m_isOver = true;

        velocity.y += -CatUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            velocity.y = -CatUtils.GravityForce * Time.deltaTime;
        }
        
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void OnExit(){
        if( m_dir == GlobalUtils.Direction.Left){
            velocity.x = CatUtils.PlayerSpeed * Time.deltaTime;
        }else{
            velocity.x = -CatUtils.PlayerSpeed * Time.deltaTime;
        }
        m_detector.Move(velocity * Time.deltaTime);
        velocity = new Vector2();
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new CatFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
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
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            velocity.y += -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }else if( PlayerInput.isClimbKeyPressed() ){
            m_isOver = true;
            m_nextState = new CatWallClimb( m_controllabledObject, m_dir);
        }
    }

}
