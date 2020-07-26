using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerBaseState
{

    private ICharacterSettings m_settings;

    public PlayerIdle(  GameObject controllable,
                        ICharacterSettings settings
     ) : base( controllable ) {
        name       = "PlayerIdle";
        m_settings = settings;
        m_dir      = m_FloorDetector.GetCurrentDirection();

    }

    protected void HandleStopping(){
        float acceleration = (m_settings.PlayerSpeed / m_settings.MoveBrakingTime) * Time.deltaTime;
        float currentValue = Mathf.Max( Mathf.Abs( CommonValues.PlayerVelocity.x) - acceleration, 0);
        CommonValues.PlayerVelocity.x = currentValue * (int)m_FloorDetector.GetCurrentDirection();
    }

    protected virtual void ProcessAnimationUpdate(){
        m_animator.SetFloat( "FallVelocity", -2);
        m_animator.SetFloat("MoveVelocity", Mathf.Abs(CommonValues.PlayerVelocity.x));

        if( m_animator.GetBool("SneakySneaky") ){
            distanceToFixAnimation = new Vector3(0, -145.51f , 0);
        }else{
            distanceToFixAnimation = new Vector3(0, -60 , 0);
        }

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
        return "E - ChangeForm\nWSAD or arrows - Move\n SPACE - Jump";
    }

}
