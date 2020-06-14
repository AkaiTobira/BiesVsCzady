using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SFSMBase m_controller;
    private CollisionDetectorPlayer m_detector;

    private Animator m_animator;

    void Start()
    {
        GlobalUtils.PlayerObject = transform;
        m_detector    = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMPlayerChange( transform.gameObject, new BiesIdle( gameObject ) );
        m_animator    = animationNode.gameObject.GetComponent<Animator>();
        CalculateMath();
        Debug.Log( PlayerUtils.PlayerJumpForceMin.ToString() + " " + PlayerUtils.PlayerJumpForceMax.ToString() );
    }

    private void CalculateMath(){

    }

    private void UpdateCounters(){
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();
        PlayerFallOfWallHelper.IncrementCounters();
        PlayerSwipeLock.IncrementCounters();
        PlayerJumpOffWall.IncrementCounters();

        if (Debug.isDebugBuild) CalculateMath();
    }


    [Header("DebugInfo")]
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
            case "BiesAttack1":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack1;
                infoPack.attackDamage   = BiesUtils.Attack1Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                break;
            }
            case "BiesAttack2":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack2;
                infoPack.attackDamage   = BiesUtils.Attack2Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                break;
            }
            case "BiesAttack3":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack3;
                infoPack.attackDamage   = BiesUtils.Attack3Damage;
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

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
    }

    void Update(){
        m_controller.Update();
        UpdateCounters();

        isOnGround  = m_detector.isOnGround();
        isWallClose = m_detector.isWallClose();
    //    Debug.Log( StateName );
        StateName   = m_controller.GetStateName();
        isColLeft =m_detector.isCollideWithLeftWall();
        isColRight= m_detector.isCollideWithLeftWall();
        
        directionLeft  = m_controller.GetDirection() == GlobalUtils.Direction.Left;
        directionRight = m_controller.GetDirection() == GlobalUtils.Direction.Right;
    }




}
