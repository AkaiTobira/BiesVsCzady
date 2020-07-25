using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLedgeClimb : PlayerLedgeClimb
{

    public CatLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, 350f ) {
        name = "CatLedgeClimb";
        distanceToFixAnimation = new Vector3( (isLeftOriented())? -75 : 75, 150 , 0);
        shiftValue = new Vector2( (isLeftOriented())? -400 : 400, 250);
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger("CatClimb");
        timeToEnd = getAnimationLenght("CatLedgeClimb");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }
}
