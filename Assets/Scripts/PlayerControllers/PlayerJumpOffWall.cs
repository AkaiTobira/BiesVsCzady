﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerJumpOffWall
{

    private static bool moveDirectionPressed = false;
    private static bool jumpDirectionPressed = false;
    static int jumpOffTheWallFrames = 0;
    const int MAX_JUMP_OFF_FRAMES_LIMIT  = 7;


    public static void ResetCounters(){
        jumpOffTheWallFrames = 0;
    }

    public static void IncrementCounters(){


        jumpOffTheWallFrames    = Mathf.Min( MAX_JUMP_OFF_FRAMES_LIMIT + 1,
                                             jumpOffTheWallFrames + 1 );
        if( jumpOffTheWallFrames == MAX_JUMP_OFF_FRAMES_LIMIT){
            moveDirectionPressed = false;
            jumpDirectionPressed = false;
        }

    }

    public static bool FallOffWallRequirementsMeet(){
        if( !moveDirectionPressed ){
            moveDirectionPressed = PlayerUtils.isMoveLeftKeyHold() || 
                                   PlayerUtils.isMoveRightKeyHold(); 
            ResetCounters();
        }
        Debug.Log(  ( (MAX_JUMP_OFF_FRAMES_LIMIT > jumpOffTheWallFrames) && moveDirectionPressed));
        return( (MAX_JUMP_OFF_FRAMES_LIMIT > jumpOffTheWallFrames) && moveDirectionPressed);
    }

    public static bool JumpOffWallRequirementsMeet( ){
        if( !moveDirectionPressed ) 
            moveDirectionPressed = PlayerUtils.isMoveLeftKeyHold() || 
                                   PlayerUtils.isMoveRightKeyHold(); 
        if( !jumpDirectionPressed )
            jumpDirectionPressed = PlayerUtils.isJumpKeyJustPressed();

        return( (jumpOffTheWallFrames < MAX_JUMP_OFF_FRAMES_LIMIT) && moveDirectionPressed && jumpDirectionPressed);
    }

//    private static bool isFitInGroundLeaveDelay( ){
//        return ( (jumpOffTheWallFrames > MAX_JUMP_OFF_FRAMES_LIMIT)  );
//    }

}
