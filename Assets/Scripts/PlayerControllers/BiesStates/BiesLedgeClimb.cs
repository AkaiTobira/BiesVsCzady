using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesLedgeClimb : PlayerLedgeClimb
{

    public BiesLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, 35 ) {
        name = "BiesLedgeClimb";
        shiftValue = new Vector2((isLeftOriented())? -60 : 60, 40);
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -38.1f : 38.1f, 0 , 0);
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger("BiesClimb");
        timeToEnd = getAnimationLenght("BiesLedgeClimb");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }
}
