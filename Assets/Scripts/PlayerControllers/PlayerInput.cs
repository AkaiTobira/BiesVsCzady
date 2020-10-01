using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerInput
{
    public static bool isSpecialKeyHold(){
        return Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Shift");
    }

    public static bool isMoveLeftKeyHold(){
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (Input.GetAxis("Horizontal" ) == -1);
    }

    public static bool isMoveRightKeyHold(){
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (Input.GetAxis("Horizontal" ) == 1);
    }

    public static bool isFallKeyHold(){
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (Input.GetAxis("Vertical") == -1);
    }

    public static bool isFallKeyPressed(){
        return Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("Vertical") == -1);
    }

    public static bool isClimbKeyPressed(){
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("Vertical") == 1);
    }

    public static bool isAttack1KeyPressed(){
        return Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Strike")  || Input.GetButtonDown("Fire1");
    }

    public static bool isAttack3KeyPressed(){
        return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q);
    }

    public static bool isAttack2KeyPressed(){
        return ( Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Roar")  || Input.GetButtonDown("Fire2"))
                && PlayerRoarHelper.RoarRequirementsMeet() ;
    }

    public static bool isClimbKeyHold(){
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || (Input.GetAxis("Vertical") == 1);
    }

    public static bool isJumpKeyHold(){
        return Input.GetButton( "Jump" );
    }

    public static bool isJumpKeyJustPressed(){
        return Input.GetButtonDown( "Jump" );
    }

    public static bool isActionKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.F) || Input.GetButtonDown("Action");
    }

    public static bool isBlockKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.R ) || Input.GetKeyDown( KeyCode.LeftControl ) || Input.GetKeyDown( KeyCode.RightControl) || (Input.GetAxis("Block") == 1);
    }

    public static bool isBlockKeyPressed(){
        return Input.GetKey( KeyCode.R ) || Input.GetKey( KeyCode.LeftControl ) || Input.GetKey( KeyCode.RightControl) || (Input.GetAxis("Block") == 1);
    }

    public static bool isActionKeyHold(){
        return Input.GetKey( KeyCode.F) || Input.GetButton("Action");
    }

    public static bool isChangeFormKeyJustPressed(){
        return Input.GetKeyDown( KeyCode.E) || Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Transform");
    }
}
