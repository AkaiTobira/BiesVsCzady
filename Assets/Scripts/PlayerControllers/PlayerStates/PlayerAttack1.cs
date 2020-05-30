using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1 : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;

    private Vector2 moveDistance = new Vector2();
//    private bool isAccelerating  = false;

    public PlayerAttack1( GameObject controllable) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == PlayerUtils.Direction.Left;
        name = "PlayerAttack1";
        m_animator.SetBool("Attack1", true);
        timeToEnd = getAnimationLenght("PlayerAttack1");

        moveDistance = (int)m_detector.GetCurrentDirection() * PlayerUtils.MoveDistanceDuringAttack1;

        velocity = moveDistance/timeToEnd;
    }

    private float getAnimationLenght(string animationName){
        RuntimeAnimatorController ac = m_animator.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool("Attack1", false);
        }
    }

    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        m_detector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }

    public override void HandleInput(){
/*
        if( m_detector.isWallClose() ){
            m_isOver = true;
            m_nextState = new PlayerSlide( m_controllabledObject, PlayerUtils.ReverseDirection(m_dir));
        }

        if( PlayerUtils.isMoveLeftKeyHold() ){
            swipeOn = true;
            m_swipe = PlayerUtils.Direction.Left;
        }else if( PlayerUtils.isMoveRightKeyHold() ){
            swipeOn = true;
            m_swipe = PlayerUtils.Direction.Right;
        }else{
            swipeOn = false;
        }
*/

    }
}
