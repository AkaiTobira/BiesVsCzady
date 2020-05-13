using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorPlayer : CollisionDetector
{
    [SerializeField] private LayerMask m_ladderMask        = 0;
    [SerializeField] private LayerMask m_oneWayFloorMask   = 0;
    private bool ladderDetected;
    private bool oneWayPlatformBelow;

    [SerializeField] private float FallByFloorTime = 1.0f;
    private float disableFallByOneWayFloorTimer = 0.0f;

    public void enableFallForOneWayFloor(){
        disableFallByOneWayFloorTimer = 0.0f;
    }

    public bool canFallByFloor(){
        return oneWayPlatformBelow;
    }

    public bool canClimbOnLadder(){
        return ladderDetected;
    }

    override protected void ResetCollisionInfo(){
        collisionInfo.Reset();
        ladderDetected      = false;
        oneWayPlatformBelow = false;
    }

    override protected void ProcessCollision(){
        ProcessCollisionVertical( Mathf.Sign(transition.y));
        ProcessCollisionHorizontal( Mathf.Sign(transition.x));
        ProcessOneWayPlatformDetection( Mathf.Sign(transition.y) );
        ProcessLadderDetection();
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



    public bool isOnCelling(){
        return collisionInfo.above;
    }
    public bool isOnGround(){
        return collisionInfo.below;
    }

    private void ProcessLadderDetection(){
        for( int i = 0; i < verticalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( borders.left + i * verticalDistanceBeetweenRays, 
                                             borders.bottom );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( 0, 1),
                Time.deltaTime,
                m_ladderMask
            );

            if( hit ){
                ladderDetected = true;
            }
            
            Debug.DrawRay(
                rayOrigin,
                new Vector2( 0, 1),
                new Color(0,0,1)
             );
        }
    }

}
