using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorMovable : CollisionDetector
{
    [SerializeField] public float PullFriction = 0;
    [SerializeField] public float PushFriction = 0;

    [SerializeField] public float GravityForce = 0;
    [SerializeField] public float MaxGravityForce = 0;
    
    private float accumulatedGravity = 0.0f;


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
                accumulatedGravity = 0.0f;
            }
        }        
    }

}
