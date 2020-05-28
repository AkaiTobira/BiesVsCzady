using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallHold : BaseState
{
    private bool isMovingLeft = false;

    public PlayerWallHold( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == PlayerUtils.Direction.Left;
        name = "WallHold" + ((isMovingLeft)? "L": "R");

    }

    public override void Process(){
    //    velocity.x = PlayerUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
    //    if( isMovingLeft  && m_detector.isCollideWithLeftWall() ) velocity.x = 0.0f;
    //    if( !isMovingLeft && m_detector.isCollideWithRightWall()) velocity.x = 0.0f;

        if( !m_detector.isWallClose()) m_isOver = true;

        velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        if( m_detector.isOnGround() ){
            velocity.y = -PlayerUtils.GravityForce * Time.deltaTime;
        }
        
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void OnExit(){
        if( m_dir == PlayerUtils.Direction.Left){
            velocity.x = PlayerUtils.PlayerSpeed * Time.deltaTime;
        }else{
            velocity.x = -PlayerUtils.PlayerSpeed * Time.deltaTime;
        }
        m_detector.Move(velocity * Time.deltaTime);
        velocity = new Vector2();
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new PlayerFall(m_controllabledObject, PlayerUtils.Direction.Left);
        }else if ( m_detector.IsWallPullable() && PlayerUtils.isSpecialKeyHold() ){

            if( isMovingLeft ){
                if( PlayerUtils.isMoveRightKeyHold()){
                    m_nextState = new PlayerPullObj( m_controllabledObject, PlayerUtils.Direction.Left);
                }else if( PlayerUtils.isMoveLeftKeyHold() ){
                    m_nextState = new PlayerPushObj( m_controllabledObject, PlayerUtils.Direction.Left);
                }
            }else{
                if( PlayerUtils.isMoveRightKeyHold()){
                    m_nextState = new PlayerPushObj( m_controllabledObject, PlayerUtils.Direction.Right);
                }else if( PlayerUtils.isMoveLeftKeyHold() ){
                    m_nextState = new PlayerPullObj( m_controllabledObject, PlayerUtils.Direction.Right);
                }
            } 
            // m_nextState = new PlayerPullObj
        }else if( isMovingLeft && PlayerUtils.isMoveRightKeyHold()){
            m_isOver = true;
        //    m_nextState = new PlayerMove(m_controllabledObject, PlayerUtils.Direction.Right); 
        }else if( !isMovingLeft && PlayerUtils.isMoveLeftKeyHold()){
            m_isOver = true;
        //    m_nextState = new PlayerMove(m_controllabledObject, PlayerUtils.Direction.Left); 
    //    }else if( 
    //        PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
    //                                               m_detector.isOnGround() )
    //    ){ 
    //        m_nextState = new PlayerJump(m_controllabledObject, PlayerUtils.Direction.Left);
        }else if( PlayerUtils.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

}
