using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHurt : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;
    private AnimationTransition m_transition;

    private float velocitXFriction = 0.0f;

    public CatHurt( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == GlobalUtils.Direction.Left;
        name = "CatHurt";
        m_animator.SetTrigger( "CatHurt" );

        timeToEnd = 3;
//        timeToEnd = getAnimationLenght("CatHurt");

        velocity          = infoPack.knockBackValue * 1.75f;

        velocity.x        *= (int)infoPack.fromCameAttack;
        velocitXFriction  = infoPack.knockBackFrictionX;

        if( velocitXFriction > 0){
            m_detector.CheatMove( new Vector2(0,40.0f));
        }

        Debug.Log( "KNOCKBACKVALUES " + velocity.ToString());

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
            m_animator.ResetTrigger( "CatHurt" );
        }
    }

    private void ProcessMove(){
        Debug.Log( velocity);
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity.y += -CatUtils.GravityForce * Time.deltaTime;
        float xMax = Mathf.Max(Mathf.Abs( velocity.x ) - (velocitXFriction * Time.deltaTime), 0 );

        velocity.x = Mathf.Sign(velocity.x) * xMax;
        m_detector.Move(velocity*Time.deltaTime);
        Debug.Log( velocity);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }

    public override void HandleInput(){
    }
}
