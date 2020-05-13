using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : BaseState
{

    public PlayerIdle( GameObject controllable ) : base( controllable ) {
        name = "Idle";
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround() ) ){
            m_nextState = new PlayerFall(m_controllabledObject, PlayerUtils.Direction.Left);
        }else if( PlayerUtils.isMoveLeftKeyHold() ){
            m_nextState = new PlayerMove(m_controllabledObject, PlayerUtils.Direction.Left); 
        }else if( PlayerUtils.isMoveRightKeyHold() ){
            m_nextState = new PlayerMove(m_controllabledObject, PlayerUtils.Direction.Right); 
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){
            m_nextState = new PlayerJump(m_controllabledObject, PlayerUtils.Direction.Left);
        }
    }

    public override void Process(){

        if( ! m_detector.isOnGround() ){
            velocity += new Vector2( 0, -PlayerUtils.GravityForce * Time.deltaTime);
            m_detector.Move( velocity * Time.deltaTime );
        }

    }
}
