using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerUtils
{
    public enum Direction {
        Left = -1, 
        Right = 1
    }

    public static float PlayerSpeed       = 15.0f;
    public static float JumpMaxTime       = 0.25f;
    public static float PlayerJumpForce   = 20.0f;
    public static float GravityForce      = 15.0f;
    public static float PlayerSpeedInAir  = 5.0f;
    public static float MaxWallSlideSpeed  = 2.0f;

    public static Vector2 PlayerWallJumpForce       = new Vector2(  100, 400);
    public static Vector2 MoveDistanceDuringAttack1 = new Vector2(  200,   0);
    public static Vector2 MoveDistanceDuringAttack2 = new Vector2(    0,   0);
    public static Vector2 MoveDistanceDuringAttack3 = new Vector2( 1200,   0);

    public static Direction ReverseDirection( Direction curr ){
        return (curr == Direction.Left) ? Direction.Right : Direction.Left;
    }

}
