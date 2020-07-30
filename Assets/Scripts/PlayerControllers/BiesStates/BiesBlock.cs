using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesBlock : PlayerBaseState{    
    private float timeToEnd;
    private AnimationTransition m_transition;


    public BiesBlock( GameObject controllable) : base( controllable ){
        name = "BiesBlock";
        m_animator.SetFloat("AnimationSpeed", 0.8f);
    //    distanceToFixAnimation = new Vector3(0, 7.5f , 0);
    }

    protected override void SetUpAnimation(){
        m_animator.SetBool("Block", true);
        timeToEnd = getAnimationLenght("BiesBlock") / 0.8f;

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }


    public override void OnExit(){
        m_animator.SetBool("Block", false);
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
        m_isOver |= isTimerOver();// || isWallHit();
        if( m_isOver ){
            m_animator.SetBool("Block", false);
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
