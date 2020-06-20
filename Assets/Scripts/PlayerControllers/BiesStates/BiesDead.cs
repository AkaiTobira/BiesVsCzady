using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BiesDead : BaseState{    
    private bool isMovingLeft = false;
    private float timeToEnd;
    private AnimationTransition m_transition;

    private float velocitXFriction = 0.0f;

    public BiesDead( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable ){
        isMovingLeft = m_detector.GetCurrentDirection() == GlobalUtils.Direction.Left;
        name = "BiesDead";
        m_animator.SetTrigger( "BiesDead" );
        timeToEnd = 3;//getAnimationLenght("BiesDead");

        velocity          = infoPack.knockBackValue;
        velocity.x        *= (int)infoPack.fromCameAttack;
        velocitXFriction  = infoPack.knockBackFrictionX * (int)infoPack.fromCameAttack;

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
 //           m_isOver = true;
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        velocity.y += -BiesUtils.GravityForce * Time.deltaTime;
        velocity.x = Mathf.Max( velocity.x - velocitXFriction, 0 );
        m_detector.Move(velocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
