using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerFallOfWallHelper
{


    public static void ResetCounter(){
        fallOfWallTime = 0.0f;
    }


    public static void IncrementCounters(){
        fallOfWallTime += Time.deltaTime;
    }

    public static bool FallOfWallRequirementsMeet( ){
        if( PlayerInput.isSpecialKeyHold() ){ fallOfWallTime = 0; }
        return shouldFallOff();
    }

    static float fallOfWallTime = 0.0f;
    const float MAX_FALL_OFF_TIME  = 2.0f;
    private static bool shouldFallOff( ){
        return ( (fallOfWallTime > MAX_FALL_OFF_TIME)  );
    }

}
