using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class  GlobalUtils
{
    public enum Direction {
        Left = -1, 
        Right = 1
    }

    public struct AttackInfo{
        public bool isValid;

        public float stunDuration;
        public bool lockFaceDirectionDuringKnockback;
        public Vector2 knockBackValue;
        public Direction fromCameAttack;
        public float attackDamage;
        public float knockBackFrictionX;
        public string stateName;
    };

    public static Transform   PlayerObject   = null;
    public static CameraShake cameraShake    = null;

    public static Transform    TaskMaster = null;

    public static Camera_Follow Camera = null;

    public static Text        debugConsole   = null;
    public static Text        debugConsole2  = null;

    public static Direction ReverseDirection( Direction curr ){
        return (curr == Direction.Left) ? Direction.Right : Direction.Left;
    }
}
