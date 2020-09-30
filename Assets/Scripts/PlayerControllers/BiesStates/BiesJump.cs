using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesJump : PlayerJump
{    

    public BiesJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, BiesUtils.infoPack ) {
        name = "BiesJump";
        StartAnimation();
    }

    private void StartAnimation(){
        m_animator.SetTrigger("BiesJumpPressed");
                m_animator.SetFloat( "AnimationSpeed", 3.0f);
        startAnimationDelay = getAnimationLenght( "BiesJumpPreparation")/3.0f;

        GlobalUtils.PlayerObject.GetComponent<Player>().StartCoroutine(StartJump(startAnimationDelay));
    }
    
    protected override IEnumerator StartJump( float time ){
        yield return new WaitForSeconds(time);
        if( m_isOver ) yield break;
        m_FloorDetector.CheatMove( new Vector2(0,4.0f));
        CommonValues.PlayerVelocity.y = JumpForce + GravityForce; 
        m_animator.ResetTrigger("BiesJumpPressed");
    }

    private bool isFalling(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) && CommonValues.PlayerVelocity.y <= 0 ){ 
            timeOfJumpForceRising = 0.0f;
            m_nextState = new BiesFall( m_controllabledObject, m_FloorDetector.GetCurrentDirection());
            return true;
        }
        return false;
    }

    protected override void ProcessStateEnd(){
        m_isOver |= isOnCelling() || isFalling() || isOnGround();
        if( m_isOver ){
            m_animator.ResetTrigger("BiesJumpPressed");
        }
    }

    public override void HandleInput(){
        if( m_ObjectInteractionDetector.canClimbLedge()  && !LockAreaOverseer.ledgeClimbBlock  ){
            m_isOver = true;
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
        }
        HandleInputSwipe();
        if( m_isOver ){
            m_animator.ResetTrigger("BiesJumpPressed");
        }
    }
}
