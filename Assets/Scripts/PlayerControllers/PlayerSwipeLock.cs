using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerSwipeLock
{

    public static void ResetCounter(){
         swipeLockTime = 0.0f;
    }

    public static void IncrementCounters(){
        swipeLockTime += Time.deltaTime;
    }

    public static bool SwipeUnlockRequirementsMeet( ){
        return isSwipeUnlocked();
    }

    static float swipeLockTime = 0.0f;
    const float MAX_SWIPE_LOCK_TIME  = 0.5f;
    private static bool isSwipeUnlocked( ){
        return ( (swipeLockTime > MAX_SWIPE_LOCK_TIME)  );
    }

}
