using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttackBase : PlayerBaseState
{ 

    protected float animationTime;
    protected float timeToEnd;
    private AnimationTransition m_transition;

    protected string animatorTriggerName = "";

    protected string animatorName = "";


    public BiesAttackBase( GameObject controllable, string triggerName, string annimation ) : base( controllable ){
        animatorTriggerName = triggerName;
        animatorName = annimation;


        
        SetUpAnimationOverride();
    }

    protected void SetUpAnimationOverride(){
        m_animator.SetBool(animatorTriggerName, true);
        timeToEnd     = animationTime;
        animationTime = getAnimationLenght(animatorName);

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    protected override void SetUpAnimation(){
    }

    public override void OnExit(){
        m_animator.SetBool(animatorTriggerName, false);
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool( animatorTriggerName, false);
        }
    }
    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity = (int)m_FloorDetector.GetCurrentDirection() * m_transition.MoveSpeed;
        m_FloorDetector.Move(velocity*Time.deltaTime);
    }
    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }

}
