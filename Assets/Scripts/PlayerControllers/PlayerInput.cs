using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerInput
{
    public static bool isSpecialKeyHold(){
        return Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
    }

    public static bool isMoveLeftKeyHold(){
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (Input.GetAxis("Horizontal" ) == -1);
    }

    public static bool isMoveRightKeyHold(){
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (Input.GetAxis("Horizontal" ) == 1);
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

    public static bool isAttack3KeyPressed(){
        return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q);
    }

    public static bool isAttack2KeyPressed(){
        return Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire2");
    }

    public static bool isClimbKeyHold(){
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }

    public static bool isJumpKeyHold(){
        return Input.GetButton( "Jump" );
    }

    public static bool isJumpKeyJustPressed(){
        return Input.GetButtonDown( "Jump" );
    }

    public static bool isActionKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.F );
    }

    public static bool isBlockKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.R );
    }

    public static bool isActionKeyHold(){
        return Input.GetKey( KeyCode.F );
    }

    public static bool isChangeFormKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.E) || Input.GetKeyDown(KeyCode.V);
    }
}
