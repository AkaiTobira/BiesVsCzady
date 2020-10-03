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

    [SerializeField] private float timerOfStoneFall = -2;
    [SerializeField] private float timeToStoneFall    = 0.03f;

    [SerializeField] private bool shakeEnabled = false;

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
                timerOfStoneFall += Time.deltaTime;
                Move( transition );
            }else{
                Move( new Vector2(0,-0.1f));
                if( timerOfStoneFall > timeToStoneFall ){
                    if( !canBePushedInDirection(moveDirection) ) FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enviro/stone fall");
                    if( shakeEnabled) GUIElements.cameraShake.TriggerShake(0.3f);
                }
                timerOfStoneFall = 0;
                if(accumulatedGravity != 0)
                {
                    accumulatedGravity = 0.0f;
                    //
                }

            }
        }        
    }

    private GlobalUtils.Direction moveDirection;

    public override void Move(Vector2 velocity)
    {
        moveDirection = (GlobalUtils.Direction)Mathf.Sign(velocity.x);

        if( velocity.x != 0 ){ timerOfStoneFall = 0;}
    //    if( Mathf.Abs(velocity.y) > 3 ) timerOfStoneFall = timeToStoneFall;
    //    Debug.Log( velocity.y );
        base.Move(velocity);
    }

    public override void Move(float x, float y)
    {
        
        moveDirection = (GlobalUtils.Direction)Mathf.Sign(x);

        if( x != 0 ){ timerOfStoneFall = 0;}
    //    if( Mathf.Abs(y) > 3 ) timerOfStoneFall = timeToStoneFall;
        base.Move(x, y);
    }

}

