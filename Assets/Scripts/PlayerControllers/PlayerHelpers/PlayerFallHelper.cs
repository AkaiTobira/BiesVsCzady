using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerFallHelper
{
    public static void IncrementCounters(){
        fallAfterLeaveTheGorundFrames    = Mathf.Min( MAX_FALL_AFTER_LEAVE_PLATFORM + 1,
                                                      fallAfterLeaveTheGorundFrames + 1 );
    }

    public static bool FallRequirementsMeet( bool isOnGound ){
        if( isOnGound ){ fallAfterLeaveTheGorundFrames = 0; }
        return isFitInGroundLeaveDelay();
    }

    static int fallAfterLeaveTheGorundFrames = 0;
    const int MAX_FALL_AFTER_LEAVE_PLATFORM  = 15;
    private static bool isFitInGroundLeaveDelay( ){
        return ( (fallAfterLeaveTheGorundFrames > MAX_FALL_AFTER_LEAVE_PLATFORM)  );
    }

}
