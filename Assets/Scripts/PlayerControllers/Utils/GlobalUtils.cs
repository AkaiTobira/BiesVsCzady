using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class  GlobalUtils
{

[System.Serializable]
    public enum Types{
        Cat,
        Bies,
        Both
    }
    [System.Serializable]
    public class DialogueInfo{

        public DialogueInfo(){}
        public DialogueInfo(Types key, string value){
            type = key;
            text = value;
        }
        public Types type;
        public string text;

    }

    public enum Direction {
        Left = -1, 
        Right = 1
    }

    public struct AttackInfo{
        public bool isValid;

        public float stunDuration;
        public bool lockFaceDirectionDuringKnockback;

    //    public bool isKnockbackValid;
        public Vector2 knockBackValue;
        public Direction fromCameAttack;
        public float attackDamage;
        public float knockBackFrictionX;
        public string stateName;
    };

    public static Transform   PlayerObject   = null;
    public static CameraShake cameraShake    = null;

    public static TaskMaster    TaskMaster = null;

    public static Camera_Follow Camera = null;

    public static Text TutorialConsole = null;

    public static GUIController GUIOverlay = null;

    public static DialogueBoxesController DialogueSystem = null;

    public static Text        debugConsole   = null;
    public static Text        debugConsole2  = null;

    public static Direction ReverseDirection( Direction curr ){
        return (curr == Direction.Left) ? Direction.Right : Direction.Left;
    }

    public static  Direction GetClosestSideToPosition( Vector3 axisPoint, Vector3 sidePoint ){
        return axisPoint.x < sidePoint.x ? Direction.Right : Direction.Left;
    }



}
