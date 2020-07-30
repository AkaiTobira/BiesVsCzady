using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadShot : EnemyBaseState
{

    float timeToEnd = 0;

    public CzadShot( GameObject controllable ) : base( controllable ){
        name = "CzadShot";
        timeToEnd = getAnimationLenght("CzadShot");
        entityScript.delayOfHurtGoInTimer = 0;
    }

    public void SelectNextState(){
        if( timeToEnd < 0 ) m_isOver = true;
        m_animator.SetBool( "Shot", !m_isOver);
    }


    public override void Process(){
        base.Process();
        HandleStopping();
        SelectNextState();
        timeToEnd -= Time.deltaTime;
    }

    public override void UpdateAnimator(){
        m_dir = (GlobalUtils.Direction)Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x);;
        base.UpdateAnimator();
        m_animator.SetBool( "Shot", !m_isOver);
        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs( entityScript.velocity.x ));
    }

    private void HandleStopping(){
        float acceleration      = (entityScript.maxMoveSpeed / entityScript.moveBrakingTime) * Time.deltaTime;
        float currentValue      = Mathf.Max( Mathf.Abs( entityScript.velocity.x ) - acceleration, 0);
    //    entityScript.velocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }


}
