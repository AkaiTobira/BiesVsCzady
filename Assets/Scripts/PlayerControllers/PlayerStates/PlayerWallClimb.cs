using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimb : BaseState
{
    private bool isMovingLeft = false;

    public PlayerWallClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

    //    PlayerFallOfWallHelper.ResetCounter();

        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "WallClimb";
        m_dir = dir;
        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);

    }

    public override void UpdateDirection(){

            m_controllabledObject.GetComponent<Player>().animationNode.position = 
                Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                    m_controllabledObject.transform.position, ref animationVel, m_smoothTime);


    }

    public override void Process(){
    //    if( m_detector.isOnGround()   ) m_isOver = true;
    //    if( !m_detector.isWallClose() ) m_isOver = true;
    //    if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
    //        PlayerSwipeLock.ResetCounter();
    //        m_isOver = true;
    //    }

        velocity.x = velocity.y;
        velocity.x *= -( int )m_dir;

    //    velocity.x = (m_dir != GlobalUtils.Direction.Left )? -PlayerUtils.WallClimbSpeed * Time.deltaTime : 0.001f;

        velocity.y = Mathf.Max( velocity.y + PlayerUtils.WallClimbSpeed * Time.deltaTime,
                                PlayerUtils.MaxWallClimbSpeed);
        if( PlayerInput.isSpecialKeyHold() ) velocity.y = 0.0f;
        PlayerUtils.stamina -= velocity.y * Time.deltaTime;
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){

        if( PlayerInput.isJumpKeyJustPressed() ){
            m_isOver = true;
            m_nextState = new PlayerWallJump(m_controllabledObject, m_dir);
        }

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new PlayerLedgeClimb( m_controllabledObject, m_dir);
        }else if( PlayerInput.isSpecialKeyHold() ){

        }else if( !PlayerInput.isClimbKeyHold() || PlayerUtils.stamina < 0){
            m_isOver = true;
            m_nextState = new PlayerWallSlide(m_controllabledObject, m_dir);
        }
    }
}
