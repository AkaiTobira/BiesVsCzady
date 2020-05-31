using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : BaseState
{
    private bool isMovingLeft = false;

    public PlayerSlide( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

        PlayerFallOfWallHelper.ResetCounter();

        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "WallSlide";
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
        if( m_detector.isOnGround()   ) m_isOver = true;
        if( !m_detector.isWallClose() ) m_isOver = true;
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
            PlayerSwipeLock.ResetCounter();
            m_isOver = true;
        }

        velocity.x = (m_dir != GlobalUtils.Direction.Left )? -0.001f : 0.001f;
        velocity.y = Mathf.Max( velocity.y -PlayerUtils.GravityForce * Time.deltaTime,
                                 -PlayerUtils.MaxWallSlideSpeed);
        if( PlayerInput.isSpecialKeyHold() ) velocity.y = 0.0f;


        m_detector.Move(velocity * Time.deltaTime);
    }

    public override void HandleInput(){
        if( !PlayerInput.isSpecialKeyHold() ) {

            if( PlayerInput.isJumpKeyJustPressed() ){
                m_isOver = true;
                m_nextState = new PlayerJumpWall(m_controllabledObject, m_dir);
            }
        //    }else if( isMovingLeft && PlayerUtils.isMoveRightKeyHold() ){
        //        m_isOver = true;
        //        m_nextState = new PlayerFall( m_controllabledObject, GlobalUtils.Direction.Right  );
        //    }else if(!isMovingLeft && PlayerUtils.isMoveLeftKeyHold() ){
        //        m_isOver = true;
        //        m_nextState = new PlayerFall( m_controllabledObject, GlobalUtils.Direction.Left  );
        //    }
        }else{

        }
    }
}
