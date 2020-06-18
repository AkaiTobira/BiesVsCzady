using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesHurt : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;
    private AnimationTransition m_transition;

    private float velocitXFriction = 0.0f;

    public BiesHurt( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == GlobalUtils.Direction.Left;
        name = "BiesHurt";
        m_animator.SetTrigger( "BiesHurt" );
//        timeToEnd = getAnimationLenght("BiesHurt");

        timeToEnd = 3;

        velocity          = infoPack.knockBackValue;
        velocity.x        *= (int)infoPack.fromCameAttack;
        velocitXFriction  = infoPack.knockBackFrictionX;


        if( velocitXFriction > 0){
            m_detector.CheatMove( new Vector2(0,40.0f));
        }

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
            m_animator.ResetTrigger( "BiesHurt" );
        }
    }

    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        float xMax = Mathf.Max(Mathf.Abs( velocity.x ) - velocitXFriction * Time.deltaTime, 0 );
        velocity.x = Mathf.Sign(velocity.x) * xMax;
        m_detector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
