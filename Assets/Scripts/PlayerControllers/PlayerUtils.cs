using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerUtils
{
    public enum Direction {
        Left = -1, 
        Right = 1
    }

    public const float PlayerSpeed = 20.0f;
    public const float JumpMaxTime     = 0.25f;
    public const float PlayerJumpForce = 50.0f;

    public const float PlayerSpeedInAir = 20.0f;

    public static Direction ReverseDirection( Direction curr ){
        return (curr == Direction.Left) ? Direction.Right : Direction.Left;
    }

}
