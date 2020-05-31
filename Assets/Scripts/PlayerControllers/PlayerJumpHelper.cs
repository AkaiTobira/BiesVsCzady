using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerJumpHelper
{
    public static void IncrementCounters(){
        jumpAfterLeaveTheGorundFrames    = Mathf.Min( MAX_JUMP_AFTER_LEAVE_PLATFORM, 
                                                      jumpAfterLeaveTheGorundFrames + 1);
        jumpAfterPlayerHitKeyDelayFrames = Mathf.Min( MAX_HIT_KEY_DELAY, 
                                                      jumpAfterPlayerHitKeyDelayFrames + 1);
    }

    public static bool JumpRequirementsMeet( bool isKeyPressed, bool isOnGound ){

        if( isKeyPressed ) jumpAfterPlayerHitKeyDelayFrames = 0;

        if( !isOnGound ){ jumpAfterLeaveTheGorundFrames = 0; }
        else{             jumpAfterLeaveTheGorundFrames = MAX_JUMP_AFTER_LEAVE_PLATFORM; }

        return isFitInGroundLeaveDelay(isKeyPressed) ||
               isFitInHitKeyDelay(isOnGound); 
    }

    static int jumpAfterPlayerHitKeyDelayFrames = 10;
    const int MAX_HIT_KEY_DELAY                 = 10;
    private static bool isFitInHitKeyDelay( bool isOnGound ){
        return ( (jumpAfterPlayerHitKeyDelayFrames < MAX_HIT_KEY_DELAY) && isOnGound );
    }

    static int jumpAfterLeaveTheGorundFrames = 10;
    const int MAX_JUMP_AFTER_LEAVE_PLATFORM  = 10;
    private static bool isFitInGroundLeaveDelay( bool isKeyPressed ){
        return (jumpAfterLeaveTheGorundFrames < MAX_JUMP_AFTER_LEAVE_PLATFORM && isKeyPressed);
    }

}
