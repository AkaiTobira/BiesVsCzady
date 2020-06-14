using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallSlide : BaseState
{
    private bool isMovingLeft = false;

    public CatWallSlide( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

        PlayerFallOfWallHelper.ResetCounter();
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "CatWallSlide";
        m_dir = dir;
        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
        CommonValues.PlayerVelocity = new Vector2(0,0);
    }

    public override void UpdateDirection(){

            m_controllabledObject.GetComponent<Player>().animationNode.position = 
                Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                    m_controllabledObject.transform.position, ref animationVel, m_smoothTime);


    }

    public override void Process(){
        if( m_detector.isOnGround()   ){
            m_isOver = true;
            m_nextState = new CatWallHold( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }
        if( !m_detector.isWallClose() ) m_isOver = true;
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
            PlayerSwipeLock.ResetCounter();
            m_isOver = true;
        }

    //    CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.y * ( int )m_dir;

        //velocity.x = (m_dir != GlobalUtils.Direction.Left )? -0.001f : 0.001f;
        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y -CatUtils.GravityForce * Time.deltaTime,
                                 -CatUtils.MaxWallSlideSpeed);
        if( PlayerInput.isSpecialKeyHold() ) CommonValues.PlayerVelocity.y = 0.0f;


        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        CatUtils.stamina = Mathf.Min( CatUtils.stamina + Mathf.Abs(CommonValues.PlayerVelocity.y) * Time.deltaTime, CatUtils.MaxStamina );
    }

    public override void HandleInput(){
        if( PlayerInput.isClimbKeyPressed() ){
            m_isOver = true;
            m_nextState = new CatWallClimb( m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }
        if( !PlayerInput.isSpecialKeyHold() ) {

            if( PlayerInput.isJumpKeyJustPressed() ){
                m_isOver = true;
                CommonValues.PlayerVelocity.x = 0;
                m_nextState = new CatWallJump(m_controllabledObject, m_dir);
            }
        //    }else if( isMovingLeft && CatUtils.isMoveRightKeyHold() ){
        //        m_isOver = true;
        //        m_nextState = new PlayerFall( m_controllabledObject, GlobalUtils.Direction.Right  );
        //    }else if(!isMovingLeft && CatUtils.isMoveLeftKeyHold() ){
        //        m_isOver = true;
        //        m_nextState = new PlayerFall( m_controllabledObject, GlobalUtils.Direction.Left  );
        //    }
        }else{
            if( PlayerInput.isJumpKeyJustPressed() ){
                m_isOver = true;
                CommonValues.PlayerVelocity.x = 0;
                m_nextState = new CatWallJump(m_controllabledObject, m_dir);
            }
        }
    }
}
