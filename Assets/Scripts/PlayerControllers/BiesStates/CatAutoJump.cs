using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAutoJump : CatJump
{    

    public CatAutoJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir) {
        name = "CatAutoJump";
        CommonValues.PlayerVelocity.x = CatUtils.PlayerSpeed * 0.5f * (int)dir;
    }

    private float lostTime = 0.0f;

    public override void HandleInput(){
        lostTime -= Time.deltaTime;
        if( m_ObjectInteractionDetector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( PlayerInput.isJumpKeyJustPressed() ){
            timeOfJumpForceRising = Mathf.Max( CatUtils.JumpMaxTime - lostTime, 0 );
            lostTime -= CatUtils.JumpMaxTime;
        }

        HandleInputSwipe();
        if( m_isOver ){
            m_animator.ResetTrigger("CatJumpPressed");
        }
    }
}
