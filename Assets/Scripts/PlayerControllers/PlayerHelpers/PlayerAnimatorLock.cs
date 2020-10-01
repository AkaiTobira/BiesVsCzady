using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerAnimatorLock
{
    public static void IncrementCounters(){
        lockTime = Mathf.Max( lockTime - Time.deltaTime, 0 );
    }

    public static bool UnlockRequirementsMeen( ){
        if( lockTime > 0 ){ return false; }
        return true;
    }

    public static float lockTime = 0.0f;

}
