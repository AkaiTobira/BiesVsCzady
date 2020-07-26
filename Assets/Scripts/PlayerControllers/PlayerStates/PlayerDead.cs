using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerDead : PlayerBaseState{    
    protected float timeToEnd;
    private AnimationTransition m_transition;
    private float velocitXFriction = 0.0f;
    protected float realoadDealay = 3.0f;

    public PlayerDead(  GameObject controllable, 
                        GlobalUtils.AttackInfo infoPack,
                        ICharacterSettings settings) : base( controllable ){
        m_settings = settings;
        name = "PlayerDead";
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger( "CatDead" );
        timeToEnd = getAnimationLenght("CatDead") + realoadDealay;

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            GlobalUtils.TaskMaster.SetPlayerAtLastCheckpoint();
        }
    }

    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity.y += -m_settings.GravityForce * Time.deltaTime;
        velocity.x = Mathf.Max( velocity.x - velocitXFriction, 0 );
        m_FloorDetector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){}

    public override string GetTutorialAdvice(){
        return "";
    }

}
