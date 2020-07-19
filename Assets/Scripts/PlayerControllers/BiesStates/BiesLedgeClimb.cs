using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesLedgeClimb : PlayerLedgeClimb
{

    public BiesLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, 350 ) {
        name = "BiesLedgeClimb";
        shiftValue = new Vector2((isLeftOriented())? -600 : 600, 400);
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -381 : 381, 0 , 0);
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger("BiesClimb");
        timeToEnd = getAnimationLenght("BiesLedgeClimb");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }
}
