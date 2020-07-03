using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CzadAttack : EnemyBaseState
{

    float timeToEnd = 0;

    public CzadAttack( GameObject controllable ) : base( controllable ){
        name = "CzadAttack";

        timeToEnd = getAnimationLenght("CzadAttack");
    }

    public void SelectNextState(){
        if( timeToEnd < 0 ) m_isOver = true;

        if( m_isOver ) m_animator.SetBool( "Attack", false);
    }

    public override void Process(){
        base.Process();
        HandleStopping();
        SelectNextState();
        timeToEnd -= Time.deltaTime;
    }

    protected float getAnimationLenght(string animationName){
        RuntimeAnimatorController ac = m_animator.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }

    public override void UpdateAnimator(){
        base.UpdateAnimator();
        m_animator.SetBool( "Attack", true);
        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs( entityScript.velocity.x ));
    }

    private void HandleStopping(){
        float acceleration      = (entityScript.maxMoveSpeed / entityScript.moveBrakingTime) * Time.deltaTime;
        float currentValue      = Mathf.Max( Mathf.Abs( entityScript.velocity.x ) - acceleration, 0);
        entityScript.velocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }


}
