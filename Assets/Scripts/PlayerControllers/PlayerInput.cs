using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerInput
{
    public static bool isSpecialKeyHold(){
        return Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
    }

    public static bool isMoveLeftKeyHold(){
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    public static bool isMoveRightKeyHold(){
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public static bool isFallKeyHold(){
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    public static bool isAttack1KeyPressed(){
        return Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Fire1");
    }

    public static bool isAttack2KeyPressed(){
        return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q);
    }

    public static bool isAttack3KeyPressed(){
        return Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire2");
    }

    public static bool isJumpKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.Space );
    }
}
