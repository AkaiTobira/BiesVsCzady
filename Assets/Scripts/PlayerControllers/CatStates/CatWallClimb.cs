using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallClimb : BaseState
{
    public CatWallClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

    //    PlayerFallOfWallHelper.ResetCounter();
        name = "CatWallClimb";
        m_dir = dir;

        SetUpRotation();
        m_detector.CheatMove(new Vector2(0,2));
        CommonValues.PlayerVelocity.x = CatUtils.MoveSpeedInAir * ( int )m_dir;
    }

    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    protected override void UpdateDirection(){
    }

    public override void OnExit(){
    }

    public override void Process(){
    //    if( m_detector.isOnGround()   ) m_isOver = true;
    //    if( !m_detector.isWallClose() ) m_isOver = true;
    //    if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
    //        PlayerSwipeLock.ResetCounter();
    //        m_isOver = true;
    //    }

    //   velocity.x = CatUtils.MoveSpeedInAir;
    //  velocity.x *= ( int )m_dir;

    //    velocity.x = (m_dir != GlobalUtils.Direction.Left )? -CatUtils.WallClimbSpeed * Time.deltaTime : 0.001f;

        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y + CatUtils.WallClimbSpeed * Time.deltaTime,
                                                    CatUtils.MaxWallClimbSpeed);
        if( PlayerInput.isSpecialKeyHold() ) CommonValues.PlayerVelocity.y = 0.0f;
        CatUtils.stamina -= Mathf.Abs(CommonValues.PlayerVelocity.y * Time.deltaTime);
        m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
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
            m_nextState = new CatWallSlide(m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }
    }
}
