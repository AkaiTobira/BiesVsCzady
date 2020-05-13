using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SFSMBase m_controller;
    private PlayerUtils.Direction m_dir = PlayerUtils.Direction.Left;
    private CollisionDetectorPlayer m_detector;

    [SerializeField] float jumpHeight;
    [SerializeField] float timeToJumpApex;
    [SerializeField] float moveDistance = 15.0f;
    [SerializeField] float moveDistanceInAir = 5.0f;

    public void changeDirection( PlayerUtils.Direction dir, float angle = 361.0f ){
        if( m_dir == dir ) return;
        Vector3 localTransform = transform.localScale;
        localTransform.x       = Mathf.Abs( localTransform.x ) * (float)dir * -1 ;
        transform.localScale   = localTransform;
        m_dir = dir;
    }

    void Start()
    {
        m_detector    = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMBase( transform.gameObject, new PlayerIdle( gameObject ) );
        CalculateMath();
    }

    private void CalculateMath(){
        PlayerUtils.GravityForce     = (2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        PlayerUtils.PlayerSpeed      = moveDistance;
        PlayerUtils.PlayerJumpForce  = Mathf.Abs(PlayerUtils.GravityForce) * timeToJumpApex;
        PlayerUtils.PlayerSpeedInAir = moveDistanceInAir;
    }

    private void UpdateCounters(){
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();

        if (Debug.isDebugBuild) CalculateMath();
    }

    [SerializeField] bool isOnGround = false;
    [SerializeField] string StateName = "Idle";
    // Update is called once per frame
    void Update(){
        m_controller.Update();
        UpdateCounters();

        isOnGround = m_detector.isOnGround();
        StateName  = m_controller.GetStateName();
    }
}
