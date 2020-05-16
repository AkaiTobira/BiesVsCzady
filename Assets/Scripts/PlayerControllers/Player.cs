using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SFSMBase m_controller;
    private PlayerUtils.Direction m_dir = PlayerUtils.Direction.Left;
    private CollisionDetectorPlayer m_detector;

    [SerializeField] float jumpHeight        = 0.0f;
    [SerializeField] float timeToJumpApex    = 0.0f;
    [SerializeField] float moveDistance      = 15.0f;
    [SerializeField] float moveDistanceInAir = 5.0f;

    [Range( 0.0f, 1.0f)] public float wallFriction = 1.0f;

    [SerializeField] Vector2 WallJumpFactors = new Vector2(0.0f,0.0f);

    void Start()
    {
        m_detector    = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMBase( transform.gameObject, new PlayerIdle( gameObject ) );
        CalculateMath();
    }

    private void CalculateMath(){
        PlayerUtils.GravityForce        = (2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        PlayerUtils.PlayerSpeed         = moveDistance;
        PlayerUtils.PlayerJumpForce     = Mathf.Abs(PlayerUtils.GravityForce) * timeToJumpApex;
        PlayerUtils.PlayerSpeedInAir    = moveDistanceInAir;
        PlayerUtils.MaxWallSlideSpeed   = PlayerUtils.GravityForce * wallFriction; 
        PlayerUtils.PlayerWallJumpForce = new Vector2( WallJumpFactors.x * PlayerUtils.PlayerSpeed,
                                                       WallJumpFactors.y * PlayerUtils.PlayerJumpForce);
    }

    private void UpdateCounters(){
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();
        PlayerFallOfWallHelper.IncrementCounters();
        PlayerSwipeLock.IncrementCounters();
        PlayerJumpOffWall.IncrementCounters();

        if (Debug.isDebugBuild) CalculateMath();
    }

    [SerializeField] bool isOnGround     = false;
    [SerializeField] bool isWallClose    = false;
    [SerializeField] bool isColLeft      = false;
    [SerializeField] bool isColRight     = false;
    [SerializeField] bool directionLeft  = false;
    [SerializeField] bool directionRight = false;
    [SerializeField] string StateName    = "Idle";
    // Update is called once per frame
    void Update(){
        m_controller.Update();
        UpdateCounters();

        isOnGround  = m_detector.isOnGround();
        isWallClose = m_detector.isWallClose();
        Debug.Log( StateName );
        StateName   = m_controller.GetStateName();
        isColLeft =m_detector.isCollideWithLeftWall();
        isColRight= m_detector.isCollideWithLeftWall();
        directionLeft  = m_controller.GetDirection() == PlayerUtils.Direction.Left;
        directionRight = m_controller.GetDirection() == PlayerUtils.Direction.Right;
    }
}
