using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallSlide : PlayerBaseState
{
    public CatWallSlide( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        m_dir = dir;
        name = "CatWallSlide" + ((isLeftOriented()) ? "L" : "R");
        PlayerFallOfWallHelper.ResetCounter();
        PlayerMoveOfWallHelper.DisableCounter();
        CommonValues.PlayerVelocity = new Vector2(0,0);
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -12.5f : 12.5f, 0 , 0);
    }

    protected override void SetUpAnimation(){
        m_animator.SetBool("isWallClose", true);
        m_animator.SetBool("isSliding", true);
    }


    protected override void UpdateDirection(){}


    private bool isOnGround(){
        if( m_FloorDetector.isOnGround() ){
            m_nextState = new CatWallHold( m_controllabledObject, m_dir);
            return true;
        }
        return false;
    }

    private bool isFallingOffWall(){
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet()){
            PlayerSwipeLock.ResetCounter();
            return true;
        }
        return false;
    }

    private bool isMovingOffWall(){
        if( PlayerMoveOfWallHelper.MoveOfWallRequirementsMeet()  ){
            m_FloorDetector.Move( new Vector2( ( isRightOriented() ) ? -4 : 4, 0));
            m_nextState = new CatFall( m_controllabledObject,  GlobalUtils.ReverseDirection( m_dir ) );
            m_animator.SetBool("isWallClose", false);
            m_animator.SetBool("isSliding", false);
            m_animator.SetFloat( "FallVelocity", -2);
            return true;
        }
        return false;
    }

    private bool isFarFromWall(){
        return !m_WallDetector.isWallClose();
    }

    private void  ProcessStateEnd(){
        m_isOver |= isOnGround() || isFallingOffWall() || isMovingOffWall() || isFarFromWall();

        if( m_isOver){
            m_animator.SetBool("isWallClose", false);
            m_animator.SetBool("isSliding", false);
            m_animator.SetBool("isSliding", m_FloorDetector.isOnGround());
        }
    }

    public override void Process(){

        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        m_animator.SetBool("isSliding", true);
        m_animator.SetBool("isWallClose", m_WallDetector.isWallClose());
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -12.5f : 12.5f, 0 , 0);


        CommonValues.PlayerVelocity.y = Mathf.Max( CommonValues.PlayerVelocity.y -CatUtils.GravityForce * Time.deltaTime,
                                                    -CatUtils.MaxWallSlideSpeed);
        if( PlayerInput.isSpecialKeyHold() ) CommonValues.PlayerVelocity.y = 0.0f;
        
        m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        ProcessStaminaUpdate();
        ProcessStateEnd();
    }

    private void ProcessStaminaUpdate(){
        CatUtils.stamina = Mathf.Min( CatUtils.stamina + Mathf.Abs(CommonValues.PlayerVelocity.y) * Time.deltaTime, 
                                      CatUtils.MaxStamina );
    }

    public override void HandleInput(){
        if( PlayerInput.isClimbKeyHold() && CatUtils.stamina > CatUtils.MaxStamina/2.0f ){
            m_isOver = true;
            m_nextState = new CatWallClimb( m_controllabledObject, m_dir);
        }
        if( !PlayerInput.isSpecialKeyHold() ) {
            m_animator.SetBool("isSliding", false);
            m_animator.SetBool("isHoldKeyPressed", false);

            if( PlayerInput.isJumpKeyJustPressed() ){
                m_isOver = true;
                CommonValues.PlayerVelocity.x = 0;
                
                m_nextState = new CatWallJump(m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }else if( ( m_dir == GlobalUtils.Direction.Left ) && PlayerInput.isMoveRightKeyHold() ){
                PlayerMoveOfWallHelper.EnableCounter();
            }else if( ( m_dir == GlobalUtils.Direction.Right) && PlayerInput.isMoveLeftKeyHold() ){
                PlayerMoveOfWallHelper.EnableCounter();
            }else if( PlayerInput.isFallKeyHold() ){
                PlayerMoveOfWallHelper.EnableCounter();
            }
        }else{
            m_animator.SetBool("isHoldKeyPressed", true);
            m_animator.SetBool("isSliding", true);
            if( PlayerInput.isJumpKeyJustPressed() ){
                m_isOver = true;
                CommonValues.PlayerVelocity.x = 0;
                m_nextState = new CatWallJump(m_controllabledObject, GlobalUtils.ReverseDirection(m_dir));
            }
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
