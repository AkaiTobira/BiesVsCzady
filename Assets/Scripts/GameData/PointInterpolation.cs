using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointInterpolation 
{
    public struct PointPack{
        public Vector3 startPoint;
        public Vector3 endPoint;

        public Vector3 controlPoint1;
        public Vector3 controlPoint2;

        public PointPack( Vector3 pointS, Vector3 point1, Vector3 point2, Vector3 pointE){
            startPoint    = pointS;
            endPoint      = pointE;
            controlPoint1 = point1;
            controlPoint2 = point2;
        }
    };
    
    //The De Casteljau's Algorithm
    public static Vector3 DeCasteljausAlgorithm(float t, PointPack infoPack)
    {
        //Linear interpolation = lerp = (1 - t) * A + t * B
        //Could use Vector3.Lerp(A, B, t)

        //To make it faster
        float oneMinusT = 1f - t;
        
        //Layer 1
        Vector3 Q = oneMinusT * infoPack.startPoint    + t * infoPack.controlPoint1;
        Vector3 R = oneMinusT * infoPack.controlPoint1 + t * infoPack.controlPoint2;
        Vector3 S = oneMinusT * infoPack.controlPoint2 + t * infoPack.endPoint;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }
}
