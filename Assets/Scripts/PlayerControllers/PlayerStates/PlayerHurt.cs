using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : PlayerBaseState{    
    protected float timeToEnd;
    protected AnimationTransition m_transition;
    protected float velocitXFriction = 0.0f;

    protected float knocBackDirection;

    protected bool isFaceLocked = false;

    protected GlobalUtils.Direction savedDir;

    ICharacterSettings m_settings;


    public PlayerHurt(  GameObject controllable, 
                        GlobalUtils.AttackInfo infoPack,
                        ICharacterSettings settings
                        ) : base( controllable ){

        Debug.Log( "Player Hurt");
        PlayerFallHelper.FallRequirementsMeet( true );
        m_settings = settings;
        savedDir     = m_FloorDetector.GetCurrentDirection();
        fillKnockbackInfo( infoPack );
        m_FloorDetector.CheatMove( new Vector2(0,40.0f));

    }

    protected override void UpdateDirection(){
        if( !isFaceLocked ) base.UpdateDirection();
    }


    protected override void SetUpAnimation(){
        m_transition = m_controllabledObject.
               GetComponent<Player>().animationNode.
               GetComponent<AnimationTransition>();
    }

    private void fillKnockbackInfo( GlobalUtils.AttackInfo infoPack ){
        isFaceLocked = infoPack.lockFaceDirectionDuringKnockback;

        if( isFaceLocked ){
            knocBackDirection = (int)infoPack.fromCameAttack;
            CommonValues.PlayerVelocity   = infoPack.knockBackValue;
            CommonValues.PlayerVelocity.x *= (int)infoPack.fromCameAttack;
        }else{
            //m_dir = infoPack.fromCameAttack;
            knocBackDirection = Mathf.Sign(CommonValues.PlayerVelocity.x);
            CommonValues.PlayerVelocity   = infoPack.knockBackValue;
        }

        if( velocitXFriction > 0){
            m_FloorDetector.CheatMove( new Vector2(0,40.0f));
        }

        Debug.Log( "PlayerHurt :" +  isFaceLocked.ToString() +  CommonValues.PlayerVelocity.ToString() );
    }


    protected virtual void ProcessStateEnd(){
        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0 ){// && m_FloorDetector.isOnGround()) {
            m_isOver = true;
            if( isFaceLocked ) m_FloorDetector.Move( new Vector2( 0.001f, 0) * (int)savedDir);
        }
    }

    private void ProcessMove(){
        m_animator.SetFloat( "FallVelocity", CommonValues.PlayerVelocity.y);
        m_animator.SetBool(  "isGrounded", m_FloorDetector.isOnGround());
        PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround() );
    //    CommonValues.PlayerVelocity.y += -m_settings.GravityForce * Time.deltaTime;

        Debug.Log( "ProcessMove before IF" + CommonValues.PlayerVelocity.y + " >>>> " + m_settings.GravityForce + " << " +m_settings.GravityForce * Time.deltaTime );

        if( knocBackDirection == -1 ) {
            CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x - velocitXFriction * Time.deltaTime;
        }else{
            CommonValues.PlayerVelocity.x = CommonValues.PlayerVelocity.x + velocitXFriction * Time.deltaTime;
        }

        Debug.Log( "ProcessMove after IF"  + CommonValues.PlayerVelocity.y);

        m_FloorDetector.Move(CommonValues.PlayerVelocity*Time.deltaTime);
    }

    public override void Process(){
        ProcessStateEnd();
        ProcessMove();
    }
    public override void HandleInput(){
    }
}
