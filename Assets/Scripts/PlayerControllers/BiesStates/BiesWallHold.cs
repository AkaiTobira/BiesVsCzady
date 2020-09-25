using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesWallHold : PlayerBaseState
{
    public BiesWallHold( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        CatUtils.ResetStamina();
        m_dir = dir;
        name = "BiesWallHold" + ((isLeftOriented())? "L": "R");
        CommonValues.PlayerVelocity.y =0;

        SetUpAnimValue();
    }

    private void SetUpAnimValue(){
        m_animator.SetBool( "isWallMovable",  m_ObjectInteractionDetector.IsWallPullable() );
        if( m_ObjectInteractionDetector.IsWallPullable() ){
            distanceToFixAnimation = new Vector3(30, 6 , 0);
        }else{
            distanceToFixAnimation = new Vector3(15 * ((isLeftOriented())? 1: -2) , 6, 0);
        }
    }

    public override void Process(){
        m_ObjectInteractionDetector.UpdateDestroyableExistance();

        if( !m_WallDetector.isWallClose()) m_isOver = true;

        m_animator.SetFloat( "FallVelocity", 0);
        if( !m_FloorDetector.isOnGround() ){
            CommonValues.PlayerVelocity.y = -BiesUtils.GravityForce * Time.deltaTime;
            m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        }else{
            CommonValues.PlayerVelocity.y = 0;
        }
        
    }

    public override void OnExit(){
        if( isLeftOriented() &&  PlayerInput.isMoveRightKeyHold()  ){
            velocity.x = BiesUtils.PlayerSpeed * Time.deltaTime;
            m_FloorDetector.Move(velocity * Time.deltaTime);
        }else if( isRightOriented() && PlayerInput.isMoveLeftKeyHold() ){
            velocity.x = -BiesUtils.PlayerSpeed * Time.deltaTime;
            m_FloorDetector.Move(velocity * Time.deltaTime);
        }
        velocity = new Vector2();
    }

    private bool IsObjPullableCloseToWall(){
        return m_ObjectInteractionDetector.GetPullableObject().GetComponent<CollisionDetectorMovable>().canBePushedInDirection(  m_dir);
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) ){
            m_nextState = new BiesFall(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isAttack1KeyPressed() ){
            m_nextState = new BiesAttack1(m_controllabledObject);
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new BiesAttack2(m_controllabledObject);
        }else if ( m_ObjectInteractionDetector.IsWallPullable() && PlayerInput.isSpecialKeyHold() && IsObjPullableCloseToWall() ){

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
                                                   m_FloorDetector.isOnGround() )
        ){ 
            m_isOver = true;
            m_nextState = new BiesJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_ObjectInteractionDetector.enableFallForOneWayFloor();
            velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
            m_FloorDetector.Move( velocity * Time.deltaTime );
        }
    }

    public override string GetTutorialAdvice(){
        string msg = (( LockAreaOverseer.isChangeLocked ) ? "" : "E - ChangeForm");
        msg += "\nSPACE - Jump";
        msg += ( m_ObjectInteractionDetector.IsWallPullable()   ) ? "\nSHIFT + A/D or arrows to move object" : "";
        msg += ( m_ObjectInteractionDetector.IsWallDestroyable() ) ? "\nX or LMB - hit to destroy" : "";

        return msg;
    }

}
