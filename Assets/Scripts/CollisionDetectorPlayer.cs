using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorPlayer : CollisionDetector
{
    [SerializeField] private LayerMask m_oneWayFloorMask   = 0;
    private bool oneWayPlatformBelow;
    private bool closeToWall;


    private Transform pullableObject    = null;

    private Transform destroyableObject = null;
    private bool isObjectPullable;
    private bool isObjectDestroyable;

    [SerializeField] private float FallByFloorTime = 1.0f;
    private float disableFallByOneWayFloorTimer = 0.0f;
    [SerializeField] private float wallCheckRayLenght = 0.3f;

    public void enableFallForOneWayFloor(){
        disableFallByOneWayFloorTimer = 0.0f;
    }

    public bool canFallByFloor(){
        return oneWayPlatformBelow;
    }

    public Transform GetPullableObject(){
        return pullableObject;
    }

    override protected void ResetCollisionInfo(){
        collisionInfo.Reset();
        oneWayPlatformBelow = false;
        closeToWall         = false;
    }

    public void DetectWall( ){
        ProcessCollisionWallClose();
    }

    override protected void ProcessCollision(){
        ProcessSlopeDetection( Mathf.Sign(transition.x) );
        DescendSlope();
        ProcessCollisionHorizontal( Mathf.Sign(transition.x));
        ProcessCollisionVertical(   Mathf.Sign(transition.y));
        ProcessOneWayPlatformDetection( Mathf.Sign(transition.y) );
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
                HandleDestroyable(hit.collider);
                HandleMoveable(hit.collider);
            }else{
                closeToWall      = false || closeToWall;
                if( !closeToWall ){
                    isObjectPullable  = false;
                    pullableObject    = null;
                    destroyableObject = null;
                }
            }

            Debug.DrawRay(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0) * rayLenght,
                new Color(1,1,1)
             );

        }
    }

    private void HandleDestroyable( Collider2D obj ){
        isObjectDestroyable = obj.tag == "Destroyable";
        if( isObjectDestroyable){
            destroyableObject = obj.transform;
        }else{
            destroyableObject = null;
        }
    }


    private void HandleMoveable( Collider2D obj){
        isObjectPullable    = obj.tag == "Movable";
        if( isObjectPullable ){
            pullableObject = obj.transform;
        }else{
            pullableObject = null;
        }
    }

    public bool IsWallPullable(){
        return isObjectPullable;
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
                                             borders.bottom + skinSize );


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
