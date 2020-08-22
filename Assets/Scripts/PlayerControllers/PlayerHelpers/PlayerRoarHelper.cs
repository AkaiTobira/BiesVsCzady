using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerRoarHelper
{
    static float coldownTimer = 0;
    public static float ROAR_COLDOWN = 2.0f;

    public static void IncrementCounters(){
        coldownTimer    = Mathf.Max( 0.0f,
                          coldownTimer - Time.deltaTime );
    }

    public static bool RoarRequirementsMeet(){
        return coldownTimer == 0;
    }

    public static void RoarUsed(){
        coldownTimer = ROAR_COLDOWN;
    }

}
