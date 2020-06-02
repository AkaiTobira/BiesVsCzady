using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SFSMBase m_controller;
    private CollisionDetectorPlayer m_detector;

    private Animator m_animator;

    [SerializeField] float minJumpHeight         = 5.0f;
    [SerializeField] float jumpHeight            = 0.0f;
    [SerializeField] float jumpHoldWallTimeDelay = 0.0f;
    [SerializeField] public float timeToJumpApex = 0.0f;
    [SerializeField] float moveDistance          = 15.0f;
    [SerializeField] float moveDistanceInAir     = 5.0f;
    [SerializeField] float maxMoveSpeedInAir     = 10.0f;

    [SerializeField] float maxWallClimbSpeed     = 10.0f;
    [SerializeField] float wallClimbSpeed        = 10.0f;


    [Range( 0.0f, 1.0f)] public float wallFriction = 1.0f;

    [SerializeField] Vector2 WallJumpFactors = new Vector2(0.0f,0.0f);

    void Start()
    {
        GlobalUtils.PlayerObject = transform;
        m_detector    = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMBase( transform.gameObject, new PlayerIdle( gameObject ) );
        m_animator    = animationNode.gameObject.GetComponent<Animator>();
        CalculateMath();
        Debug.Log( PlayerUtils.PlayerJumpForceMin.ToString() + " " + PlayerUtils.PlayerJumpForceMax.ToString() );
    }

    private void CalculateMath(){
        PlayerUtils.GravityForce          = (2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        PlayerUtils.PlayerJumpForceMin    = Mathf.Sqrt (2 * Mathf.Abs (PlayerUtils.GravityForce) * minJumpHeight);
        PlayerUtils.PlayerSpeed           = moveDistance;
        PlayerUtils.PlayerJumpForceMax    = Mathf.Abs(PlayerUtils.GravityForce) * timeToJumpApex;
        PlayerUtils.JumpMaxTime           = timeToJumpApex;
        PlayerUtils.JumpHoldTimeDelay     = jumpHoldWallTimeDelay;
        PlayerUtils.MoveSpeedInAir      = moveDistanceInAir;
        PlayerUtils.MaxWallSlideSpeed     = PlayerUtils.GravityForce * wallFriction; 
        PlayerUtils.PlayerWallJumpForce   = new Vector2( WallJumpFactors.x * PlayerUtils.PlayerSpeed,
                                                         WallJumpFactors.y * PlayerUtils.PlayerJumpForceMax);

        PlayerUtils.MaxMoveSpeedInAir = maxMoveSpeedInAir;
        PlayerUtils.MaxWallClimbSpeed = maxWallClimbSpeed;
        PlayerUtils.WallClimbSpeed    = wallClimbSpeed;
    }

    private void UpdateCounters(){
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();
        PlayerFallOfWallHelper.IncrementCounters();
        PlayerSwipeLock.IncrementCounters();
        PlayerJumpOffWall.IncrementCounters();

        if (Debug.isDebugBuild) CalculateMath();
    }

    [SerializeField] public Transform animationNode;

    [SerializeField] bool isOnGround     = false;
    [SerializeField] bool isWallClose    = false;
    [SerializeField] bool isColLeft      = false;
    [SerializeField] bool isColRight     = false;
    [SerializeField] bool directionLeft  = false;
    [SerializeField] bool directionRight = false;
    [SerializeField] string StateName    = "Idle";
    // Update is called once per frame


    public GlobalUtils.AttackStateInfo GetPlayerAttackInfo(){
        GlobalUtils.AttackStateInfo infoPack = new GlobalUtils.AttackStateInfo();
        infoPack.stateName = m_controller.GetStateName();
        switch( infoPack.stateName ){
            case "PlayerAttack1":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = PlayerUtils.KnockBackValueAttack1;
                infoPack.attackDamage   = PlayerUtils.Attack1Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                break;
            }
            case "PlayerAttack2":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = PlayerUtils.KnockBackValueAttack2;
                infoPack.attackDamage   = PlayerUtils.Attack2Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                break;
            }
            case "PlayerAttack3":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = PlayerUtils.KnockBackValueAttack3;
                infoPack.attackDamage   = PlayerUtils.Attack3Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                break;
            }
            default:
            {
                infoPack.isValid = false;
                break;
            }
        }

        return infoPack;
    }

    void Update(){
        m_controller.Update();
        UpdateCounters();

        isOnGround  = m_detector.isOnGround();
        isWallClose = m_detector.isWallClose();
        Debug.Log( StateName );
        StateName   = m_controller.GetStateName();
        isColLeft =m_detector.isCollideWithLeftWall();
        isColRight= m_detector.isCollideWithLeftWall();
        
        directionLeft  = m_controller.GetDirection() == GlobalUtils.Direction.Left;
        directionRight = m_controller.GetDirection() == GlobalUtils.Direction.Right;
    }
}
