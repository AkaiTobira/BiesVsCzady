using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLedgeClimb : PlayerLedgeClimb
{

    public CatLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable, dir, 35f ) {
        name = "CatLedgeClimb";
        distanceToFixAnimation = new Vector3( (isLeftOriented())? 26.5f : -26.5f, -3.5f , 0);
        shiftValue = new Vector2((isLeftOriented())? 19 : -19, 15);

        Debug.Log( dir );
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger("CatClimb");
        timeToEnd = getAnimationLenght("CatLedgeClimb");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }
}
