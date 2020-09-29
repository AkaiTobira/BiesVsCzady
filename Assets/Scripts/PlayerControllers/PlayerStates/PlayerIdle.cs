using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerBaseState
{
    protected IPlatformEdgeDetector m_lowerEdgeDetector;

    public PlayerIdle(  GameObject controllable,
                        ICharacterSettings settings
     ) : base( controllable ) {
        name       = "PlayerIdle";
        m_settings = settings;
        m_dir      = m_FloorDetector.GetCurrentDirection();

        m_lowerEdgeDetector = controllable.GetComponent<IPlatformEdgeDetector>();
    }

    protected void HandleStopping(){
        float acceleration = (m_settings.PlayerSpeed / m_settings.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

    protected virtual void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", -2);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));
    }

    public override void Process(){
        HandleStopping();
        ProcessAnimationUpdate();
        if( ! m_FloorDetector.isOnGround() ){
            CommonValues.PlayerVelocity.y += -m_settings.GravityForce * Time.deltaTime;
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
        }else{
            CommonValues.PlayerVelocity.y = -m_settings.GravityForce * Time.deltaTime;
            m_FloorDetector.Move( CommonValues.PlayerVelocity * Time.deltaTime );
            CatUtils.ResetStamina();
            CommonValues.PlayerVelocity.y = 0;
        }
    }


    public override string GetTutorialAdvice(){
        string msg = ( LockAreaOverseer.isChangeLocked ) ? "" : "E - ChangeForm";
        msg += "\nWSAD or arrows - Move\n SPACE - Jump";
        return msg;
    }

}
