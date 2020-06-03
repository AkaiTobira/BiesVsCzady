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

    public static bool isFallKeyPressed(){
        return Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
    }

    public static bool isClimbKeyPressed(){
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
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

    public static bool isClimbKeyHold(){
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }

    public static bool isJumpKeyHold(){
        return Input.GetKey( KeyCode.Space );
    }

    public static bool isJumpKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.Space );
    }

    public static bool isChangeFormKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.E) || Input.GetKeyDown(KeyCode.V);
    }
}
