using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : BaseState
{
    private bool isMovingLeft = false;
    private PlayerUtils.Direction m_dir;

    public PlayerSlide( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

        isMovingLeft = dir == PlayerUtils.Direction.Left;
        name = "WallSlide";
    }

    private float timeToFallOffWall = 0.0f;

    public override void Process(){
        if( m_detector.isOnGround()   ) m_isOver = true;
        if( !m_detector.isWallClose() ) m_isOver = true;
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
            PlayerSwipeLock.ResetCounter();
            m_isOver = true;
        }

        velocity.y =  Mathf.Max( velocity.y -PlayerUtils.GravityForce * Time.deltaTime,
                                 -PlayerUtils.MaxWallSlideSpeed);
        if( PlayerUtils.isSpecialKeyHold() ) velocity.y = 0.0f;


        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){
        if( !PlayerUtils.isSpecialKeyHold() ) {
            if( isMovingLeft && PlayerUtils.isMoveRightKeyHold() ){
                m_isOver = true;
                m_nextState = new PlayerFall( m_controllabledObject, PlayerUtils.Direction.Right  );
            }else if(!isMovingLeft && PlayerUtils.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new PlayerFall( m_controllabledObject, PlayerUtils.Direction.Left  );
            }
        }else{

        }
    }
}
