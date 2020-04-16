using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerUtils
{
    public enum Direction {
        Left = -1, 
        Right = 1
    }

    public static float PlayerSpeed = 15.0f;
    public static float JumpMaxTime     = 0.25f;
    public static float PlayerJumpForce = 15.0f;


}
