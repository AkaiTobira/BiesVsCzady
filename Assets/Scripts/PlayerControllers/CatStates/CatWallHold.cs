using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWallHold : BaseState
{
    private bool isMovingLeft = false;

    public CatWallHold( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP
   //     controllable.transform.GetComponent<Player>().changeDirection(dir);
        CatUtils.ResetStamina();
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "CatWallHold" + ((isMovingLeft)? "L": "R");
        m_dir = dir;
        CommonValues.PlayerVelocity.y =0;

    }

    public override void Process(){
        if( !m_detector.isWallClose()) m_isOver = true;

        if( !m_detector.isOnGround() ){
            CommonValues.PlayerVelocity.y = -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        }else{
            CommonValues.PlayerVelocity.y = 0.0f;
        }
    }

    public override void OnExit(){
    }

    public override void HandleInput(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            //m_isOver = true;
            m_nextState = new CatFall(m_controllabledObject, m_detector.GetCurrentDirection());
        }else if( PlayerInput.isAttack2KeyPressed() ){
            m_nextState = new CatAttack2(m_controllabledObject);
        }else if( isMovingLeft && PlayerInput.isMoveRightKeyHold()){
            m_isOver = true;
            CommonValues.PlayerVelocity.x = CatUtils.PlayerSpeed * Time.deltaTime;
            m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        //    m_nextState = new PlayerMove(m_controllabledObject, GlobalUtils.Direction.Right); 
        }else if( !isMovingLeft && PlayerInput.isMoveLeftKeyHold()){
            m_isOver = true;
            CommonValues.PlayerVelocity.x = -CatUtils.PlayerSpeed * Time.deltaTime;
            m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
        //    m_nextState = new PlayerMove(m_controllabledObject, GlobalUtils.Direction.Left); 
        }else if( 
            PlayerJumpHelper.JumpRequirementsMeet( PlayerInput.isJumpKeyJustPressed(), 
                                                   m_detector.isOnGround() )
        ){ 
            m_nextState = new CatJump(m_controllabledObject, GlobalUtils.Direction.Left);
        }else if( PlayerInput.isFallKeyHold() ) {
            m_detector.enableFallForOneWayFloor();
            CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
            m_detector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }else if( PlayerInput.isClimbKeyPressed() ){
        //    m_isOver = true;
            m_isOver = true;
            m_nextState = new CatWallClimb( m_controllabledObject, m_dir);
        }
    }

}
