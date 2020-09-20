using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAutoJump : BiesJump
{    

    public BiesAutoJump( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir) {
        name = "BiesAutoJump";
        CommonValues.PlayerVelocity.x = BiesUtils.PlayerSpeed * 0.65f * (int)dir;
//        Debug.Log( CommonValues.PlayerVelocity );
    }

    private float lostTime = 0.0f;

    public override void HandleInput(){
        lostTime -= Time.deltaTime;

        Debug.Log( "CCL" +  m_ObjectInteractionDetector.canClimbLedge() );

        if( m_ObjectInteractionDetector.canClimbLedge()  && !LockAreaOverseer.ledgeClimbBlock  ){
            m_isOver = true;
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
        }else if( PlayerInput.isJumpKeyJustPressed() ){
            timeOfJumpForceRising = Mathf.Max( BiesUtils.JumpMaxTime - lostTime, 0 );
            lostTime -= BiesUtils.JumpMaxTime;
        }

        HandleInputSwipe();
        if( m_isOver ){
            m_animator.ResetTrigger("BiesJumpPressed");
        }
    }
}
