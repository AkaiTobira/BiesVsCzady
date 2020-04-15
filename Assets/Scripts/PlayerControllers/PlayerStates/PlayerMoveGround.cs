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
        m_controllabledObject.GetComponent<Rigidbody2D>().velocity = PlayerUtils.PlayerSpeed * (float)m_dir;
    }

    public override void HandleInput(){
        if( !Input.GetKey(KeyCode.A)       &&  isMovingLeft ) { m_isOver = true; }
        else if( !Input.GetKey(KeyCode.D)  && !isMovingLeft ) { m_isOver = true; };
    }

}
