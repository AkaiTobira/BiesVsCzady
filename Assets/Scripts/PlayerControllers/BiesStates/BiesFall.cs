using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesFall : PlayerFall
{
    public BiesFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, BiesUtils.infoPack ) {
        name = "BiesFall";
    }

    public override void Process(){
        base.Process();
        if(  m_isOver ) GUIElements.cameraShake.TriggerShake(0.3f);
    }

    public override void HandleInput(){
        if( m_isOver ) return;
        HandleInputSwipe();

    //     if(PlayerJumpHelper.JumpRequirementsMeet( PlayerUtils.isJumpKeyJustPressed(), 
    //                                               m_FloorDetector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }


        Debug.Log( "CCL" +  m_ObjectInteractionDetector.canClimbLedge() );


        if( m_ObjectInteractionDetector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new BiesLedgeClimb( m_controllabledObject, m_dir);
        }

    }
}
