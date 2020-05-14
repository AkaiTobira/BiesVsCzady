using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : BaseState
{
    private bool isMovingLeft = false;
    private PlayerUtils.Direction m_dir;

    public PlayerMove( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == PlayerUtils.Direction.Left;
        name = "Move";
    //    m_dir = dir;
    }

    public override void Process(){
        velocity.x = PlayerUtils.PlayerSpeed * ( isMovingLeft ? -1 : 1);
        if( ! m_detector.isOnGround() ){
            velocity.y += -PlayerUtils.GravityForce * Time.deltaTime;
        }else{
            velocity.y = 0;
        }
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new PlayerFall(m_controllabledObject, PlayerUtils.Direction.Left);
        }else if( isMovingLeft && !PlayerUtils.isMoveLeftKeyHold()){
            m_isOver = true;
        }else if( !isMovingLeft && !PlayerUtils.isMoveRightKeyHold()){
            m_isOver = true;
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new PlayerJump(m_controllabledObject, PlayerUtils.Direction.Left);
        }
    }

}
