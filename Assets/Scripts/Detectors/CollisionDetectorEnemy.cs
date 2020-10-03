using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorEnemy : CollisionDetector, IPlatformEdgeDetector, ICollisionWallDetector, IFieldSightDetector
{

    [SerializeField] float ledgeRayLenght  = 20;
        
    [SerializeField] private float wallCheckRayLenght = 75f;

    bool closeToWall = true;

    public bool canClimbLedgeFromUpSite(){
        return false;
    }

    override protected void ResetCollisionInfo(){
        base.ResetCollisionInfo();
        closeToWall = false;
    }

    override protected void ProcessCollision(){
        base.ProcessCollision();
        ProcessCollisionWallClose();
    }

    public bool hasReachedPlatformEdge( ){

        float direction = collisionInfo.faceDir;

        Vector2 rayOrigin = new Vector2( (direction == DIR_LEFT) ? 
                                          borders.left : borders.right,
                                          borders.bottom - skinSize);

        RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                Vector2.down,
                ledgeRayLenght,
                m_collsionMask
            );

        Debug.DrawRay(
                rayOrigin,
                Vector2.down * ledgeRayLenght,
                new Color(1,1,1)
        );

        rayOrigin = new Vector2( (direction == DIR_LEFT) ? 
                                          borders.left  + horizontalDistanceBeetweenRays : 
                                          borders.right - horizontalDistanceBeetweenRays,
                                          borders.bottom - skinSize);

        RaycastHit2D hit2 = Physics2D.Raycast(
                rayOrigin,
                Vector2.down,
                ledgeRayLenght,
                m_collsionMask
            );

        Debug.DrawRay(
                rayOrigin,
                Vector2.down * ledgeRayLenght,
                new Color(1,1,1)
        );

        return ( !hit && hit2 );
    }

    public float GetDistanceToClosestWallFront(){

        Vector2 rayOrigin = new Vector2( (collisionInfo.faceDir == DIR_LEFT) ? 
                                            borders.left + skinSize: borders.right -skinSize ,
                                            (borders.top + borders.bottom)/2.0f  );

        RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0),
                Mathf.Infinity,
                m_collsionMask
            );

        return hit.distance - skinSize;
    }

    public float GetDistanceToClosestWallBack(){

        Vector2 rayOrigin = new Vector2( (collisionInfo.faceDir == DIR_RIGHT) ? 
                                            borders.left + skinSize: borders.right -skinSize ,
                                            (borders.top + borders.bottom)/2.0f  );

        RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( -collisionInfo.faceDir, 0),
                Mathf.Infinity,
                m_collsionMask
            );

        return hit.distance - skinSize;
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
            //    HandleDestroyable(hit.collider);
           //     HandleMoveable(hit.collider);
         //   }else{
        //        closeToWall      = false || closeToWall;
            //    if( !closeToWall ){
            //        isObjectPullable  = false;
            //        pullableObject    = null;
           //         destroyableObject = null;
           //     }
         //   }
            

            Debug.DrawRay(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0) * rayLenght,
                Color.blue
             );

        }
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

    public LayerMask playerHitBoxLayer = 0;
    public float sightLenght     = 500;

    public bool isPlayerSeen(){

//        Debug.Log( "IsLeft=" + (collisionInfo.faceDir == DIR_LEFT).ToString() );

        Vector2 rayOrigin = new Vector2( (collisionInfo.faceDir == DIR_LEFT) ? 
                                            borders.left - skinSize: borders.right + skinSize ,
                                            (borders.top + borders.bottom)/2.0f  );

        RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0),
                sightLenght,
                playerHitBoxLayer
            );

        Debug.DrawRay(
                rayOrigin,
                new Vector2( collisionInfo.faceDir, 0) * sightLenght,
                new Color(1,1,1)
             );

        base.GetSlopeAngle();

  //      Debug.Log( "isHit=" + (!hit).ToString()  );

        if( !hit ) return false;

//        Debug.Log( "isRightTarget=" + ( hit.collider.tag == "PlayerHurtBox").ToString());
        return hit.collider.tag == "PlayerHurtBox";
    }


}
