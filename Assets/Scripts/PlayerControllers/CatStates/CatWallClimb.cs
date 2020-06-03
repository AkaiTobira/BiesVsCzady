using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallClimb : BaseState
{
    private bool isMovingLeft = false;

    public CatWallClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

    //    PlayerFallOfWallHelper.ResetCounter();

        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "CatWallClimb";
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

    //    velocity.x = (m_dir != GlobalUtils.Direction.Left )? -CatUtils.WallClimbSpeed * Time.deltaTime : 0.001f;

        velocity.y = Mathf.Max( velocity.y + CatUtils.WallClimbSpeed * Time.deltaTime,
                                CatUtils.MaxWallClimbSpeed);
        if( PlayerInput.isSpecialKeyHold() ) velocity.y = 0.0f;
        CatUtils.stamina -= velocity.y * Time.deltaTime;
        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){

        if( PlayerInput.isJumpKeyJustPressed() ){
            m_isOver = true;
            m_nextState = new CatWallJump(m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }

        if( m_detector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( PlayerInput.isSpecialKeyHold() ){

        }else if( !PlayerInput.isClimbKeyHold() || CatUtils.stamina < 0){
            m_isOver = true;
            m_nextState = new CatWallSlide(m_controllabledObject, m_dir);
        }
    }
}
