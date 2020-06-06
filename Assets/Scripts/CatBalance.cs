using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBalance : MonoBehaviour
{

 [Header("Cat")]
    [SerializeField] float minJumpHeight         = 5.0f;
    [SerializeField] float targetJumpHeight            = 0.0f;
    [SerializeField] public float timeToJumpApex = 0.0f;
    [SerializeField] float moveDistance          = 15.0f;
    [SerializeField] float moveDistanceInAir     = 5.0f;
    [SerializeField] float maxMoveDistanceInAir     = 10.0f;
    [SerializeField] float wallClimbSpeed        = 10.0f;
    [SerializeField] float maxWallClimbSpeed     = 10.0f;
    [Range( 0.0f, 1.0f)] public float wallSlideFriction = 1.0f;
    [SerializeField] Vector2 WallJumpFactors = new Vector2(0.0f,0.0f);

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CatUtils.GravityForce         = (2 * targetJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        CatUtils.PlayerJumpForceMin   = Mathf.Sqrt (2 * Mathf.Abs (CatUtils.GravityForce) * minJumpHeight);
        CatUtils.PlayerSpeed          = moveDistance;
        CatUtils.PlayerJumpForceMax   = Mathf.Abs(CatUtils.GravityForce) * timeToJumpApex;
        CatUtils.JumpMaxTime          = timeToJumpApex;
        CatUtils.MoveSpeedInAir       = moveDistanceInAir;
        CatUtils.MaxWallSlideSpeed    = CatUtils.GravityForce * wallSlideFriction; 
        CatUtils.MinWallJumpForce     = new Vector2( WallJumpFactors.x * CatUtils.PlayerSpeed,
                                                     WallJumpFactors.y * CatUtils.PlayerJumpForceMin);

        CatUtils.MaxWallJumpForce     = new Vector2( WallJumpFactors.x * CatUtils.PlayerSpeed  * 1.25f,
                                                     WallJumpFactors.y * CatUtils.PlayerJumpForceMax * 1.25f);

        CatUtils.maxMoveDistanceInAir = maxMoveDistanceInAir;
        CatUtils.MaxWallClimbSpeed    = maxWallClimbSpeed;
        CatUtils.WallClimbSpeed       = wallClimbSpeed;
    }
}
