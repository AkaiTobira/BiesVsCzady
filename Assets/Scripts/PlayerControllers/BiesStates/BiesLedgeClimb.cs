using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesLedgeClimb : PlayerBaseState
{
    private float timeToEnd;
    private AnimationTransition m_transition;

    public BiesLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "BiesLedgeClimb";
        CommonValues.PlayerVelocity = new Vector2(0,0);
        m_dir = dir;
        SetUpRotation();
        PlayerFallOfWallHelper.ResetCounter();
    }
    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    protected override void  SetUpAnimation(){
        timeToEnd = getAnimationLenght("PlayerLedgeClimb");
        m_animator.SetTrigger("BiesClimb");
        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    public override void OnExit(){
        CommonValues.PlayerVelocity = new Vector2(0,0);
    }

    protected override void UpdateDirection(){}

    public override void Process(){
        velocity.x   = (int)m_detector.GetCurrentDirection() * m_transition.MoveSpeed.x;
        velocity.y   = m_transition.MoveSpeed.y;

        m_detector.CheatMove( velocity * Time.deltaTime );
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0 ) m_isOver = true;
    }

    public override void HandleInput(){
    }
}
