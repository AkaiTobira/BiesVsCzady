using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetectorMissle : CollisionDetector, ICollisionWallDetector
{

    private bool closeToWall = false;
    [SerializeField] protected float wallCheckRayLenght = 200f;

    public float GetDistanceToClosestWallFront(){
        return 0;
    }
    public float GetDistanceToClosestWallBack(){
        return 0;
    }

    protected override void ProcessCollision(){
        ProcessSlopeDetection( Mathf.Sign(transition.x) );
        DescendSlope();
        ProcessCollisionHorizontal( Mathf.Sign(transition.x));
        ProcessCollisionVertical(   Mathf.Sign(transition.y));
        ProcessCollisionHorizontal( Mathf.Sign(-transition.x));
        ProcessCollisionVertical(   Mathf.Sign(-transition.y));
        ProcessColisionOnTheSameLayer();
        ProcessCollisionWallClose();
    }

    protected void ProcessCollisionWallClose(){
        float rayLenght = wallCheckRayLenght;

        for( float i = -1.0f; i < 2.0f; i++){
            Vector2 rayOrigin = new Vector2( (collisionInfo.faceDir == DIR_LEFT) ? 
                                             borders.left + skinSize: borders.right -skinSize ,
                                              borders.bottom + ((horizontalRayNumber+i)/2.0f) * 
                                                                horizontalDistanceBeetweenRays );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0),
                rayLenght,
                m_collsionMask
            );

            if( hit ){
                closeToWall = true;
            }

            Debug.DrawRay(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0) * rayLenght,
                new Color(1,1,1)
             );

        }
    }

    public bool isWallClose(){
        return closeToWall;
    }
    public bool isCollideWithLeftWall(){
        return false;
    }
    public bool isCollideWithRightWall(){
        return false;
    }

}
