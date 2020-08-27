using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerUtils
{

    public static float PlayerSpeed           = 15.0f;
    public static float JumpMaxTime           = 0.25f;
    public static float PlayerJumpForceMax       = 20.0f;
    public static float PlayerJumpForceMin       = 0.0f;
    public static float GravityForce          = 15.0f;
    public static float MoveSpeedInAir      = 5.0f;
    public static float MaxWallSlideSpeed     = 2.0f;

    public static float WallClimbSpeed      = 5.0f;
    public static float MaxWallClimbSpeed     = 2.0f;
    public static float FallOffWallFactor     = 0.1f;

    public static float JumpAccelerationSpeed = 0;

    public static float maxMoveDistanceInAir   = 0.0f;

    public static float JumpHoldTimeDelay = 0;
    public static Vector2 PlayerWallJumpForce = new Vector2( 100, 400);

    public static float Attack1Damage = 2;
    public static float Attack2Damage = 0;
    public static float Attack3Damage = 5;

    public static Vector2 KnockBackValueAttack1 =  new Vector2( 100, 1000);
    public static Vector2 KnockBackValueAttack2 =  new Vector2( 100, 400);
    public static Vector2 KnockBackValueAttack3 =  new Vector2( 100, 400);

    //Its not related to up ones
    public static float swipeSpeedValue = 0.0f;


    public static void ResetStamina(){
        stamina = 70;
    }

    public static float stamina = 70;
}
