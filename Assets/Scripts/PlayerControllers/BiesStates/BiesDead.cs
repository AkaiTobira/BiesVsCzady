using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BiesDead : PlayerDead
{    
    public BiesDead( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable, infoPack, BiesUtils.infoPack ){
        name = "BiesDead";
        SetUpAnimation();
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger( "BiesDead" );
        timeToEnd = getAnimationLenght("BiesDead") + realoadDealay;
    }
}