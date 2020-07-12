using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHurt : PlayerHurt{    
    public CatHurt( GameObject controllable, 
                    GlobalUtils.AttackInfo infoPack) : base( controllable, infoPack, CatUtils.infoPack ){
        name = "CatHurt";

        CommonValues.PlayerVelocity *= 1.75f;

    }

    protected override void SetUpAnimation(){
        m_animator.SetTrigger( "CatHurt" );
        timeToEnd = getAnimationLenght("CatHurt");

        base.SetUpAnimation();
    }

    protected override void  ProcessStateEnd(){
        base.ProcessStateEnd();
        if( m_isOver ){
            m_animator.ResetTrigger( "CatHurt" );

            if( isFaceLocked ) {
                m_FloorDetector.Move( new Vector2( 0.001f, 0) * (int)savedDir);
            }
            if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround())){
                m_nextState = new CatFall( m_controllabledObject, m_dir);
            }
        }
    }

    public override void HandleInput(){}
}
