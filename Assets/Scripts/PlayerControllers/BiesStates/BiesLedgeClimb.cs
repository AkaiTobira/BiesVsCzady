using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesLedgeClimb : PlayerLedgeClimb
{

    public BiesLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, 25 ) {
        name = "BiesLedgeClimb";
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -25f  : 25f  , 0 , 0);

    }

//38.1
    protected override void  SetUpAnimation(){
        m_animator.SetTrigger("BiesClimb");
        timeToEnd = getAnimationLenght("BiesLedgeClimb");
        maxOfAnimationToEnd = timeToEnd;
        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }
}
