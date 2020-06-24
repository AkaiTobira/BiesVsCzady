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
    [SerializeField] float moveDistanceInAirWallJump     = 5.0f;
    [SerializeField] float maxMoveDistanceInAirWallJump     = 10.0f;
    [SerializeField] float wallClimbSpeed        = 10.0f;
    [SerializeField] float maxWallClimbSpeed     = 10.0f;
    [Range( 0.0f, 1.0f)] public float wallSlideFriction = 1.0f;
    [SerializeField] Vector2 WallJumpFactors = new Vector2(0.0f,0.0f);

    [Range( 0.0001f, 10.0f)] public float MoveAccelerationTime      = 0.0f;
    [Range( 0.0001f, 10.0f)] public float MoveBrakingTime      = 0.0f;


    void Start()
    {
        LoadBalance("CatValues");
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
        CatUtils.MoveSpeedInAirWallJump        = moveDistanceInAirWallJump;
        CatUtils.maxMoveDistanceInAirWallJump  = maxMoveDistanceInAirWallJump;
        CatUtils.MoveAccelerationTime     = MoveAccelerationTime;
        CatUtils.MoveBrakingTime          = MoveBrakingTime;


    }

    public void SaveBalance(){
        JsonLoader.CatValues newCatValues = new JsonLoader.CatValues();

        newCatValues.minJumpHeight        = minJumpHeight;
        newCatValues.targetJumpHeight     = targetJumpHeight;
        newCatValues.timeToJumpApex       = timeToJumpApex;
        newCatValues.moveDistance         = moveDistance;
        newCatValues.moveDistanceInAir    = moveDistanceInAir;
        newCatValues.maxMoveDistanceInAir = maxMoveDistanceInAir;
        newCatValues.wallClimbSpeed       = wallClimbSpeed;
        newCatValues.maxWallClimbSpeed    = maxWallClimbSpeed;
        newCatValues.wallSlideFriction    = wallSlideFriction;
        newCatValues.WallJumpFactorsX     = WallJumpFactors.x;
        newCatValues.WallJumpFactorsY     = WallJumpFactors.y;

        newCatValues.moveDistanceInAirWallJump    = moveDistanceInAirWallJump;
        newCatValues.maxMoveDistanceInAirWallJump = maxMoveDistanceInAirWallJump;

        newCatValues.moveAccelerationTime = MoveAccelerationTime;
        newCatValues.moveBrakingTime      = MoveBrakingTime;

        string catValues = JsonUtility.ToJson(newCatValues);
        System.IO.File.WriteAllText( Application.dataPath +  "/Resources/Temp/CatValues.json", catValues);
    }

    public void LoadBalance( string path){
        var jsonFile = System.IO.File.ReadAllText( Application.dataPath +  "/Resources/" + path + ".json" );
        var newCatValues = JsonUtility.FromJson<JsonLoader.CatValues>(jsonFile);

        minJumpHeight        = newCatValues.minJumpHeight;
        targetJumpHeight     = newCatValues.targetJumpHeight;
        timeToJumpApex       = newCatValues.timeToJumpApex;
        moveDistance         = newCatValues.moveDistance;
        moveDistanceInAir    = newCatValues.moveDistanceInAir;
        maxMoveDistanceInAir = newCatValues.maxMoveDistanceInAir;
        wallClimbSpeed       = newCatValues.wallClimbSpeed;
        maxWallClimbSpeed    = newCatValues.maxWallClimbSpeed;
        wallSlideFriction    = newCatValues.wallSlideFriction;
        WallJumpFactors      = new Vector2( newCatValues.WallJumpFactorsX, newCatValues.WallJumpFactorsY );
        moveDistanceInAirWallJump        = newCatValues.moveDistanceInAirWallJump;
        maxMoveDistanceInAirWallJump  = newCatValues.maxMoveDistanceInAirWallJump;

        MoveAccelerationTime = newCatValues.moveAccelerationTime;
        MoveBrakingTime      = newCatValues.moveBrakingTime;

    }

    public void LockCurrentTemp(){
        var jsonFile = System.IO.File.ReadAllText( Application.dataPath +  "/Resources/Temp/CatValues.json" );
        var newCatValues = JsonUtility.FromJson<JsonLoader.CatValues>(jsonFile);
        string catValues = JsonUtility.ToJson(newCatValues);
        System.IO.File.WriteAllText( Application.dataPath +  "/Resources/CatValues.json", catValues);
    }

}
