using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesAttack1 : PlayerBaseState
{ 

    private float animationTime;
    private float timeToEnd;
    private AnimationTransition m_transition;

    float ANNIMATION_SPEED = 2f;

    GlobalUtils.Direction m_lockedDirection;

    public BiesAttack1( GameObject controllable) : base( controllable ){
        name = "BiesAttack1";
        distanceToFixAnimation = new Vector3(0, 7.5f , 0);
        m_animator.SetBool("Attack1", true);
        animationTime = getAnimationLenght("PlayerAttack1") / (ANNIMATION_SPEED);// * 3.0f);

        m_animator.SetFloat("AnimationSpeed", ANNIMATION_SPEED );// 3);

        m_lockedDirection = m_FloorDetector.GetCurrentDirection();

//        Debug.Log(animationTime);
        timeToEnd     = animationTime;

        velocity = new Vector2( 30, 0 );
    }

    protected override void SetUpAnimation(){
        

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }


    public override void OnExit(){
        m_animator.SetBool("Attack1", false);
    }


    private void  ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0){
            m_isOver = true;
            m_animator.SetBool("Attack1", false);
        }
    }


    private void ProcessMove(){
        PlayerFallHelper.FallRequirementsMeet( true );
        m_FloorDetector.Move((float)m_lockedDirection * velocity * Time.deltaTime);
    }
    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    //    HandleStopping();
    }

    private void HandleStopping(){
        float acceleration = (BiesUtils.PlayerSpeed / BiesUtils.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

    public override void HandleInput(){

        if( PlayerInput.isAttack1KeyPressed() && timeToEnd < 0.5 * animationTime ){
            m_isOver = true;
            m_nextState = new BiesAttack4( m_controllabledObject);
        }


    }
}
