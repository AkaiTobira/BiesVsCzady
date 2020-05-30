using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2 : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;

    private AnimationTransition m_transition;


    public PlayerAttack2( GameObject controllable) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == PlayerUtils.Direction.Left;
        name = "PlayerAttack2";
        m_animator.SetBool("Attack2", true);
        timeToEnd = getAnimationLenght("PlayerAttack2");

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
            m_animator.SetBool("Attack2", false);
        }
    }
    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity = (int)m_detector.GetCurrentDirection() * m_transition.MoveSpeed;
        m_detector.Move(velocity*Time.deltaTime);
    }
    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
