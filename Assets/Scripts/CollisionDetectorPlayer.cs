using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorPlayer : CollisionDetector
{
    [SerializeField] private LayerMask m_oneWayFloorMask   = 0;
    private bool oneWayPlatformBelow;
    private bool closeToWall;

    [SerializeField] private float FallByFloorTime = 1.0f;
    private float disableFallByOneWayFloorTimer = 0.0f;
    [SerializeField] private float wallCheckRayLenght = 0.3f;

    public void enableFallForOneWayFloor(){
        disableFallByOneWayFloorTimer = 0.0f;
    }

    public bool canFallByFloor(){
        return oneWayPlatformBelow;
    }


    override protected void ResetCollisionInfo(){
        collisionInfo.Reset();
        oneWayPlatformBelow = false;
    }

    override protected void ProcessCollision(){
        ProcessCollisionHorizontal( Mathf.Sign(transition.x));
        ProcessCollisionVertical(   Mathf.Sign(transition.y));
        ProcessOneWayPlatformDetection( Mathf.Sign(transition.y) );
        ProcessCollisionWallClose();
    }

    protected void ProcessCollisionWallClose(){
        float rayLenght = wallCheckRayLenght;

        Vector2 rayOrigin = new Vector2( (collisionInfo.faceDir == DIR_LEFT) ? borders.left : 
                                                                               borders.right,
                                          borders.bottom + (horizontalRayNumber/2.0f) * 
                                                            horizontalDistanceBeetweenRays );

        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            new Vector2( collisionInfo.faceDir, 0),
            rayLenght,
            m_collsionMask
        );

        if( hit ){
            closeToWall = true;
        }else{
            closeToWall = false;
        }
        
        Debug.DrawRay(
            rayOrigin,
            new Vector2( collisionInfo.faceDir, 0) * rayLenght,
            new Color(1,1,1)
         );
    }


    protected void ProcessOneWayPlatformDetection( float directionY ){
        if( disableFallByOneWayFloorTimer < FallByFloorTime ){
            disableFallByOneWayFloorTimer += Time.deltaTime;
            return;
        }
        if( directionY == DIR_UP ) return;
        
        float rayLenght  = Mathf.Abs (transition.y) + skinSize;

        for( int i = 0; i < verticalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( borders.left + i * verticalDistanceBeetweenRays, 
                                             borders.bottom );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( 0, directionY),
                rayLenght,
                m_oneWayFloorMask
            );

            if( hit ){
                rayLenght  = hit.distance;
                oneWayPlatformBelow = true;
                collisionInfo.below = true;
            }
            
            Debug.DrawRay(
                rayOrigin,
                new Vector2( 0, directionY) * rayLenght,
                new Color(0,1,0)
             );
        }
        transition.y = Mathf.Sign(transition.y) * ( rayLenght -skinSize );
    }


    public bool isWallClose(){
        return closeToWall;
    }

    public bool isCollideWithLeftWall(){
        return collisionInfo.left;
    }

    public bool isCollideWithRightWall(){
        return collisionInfo.right;
    }

    public bool isOnCelling(){
        return collisionInfo.above;
    }
    public bool isOnGround(){
        return collisionInfo.below;
    }
}
