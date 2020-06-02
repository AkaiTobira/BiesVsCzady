using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimb : BaseState
{
    private bool isMovingLeft = false;
    //private bool climbing = false;

    private float timeToEnd;
    private AnimationTransition m_transition;

    public PlayerLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

        PlayerFallOfWallHelper.ResetCounter();

        isMovingLeft = dir == GlobalUtils.Direction.Left;
        name = "LedgeClimb";
        m_dir = dir;
        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);

        timeToEnd = getAnimationLenght("PlayerLedgeClimb");
        m_animator.SetTrigger("Climb");
        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    public override void UpdateDirection(){
            m_controllabledObject.GetComponent<Player>().animationNode.position = 
                Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                    m_controllabledObject.transform.position, ref animationVel, m_smoothTime);
    }

    private float getAnimationLenght(string animationName){
        RuntimeAnimatorController ac = m_animator.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }

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
