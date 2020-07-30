using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugDrawHelper
{


    public static readonly Color DEFAULT_RED = new  Color(1,0,0);
    public static readonly float X_RAY_LENGHT = 5f;

    public static void DrawX( Vector3 targetPosition){

        Debug.DrawLine( 
                targetPosition + new Vector3(  X_RAY_LENGHT,  X_RAY_LENGHT),
                targetPosition + new Vector3( -X_RAY_LENGHT, -X_RAY_LENGHT),
                DEFAULT_RED
            );

            Debug.DrawLine( 
                targetPosition + new Vector3(  X_RAY_LENGHT, -X_RAY_LENGHT),
                targetPosition + new Vector3( -X_RAY_LENGHT, X_RAY_LENGHT),
                DEFAULT_RED
            );
    }


    public static void DrawX( Vector2 targetPosition){

        Debug.DrawLine( 
                targetPosition + new Vector2(  X_RAY_LENGHT,  X_RAY_LENGHT),
                targetPosition + new Vector2( -X_RAY_LENGHT, -X_RAY_LENGHT),
                DEFAULT_RED
            );

            Debug.DrawLine( 
                targetPosition + new Vector2(  X_RAY_LENGHT, -X_RAY_LENGHT),
                targetPosition + new Vector2( -X_RAY_LENGHT, X_RAY_LENGHT),
                DEFAULT_RED
            );
    }

    public static void DrawX( Vector3 targetPosition, Color c ){

        Debug.DrawLine( 
                targetPosition + new Vector3(  X_RAY_LENGHT,  X_RAY_LENGHT),
                targetPosition + new Vector3( -X_RAY_LENGHT, -X_RAY_LENGHT),
                c
            );

            Debug.DrawLine( 
                targetPosition + new Vector3(  X_RAY_LENGHT, -X_RAY_LENGHT),
                targetPosition + new Vector3( -X_RAY_LENGHT, X_RAY_LENGHT),
                c
            );
    }

    public static void DrawX( Vector2 targetPosition, Color c ){

        Debug.DrawLine( 
                targetPosition + new Vector2(  X_RAY_LENGHT,  X_RAY_LENGHT),
                targetPosition + new Vector2( -X_RAY_LENGHT, -X_RAY_LENGHT),
                c
            );

            Debug.DrawLine( 
                targetPosition + new Vector2(  X_RAY_LENGHT, -X_RAY_LENGHT),
                targetPosition + new Vector2( -X_RAY_LENGHT, X_RAY_LENGHT),
                c
            );
    }

}
