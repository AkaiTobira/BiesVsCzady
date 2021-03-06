﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatJump : PlayerJump
{    
    public CatJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir,  CatUtils.infoPack ) {
        name = "CatJump";
        distanceToFixAnimation = new Vector3(0, 3 , 0);
        StartAnimation();
    }

    private  void StartAnimation(){
        startAnimationDelay = getAnimationLenght( "CatJumpPreparation")/2.0f;
        m_animator.SetTrigger("CatJumpPressed");
        GlobalUtils.PlayerObject.GetComponent<Player>().StartCoroutine(StartJump(startAnimationDelay));
    }

    protected override IEnumerator StartJump( float time ){
        yield return new WaitForSeconds(time);
        if( m_isOver ) yield break;
        m_FloorDetector.CheatMove( new Vector2(0,4.0f));
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        m_animator.ResetTrigger("CatJumpPressed");
    }

    private bool isFalling(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) && CommonValues.PlayerVelocity.y < 0 ){ 
            timeOfJumpForceRising = 0.0f;
            m_nextState = new CatFall( m_controllabledObject, m_FloorDetector.GetCurrentDirection());
            return true;
        }
        return false;
    }

    protected override void ProcessStateEnd(){
        m_isOver |= isOnCelling() || isFalling() || isOnGround();
        if( m_isOver ){
            m_animator.ResetTrigger("CatJumpPressed");
        }
    }

    public override void HandleInput(){
        if( m_ObjectInteractionDetector.canClimbLedge() && !LockAreaOverseer.ledgeClimbBlock ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_WallDetector.isWallClose() && timeOfIgnoringWallStick < 0 ){
            if( isLeftOriented() && PlayerInput.isMoveLeftKeyHold() ){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            }else if ( isRightOriented() && PlayerInput.isMoveRightKeyHold()){
                m_isOver = true;
                m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
            }else if( PlayerInput.isClimbKeyHold() ){
                m_isOver = true;
                m_nextState = new CatWallClimb( m_controllabledObject, m_dir);
            }
        }
        HandleInputSwipe();
        if( m_isOver ){
            m_animator.ResetTrigger("CatJumpPressed");
        }
    }

    public override string GetCombatAdvice(){
        return "E - Change Form";
    }
}
