using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorMovable : CollisionDetector, ICollisionWallDetector
{
    [SerializeField] public float PullFriction = 0;
    [SerializeField] public float PushFriction = 0;

    [SerializeField] public float GravityForce = 0;
    [SerializeField] public float MaxGravityForce = 0;
    
    private float accumulatedGravity = 0.0f;


    public float GetDistanceToClosestWallFront(){
        return 0;
    }
    public float GetDistanceToClosestWallBack(){
        return 0;
    }
    public bool isWallClose(){
        return false;
    }
    public bool isCollideWithLeftWall(){
        return collisionInfo.left;
    }
    public bool isCollideWithRightWall(){
        return collisionInfo.right;
    }

    public bool canBePushedInDirection( GlobalUtils.Direction m_dir){
        if( m_dir == GlobalUtils.Direction.Right ) return !collisionInfo.right;
        if( m_dir == GlobalUtils.Direction.Left  ) return !collisionInfo.left;
        return false;
    }


    protected override void ProcessAutoGravity(){
        if( autoGravityOn ){
            if( !collisionInfo.below){
                if( accumulatedGravity < MaxGravityForce){
                    accumulatedGravity += GravityForce;
                }
                transition.y = -accumulatedGravity * Time.deltaTime;
                Move( transition );
            }else{
                Move( new Vector2(0,-0.1f));
                if(accumulatedGravity != 0)
                {
                    accumulatedGravity = 0.0f;
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enviro/stone fall");
                }

            }
        }        
    }

    public override void Move(float x, float y)
    {
        base.Move(x, y);
    }

}

