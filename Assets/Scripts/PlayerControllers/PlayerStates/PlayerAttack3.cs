using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack3 : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;

    private AnimationTransition m_transition;


    public PlayerAttack3( GameObject controllable) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == GlobalUtils.Direction.Left;
        name = "PlayerAttack3";
        m_animator.SetBool("Attack3", true);
        timeToEnd = getAnimationLenght("PlayerAttack3");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
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
            m_animator.SetBool("Attack3", false);
            Debug.Log("TimeEnd");
        }

        if( m_detector.isWallClose() ){
            Debug.Log("HIT A WALL");
            m_isOver = true;
            m_animator.SetBool("Attack3", false);
            //TOWALLHITSTATE 
        }
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
    public override void HandleInput(){
    }
}
