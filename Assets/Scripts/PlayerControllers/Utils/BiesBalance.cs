using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesBalance : MonoBehaviour
{
 [Header("Bies")]
    [SerializeField] float minJumpHeight         = 5.0f;
    [SerializeField] float targetJumpHeight      = 0.0f;
    [SerializeField] public float timeToJumpApex = 0.0f;
    [SerializeField] float moveDistance          = 15.0f;
    [SerializeField] float moveDistanceInAir     = 5.0f;
    [SerializeField] float maxMoveDistanceInAir     = 10.0f;

    [SerializeField] float Attack1Damage = 2;
    [SerializeField] Vector2 KnockBackValueAttack1 =  new Vector2( 100, 1000);

    [SerializeField] float Attack2Damage = 5;
    [SerializeField] Vector2 KnockBackValueAttack2 =  new Vector2( 100, 400);

    [SerializeField] float Attack3Damage = 5;
    [SerializeField] Vector2 KnockBackValueAttack3 =  new Vector2( 100, 400);

    [SerializeField] float RoarDamage = 0;
    [SerializeField] Vector2 KnockBackValueRoar =  new Vector2( 0, 0);

    [SerializeField] float RoarStunDuration = 2.0f;

    [Range( 0.0001f, 10.0f)] public float MoveAccelerationTime      = 0.0f;
    [Range( 0.0001f, 10.0f)] public float MoveBrakingTime      = 0.0f;

    BiesUtils.BiesValues infoPack = new BiesUtils.BiesValues();

    void Start()
    {
        LoadBalance("BiesValues");
    }


    void Update()
    {
        BiesUtils.GravityForce          = (2 * targetJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        BiesUtils.PlayerJumpForceMin    = Mathf.Sqrt (2 * Mathf.Abs (BiesUtils.GravityForce) * minJumpHeight);
        BiesUtils.PlayerSpeed           = moveDistance;
        BiesUtils.PlayerJumpForceMax    = Mathf.Abs(BiesUtils.GravityForce) * timeToJumpApex;
        BiesUtils.JumpMaxTime           = timeToJumpApex;
        BiesUtils.MoveSpeedInAir        = moveDistanceInAir;
        BiesUtils.maxMoveDistanceInAir  = maxMoveDistanceInAir;
        BiesUtils.Attack1Damage         = Attack1Damage;
        BiesUtils.Attack2Damage         = Attack2Damage;
        BiesUtils.Attack3Damage         = Attack3Damage;
        BiesUtils.RoarDamage            = RoarDamage;
        BiesUtils.KnockBackValueRoar    = KnockBackValueRoar;
        BiesUtils.KnockBackValueAttack1 = KnockBackValueAttack1;
        BiesUtils.KnockBackValueAttack2 = KnockBackValueAttack2;
        BiesUtils.KnockBackValueAttack3 = KnockBackValueAttack3;
        BiesUtils.MoveAccelerationTime  = MoveAccelerationTime;
        BiesUtils.MoveBrakingTime       = MoveBrakingTime;
        BiesUtils.RoarStunDuration = RoarStunDuration;

        

        infoPack.GravityForce           = BiesUtils.GravityForce         ; 
        infoPack.PlayerJumpForceMin     = BiesUtils.PlayerJumpForceMin   ; 
        infoPack.PlayerSpeed            = BiesUtils.PlayerSpeed          ; 
        infoPack.PlayerJumpForceMax     = BiesUtils.PlayerJumpForceMax   ; 
        infoPack.JumpMaxTime            = BiesUtils.JumpMaxTime          ; 
        infoPack.MoveSpeedInAir         = BiesUtils.MoveSpeedInAir       ; 
        infoPack.maxMoveDistanceInAir   = BiesUtils.maxMoveDistanceInAir ; 
        infoPack.Attack1Damage          = BiesUtils.Attack1Damage        ; 
        infoPack.Attack2Damage          = BiesUtils.Attack2Damage        ; 
        infoPack.Attack3Damage          = BiesUtils.Attack3Damage        ; 
        infoPack.RoarDamage             = BiesUtils.RoarDamage;
        infoPack.KnockBackValueRoar     = BiesUtils.KnockBackValueRoar;
        infoPack.KnockBackValueAttack1  = BiesUtils.KnockBackValueAttack1; 
        infoPack.KnockBackValueAttack2  = BiesUtils.KnockBackValueAttack2; 
        infoPack.KnockBackValueAttack3  = BiesUtils.KnockBackValueAttack3; 
        infoPack.MoveAccelerationTime   = BiesUtils.MoveAccelerationTime ; 
        infoPack.MoveBrakingTime        = BiesUtils.MoveBrakingTime      ; 
        infoPack.RoarStunDuration = BiesUtils.RoarStunDuration;
        BiesUtils.infoPack = infoPack;
    }

    public void SaveBalance(){
        JsonLoader.BiesValues newBiesValues = new JsonLoader.BiesValues();

        newBiesValues.minJumpHeight        = minJumpHeight;
        newBiesValues.targetJumpHeight     = targetJumpHeight;
        newBiesValues.timeToJumpApex       = timeToJumpApex;
        newBiesValues.moveDistance         = moveDistance;
        newBiesValues.moveDistanceInAir    = moveDistanceInAir;
        newBiesValues.maxMoveDistanceInAir = maxMoveDistanceInAir;

        newBiesValues.Attack1Damage = Attack1Damage;
        newBiesValues.Attack2Damage = Attack2Damage;
        newBiesValues.Attack3Damage = Attack3Damage;
        newBiesValues.RoarDamage    = RoarDamage;

        newBiesValues.KnockBackValueRoarX    = KnockBackValueRoar.x;
        newBiesValues.KnockBackValueRoarY    = KnockBackValueRoar.y;
        newBiesValues.KnockBackValueAttack1X = KnockBackValueAttack1.x;
        newBiesValues.KnockBackValueAttack1Y = KnockBackValueAttack1.y;
        newBiesValues.KnockBackValueAttack2X = KnockBackValueAttack2.x;
        newBiesValues.KnockBackValueAttack2Y = KnockBackValueAttack2.y;
        newBiesValues.KnockBackValueAttack3X = KnockBackValueAttack3.x;
        newBiesValues.KnockBackValueAttack3Y = KnockBackValueAttack3.y;

        newBiesValues.moveAccelerationTime = MoveAccelerationTime;
        newBiesValues.moveBrakingTime      = MoveBrakingTime;
        newBiesValues.RoarStunDuration = RoarStunDuration;



        string catValues = JsonUtility.ToJson(newBiesValues);
        System.IO.File.WriteAllText( Application.dataPath +  "/Resources/Temp/BiesValues.json", catValues);
    }


     public void LoadBalance( string path){
        var jsonFile = Resources.Load<TextAsset>(path).ToString();
        
        var newBiesValues = JsonUtility.FromJson<JsonLoader.BiesValues>(jsonFile);

        Attack1Damage = newBiesValues.Attack1Damage;
        Attack2Damage = newBiesValues.Attack2Damage;
        Attack3Damage = newBiesValues.Attack3Damage;
        RoarDamage    = newBiesValues.RoarDamage;

        KnockBackValueRoar    = new Vector2(newBiesValues.KnockBackValueRoarX, newBiesValues.KnockBackValueRoarY);
        KnockBackValueAttack1 = new Vector2(newBiesValues.KnockBackValueAttack1X, newBiesValues.KnockBackValueAttack1Y);
        KnockBackValueAttack2 = new Vector2(newBiesValues.KnockBackValueAttack2X, newBiesValues.KnockBackValueAttack2Y);
        KnockBackValueAttack3 = new Vector2(newBiesValues.KnockBackValueAttack3X, newBiesValues.KnockBackValueAttack3Y);

        minJumpHeight        = newBiesValues.minJumpHeight;
        targetJumpHeight     = newBiesValues.targetJumpHeight;
        timeToJumpApex       = newBiesValues.timeToJumpApex;
        moveDistance         = newBiesValues.moveDistance;
        moveDistanceInAir    = newBiesValues.moveDistanceInAir;
        maxMoveDistanceInAir = newBiesValues.maxMoveDistanceInAir;

        RoarStunDuration = newBiesValues.RoarStunDuration;

        MoveAccelerationTime = newBiesValues.moveAccelerationTime;
        MoveBrakingTime      = newBiesValues.moveBrakingTime;
    }

    public void LockCurrentTemp(){
        var jsonFile = System.IO.File.ReadAllText( Application.dataPath +  "/Resources/Temp/BiesValues.json" );
        var newBiesValues = JsonUtility.FromJson<JsonLoader.BiesValues>(jsonFile);
        string BiesValues = JsonUtility.ToJson(newBiesValues);
        System.IO.File.WriteAllText( Application.dataPath +  "/Resources/BiesValues.json", BiesValues);
    }
}
