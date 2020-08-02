using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  ICharacterSettings
{
    public float PlayerSpeed         = 15.0f;
    public float JumpMaxTime         = 0.25f;
    public float PlayerJumpForceMax  = 20.0f;
    public float PlayerJumpForceMin  = 0.0f;
    public float GravityForce        = 15.0f;
    public float MoveSpeedInAir      = 5.0f;
    public float maxMoveDistanceInAir   = 0.0f;
    public float Attack1Damage = 2;
    public float Attack2Damage = 0;
    public float Attack3Damage = 5;
    public Vector2 KnockBackValueAttack1 =  new Vector2( 100, 1000);
    public Vector2 KnockBackValueAttack2 =  new Vector2( 100, 400);
    public Vector2 KnockBackValueAttack3 =  new Vector2( 100, 400);

    public float RoarDamage = 0;
    public Vector2 KnockBackValueRoar =  new Vector2( 0, 0);

    public float MoveAccelerationTime      = 0.0f;
    public float MoveBrakingTime      = 0.0f;
    public float swipeSpeedValue = 0.0f;
    public float MaxWallSlideSpeed     = 2.0f;
    public Vector2 MinWallJumpForce    = new Vector2( 100, 400);
    public Vector2 MaxWallJumpForce    = new Vector2( 100, 400);
    public float WallClimbSpeed        = 5.0f;
    public float MaxWallClimbSpeed     = 2.0f;
    public float FallOffWallFactor     = 0.1f;
    public float JumpAccelerationSpeed = 0;
    public float MoveSpeedInAirWallJump        = 5.0f;
    public float maxMoveDistanceInAirWallJump  = 0.0f;
    public float JumpHoldTimeDelay     = 0;
    public float MaxStamina = 2000;
    public void ResetStamina(){
        stamina = MaxStamina;
    }
    public float stamina = 2000;

}
