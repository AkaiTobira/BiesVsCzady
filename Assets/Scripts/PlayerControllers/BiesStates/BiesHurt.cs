using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesHurt : PlayerHurt{
    public BiesHurt( GameObject controllable, 
                     GlobalUtils.AttackInfo infoPack) : base( controllable, infoPack,  BiesUtils.infoPack ){
        name = "BiesHurt";
    }

    protected override void SetUpAnimation(){
        m_animator.SetTrigger( "BiesHurt" );
        timeToEnd = getAnimationLenght("BiesHurt");

        base.SetUpAnimation();
    }

    protected override void  ProcessStateEnd(){
        base.ProcessStateEnd();
        if( m_isOver ){
            m_animator.ResetTrigger( "BiesHurt" );
            if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround())){
                m_nextState = new BiesFall( m_controllabledObject, m_dir);
            }
        }
    }
    public override void HandleInput(){
    }
}
