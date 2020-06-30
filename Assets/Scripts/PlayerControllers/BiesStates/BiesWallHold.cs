using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesWallHold : BaseState
{
    public BiesWallHold( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        CatUtils.ResetStamina();
        m_dir = dir;
        name = "BiesWallHold" + ((isLeftOriented())? "L": "R");
        CommonValues.PlayerVelocity.y =0;
    }

    public override void Process(){
        if( !m_detector.isWallClose()) m_isOver = true;

        m_animator.SetFloat( "FallVelocity", 0);
        if( !m_detector.isOnGround() ){
            CommonValues.PlayerVelocity.y = -BiesUtils.GravityForce * Time.deltaTime;
            m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        }else{
            CommonValues.PlayerVelocity.y = 0;
        }
        
    }

    public override void OnExit(){
        if( isLeftOriented() &&  PlayerInput.isMoveRightKeyHold()  ){
            velocity.x = BiesUtils.PlayerSpeed * Time.deltaTime;
            m_detector.Move(velocity * Time.deltaTime);
        }else if( isRightOriented() && PlayerInput.isMoveLeftKeyHold() ){
            velocity.x = -BiesUtils.PlayerSpeed * Time.deltaTime;
            m_detector.Move(velocity * Time.deltaTime);
        }
        velocity = new Vector2();
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if ( m_detector.IsWallPullable() && PlayerInput.isSpecialKeyHold() ){

            if( isLeftOriented() ){
                if( PlayerInput.isMoveRightKeyHold()){
                    m_nextState = new BiesPullObj( m_controllabledObject, GlobalUtils.Direction.Left);
                }else if( PlayerInput.isMoveLeftKeyHold() ){
                    m_nextState = new BiesPushObj( m_controllabledObject, GlobalUtils.Direction.Left);
                }
            }else{
                if( PlayerInput.isMoveRightKeyHold()){
                    m_nextState = new BiesPushObj( m_controllabledObject, GlobalUtils.Direction.Right);
                }else if( PlayerInput.isMoveLeftKeyHold() ){
                    m_nextState = new BiesPullObj( m_controllabledObject, GlobalUtils.Direction.Right);
                }
            } 
        }else if( isLeftOriented() && PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
            CommonValues.needChangeDirection = true;
            m_nextState = new BiesMove(m_controllabledObject, GlobalUtils.Direction.Right); 
        }else if( isRightOriented() && PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
            CommonValues.needChangeDirection = true;
            m_nextState = new BiesMove(m_controllabledObject, GlobalUtils.Direction.Left); 
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
            m_detector.Move( velocity * Time.deltaTime );
        }
    }

}
