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

    public static Vector2 PlayerWallJumpForce = new Vector2( 100, 400);

    public static Direction ReverseDirection( Direction curr ){
        return (curr == Direction.Left) ? Direction.Right : Direction.Left;
    }

    public static bool isSpecialKeyHold(){
        return Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
    }

    public static bool isMoveLeftKeyHold(){
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    public static bool isMoveRightKeyHold(){
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public static bool isJumpKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.Space );
    }

}
