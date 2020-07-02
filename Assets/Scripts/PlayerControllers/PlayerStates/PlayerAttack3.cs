using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack3 : PlayerBaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;

    public PlayerAttack3( GameObject controllable) : base( controllable ){
        name = "PlayerAttack3";
        m_animator.SetBool("Attack3", true);
        timeToEnd = getAnimationLenght("PlayerAttack3");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    private bool isAnimationFinished(){
        if( timeToEnd < 0){
            m_animator.SetBool("Attack3", false);
            return true;
        }
        return false;
    }

    private bool isCloseToWall(){
        if( m_detector.isWallClose() ){
            m_animator.SetBool("Attack3", false);
            return true;
            //TOWALLHITSTATE 
        }
        return false;
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        m_isOver = isAnimationFinished() || isCloseToWall();
    }
    private void ProcessMove(){
        velocity.x   = (int)m_detector.GetCurrentDirection() * m_transition.MoveSpeed.x;
        if( m_detector.isOnGround() ){
            PlayerFallHelper.FallRequirementsMeet( true );
            velocity.y = 0;
        }else{
            velocity.y += -PlayerUtils.GravityForce*Time.deltaTime;
        }
        m_detector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){}
}
