using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFall : PlayerFall
{
    private float WallSlideDelay = 0.1f;

    public CatFall( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, CatUtils.infoPack ) {
        name = "CatFall";
        HandleSpecialBehaviour();
    }

    private void HandleSpecialBehaviour(){
        if( PlayerFallOfWallHelper.FallOfWallRequirementsMeet() || 
            PlayerMoveOfWallHelper.MoveOfWallRequirementsMeet() ) {
            CommonValues.PlayerVelocity.x = CatUtils.FallOffWallFactor * 
                                            ( isLeftOriented() ? 
                                                - CatUtils.MoveSpeedInAir : 
                                                  CatUtils.MoveSpeedInAir);
        }
    }


    public override void Process(){
        base.Process();
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -6.3f : 6.3f, 0 , 0);
        WallSlideDelay -= Time.deltaTime;
    }

    public override void HandleInput(){
        if( m_isOver ) return;
        HandleInputSwipe();
    //     if(PlayerJumpHelper.JumpRequirementsMeet( CatUtils.isJumpKeyJustPressed(), 
    //                                               m_FloorDetector.isOnGround() ))
    //    {
    //        m_nextState = new PlayerJump(m_controllabledObject, GlobalUtils.Direction.Left);
    //    }

        if( m_ObjectInteractionDetector.canClimbLedge() ){
            m_isOver = true;
            m_nextState = new CatLedgeClimb( m_controllabledObject, m_dir);
        }else if( m_WallDetector.isWallClose() && WallSlideDelay < 0){
            m_isOver = true;
            CommonValues.PlayerVelocity.x = 0; // To check if is needed;
            m_nextState = new CatWallSlide( m_controllabledObject, m_dir);
        }


    }
}
