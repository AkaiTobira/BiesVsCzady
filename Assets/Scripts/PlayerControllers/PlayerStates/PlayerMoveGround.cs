using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveGround : BaseState
{
    private bool isMovingLeft = false;
    private PlayerUtils.Direction m_dir;

    public PlayerMoveGround( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
        controllable.transform.GetComponent<Player>().changeDirection(dir);
        isMovingLeft = dir == PlayerUtils.Direction.Left;
        m_dir = dir;
    }

    public override void Process(){
        Rigidbody2D m_rb = m_controllabledObject.GetComponent<Rigidbody2D>();
        Vector2 curr_velocity = m_rb.velocity;
        curr_velocity.x = PlayerUtils.PlayerSpeed * (float)m_dir;
        m_rb.velocity = curr_velocity;
    }

    public override void HandleInput(){
        if( !Input.GetKey(KeyCode.A)       &&  isMovingLeft ) { m_isOver = true; }
        else if( !Input.GetKey(KeyCode.D)  && !isMovingLeft ) { m_isOver = true; }
        else if( Input.GetKey(KeyCode.Space) && m_controllabledObject.GetComponent<Player>().isOnGrounded()){
            m_nextState = new PlayerJump( m_controllabledObject, PlayerUtils.Direction.Left );  } ;
    }

}
