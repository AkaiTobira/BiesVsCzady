using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallClimb : PlayerBaseState
{
    public CatWallClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

    //    PlayerFallOfWallHelper.ResetCounter();
        m_dir = dir;
        name = "CatWallClimb" + ((isLeftOriented()) ? "L" : "R");

        SetUpRotation();
        m_FloorDetector.CheatMove(new Vector2(0,0.2f));
        CommonValues.PlayerVelocity.x = 10 * ( int )m_dir;
    }

    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    protected override void UpdateDirection(){
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -12.5f : 12.5f, -15f , 0);
    }

    public override void OnExit(){
    }

    public override void Process(){
    //    if( m_FloorDetector.isOnGround()   ) m_isOver = true;
    //    if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
    //        PlayerSwipeLock.ResetCounter();
    //        m_isOver = true;
    //    }

    //   velocity.x = CatUtils.MoveSpeedInAir;
    //  velocity.x *= ( int )m_dir;

    //    velocity.x = (m_dir != GlobalUtils.Direction.Left )? -CatUtils.WallClimbSpeed * Time.deltaTime : 0.001f;
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y + CatUtils.WallClimbSpeed * Time.deltaTime,
                                                    CatUtils.MaxWallClimbSpeed);
        if( PlayerInput.isSpecialKeyHold() ) CommonValues.PlayerVelocity.y = 0.0f;
        CatUtils.stamina -= Mathf.Abs(CommonValues.PlayerVelocity.y * Time.deltaTime);
        m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        m_animator.SetBool("isWallClose", m_WallDetector.isWallClose());
        if( !m_WallDetector.isWallClose() ) m_isOver = true;
    }

    public override void HandleInput(){

        if( PlayerInput.isJumpKeyJustPressed() ){
            m_isOver = true;
            m_nextState = new CatWallJump(m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
        }

        if( m_ObjectInteractionDetector.canClimbLedge()  && !LockAreaOverseer.ledgeClimbBlock ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( PlayerInput.isSpecialKeyHold() ){

        }else if( !PlayerInput.isClimbKeyHold() || CatUtils.stamina < 0){
            m_isOver = true;
            m_nextState = new CatWallSlide(m_controllabledObject, m_dir);
        }
    }

    public override string GetTutorialAdvice(){
        string msg = ( LockAreaOverseer.isChangeLocked ) ? "" : "E - ChangeForm";
        msg += "\nSPACE - Jump";
        msg += "\nW or Up - Climb up";
        msg += "\nSHIFT   - hold there";
        return msg;
    }

    public override string GetCombatAdvice(){
        string msg = ( LockAreaOverseer.isChangeLocked ) ? "" : "E - ChangeForm";
        msg += "\nSPACE - Jump";
        msg += "\nW or Up - Climb up";
        msg += "\nSHIFT   - hold there";
        return msg;
    }

}
