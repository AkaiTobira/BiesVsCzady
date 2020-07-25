using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttack3 : PlayerBaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;


    public BiesAttack3( GameObject controllable) : base( controllable ){
        name = "BiesAttack3";
        distanceToFixAnimation = new Vector3(0, 75 , 0);
    }

    protected override void SetUpAnimation(){
        m_animator.SetBool("Attack3", true);
        timeToEnd = getAnimationLenght("PlayerAttack3");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }


    public override void OnExit(){
        m_animator.SetBool("Attack3", false);
    }

    private bool isTimerOver(){
        if( timeToEnd < 0){
            m_isOver = true;
            return true;
        }
        return false;
    }

    private bool isWallHit(){
        if( m_WallDetector.isWallClose() ){
            //TOWALLHITSTATE 
            return true;
        }
        return false;
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        m_isOver |= isTimerOver() || isWallHit();
        if( m_isOver ){
            m_animator.SetBool("Attack3", false);
        }
    }

    private void ProcessMove(){
        velocity.x   = (int)m_FloorDetector.GetCurrentDirection() * m_transition.MoveSpeed.x;
        if( m_FloorDetector.isOnGround() ){
            PlayerFallHelper.FallRequirementsMeet( true );
            velocity.y = 0;
        }else{
            velocity.y += -PlayerUtils.GravityForce*Time.deltaTime;
        }
        m_FloorDetector.Move(velocity*Time.deltaTime);
    }
    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
