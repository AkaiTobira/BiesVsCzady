using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerBaseState
{
    private bool isAccelerating = true;
    string m_formName;
    public PlayerMove(  GameObject controllable, 
                        GlobalUtils.Direction dir,
                        ICharacterSettings settings,
                        string formName
                     ) : base( controllable ) {
        name = formName + "Move";
        m_settings = settings;
        m_formName = formName;
        SetUpRotation();
        m_dir = dir;
    }
/*
    protected override void  SetUpAnimation(){
        if( CommonValues.needChangeDirection ){
            m_animator.SetTrigger( m_formName + "ChangingDirection");
            CommonValues.needChangeDirection = false;
        }
    }*/

    private void SetUpRotation(){
        if( CommonValues.needChangeDirection ){
            m_animator.SetTrigger( m_formName + "ChangingDirection");
            CommonValues.needChangeDirection = false;
        }

        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    private void ProcessGravity(){
        CommonValues.PlayerVelocity.y += -m_settings.GravityForce * Time.deltaTime;
        if( m_FloorDetector.isOnGround() ){
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = -m_settings.GravityForce * Time.deltaTime;
        }
    }


    private void ProcessAcceleration(){
        if( ! isAccelerating ) return;
        float acceleration = (m_settings.PlayerSpeed / m_settings.MoveAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( CommonValues.PlayerVelocity.x) + acceleration, m_settings.PlayerSpeed );
        CommonValues.PlayerVelocity.x = currentValue * (int)m_dir;
    }

    private void ProcessAnimationUpdate(){
        m_animator.SetFloat("FallVelocity", -2);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));
    }

    private void ProcessWallDetectiong(){
        if( isLeftOriented()   && m_WallDetector.isCollideWithLeftWall() ) CommonValues.PlayerVelocity.x = 0.0f;
        if( isRightOriented()  && m_WallDetector.isCollideWithRightWall()) CommonValues.PlayerVelocity.x = 0.0f;
    }

    public override void Process(){
        ProcessAcceleration();
        ProcessAnimationUpdate();
        ProcessWallDetectiong();
        ProcessGravity();

        m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);

        ProcessStateEnd();
    }

    protected virtual void ProcessStateEnd(){
        if( m_isOver ){
            m_animator.ResetTrigger( m_formName + "ChangingDirection");
        }
    }

    public override void HandleInput(){
        if( PlayerInput.isMoveLeftKeyHold() || PlayerInput.isMoveRightKeyHold()) {
            isAccelerating = true;
        }
    }

    public override string GetTutorialAdvice(){
        string msg = ( LockAreaOverseer.isChangeLocked ) ? "" : "E - ChangeForm";
        msg += "\nSPACE - Jump";
        return msg;
    }


}
