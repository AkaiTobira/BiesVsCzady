using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerMoveOfWallHelper
{
    static bool isActive = false;

    public static void DisableCounter(){
        isActive = false;
        fallOfWallTime = 0;
    }

    public static void EnableCounter(){
        ResetCounter();
        isActive = true;
    }

    public static void ResetCounter(){
        if( !isActive ) fallOfWallTime = 0.0f;
    }

    public static void IncrementCounters(){
        if(!isActive) return;
        fallOfWallTime += Time.deltaTime;
        if( fallOfWallTime > MAX_FALL_OFF_TIME + 0.1f) isActive = false;
    }

    public static bool MoveOfWallRequirementsMeet( ){
        if( PlayerInput.isSpecialKeyHold() ){ fallOfWallTime = 0; }
        return shouldFallOff();
    }

    static float fallOfWallTime = 0.0f;
    const float MAX_FALL_OFF_TIME  = 0.2f;
    private static bool shouldFallOff( ){
        return ( (fallOfWallTime > MAX_FALL_OFF_TIME)  );
    }
}
