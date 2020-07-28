using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CatDead : PlayerDead{
    public CatDead( GameObject controllable, GlobalUtils.AttackInfo infoPack) : base( controllable, infoPack, CatUtils.infoPack ){
        name = "CatDead";
        SetUpAnimation();
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger( "CatDead" );
        timeToEnd = getAnimationLenght("CatDead") + realoadDealay;
    }

}
