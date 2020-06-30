using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SFSMBase m_controller;
    private CollisionDetectorPlayer m_detector;

    private Animator m_animator;

    [SerializeField] public bool invincible = true;
    void Start()
    {
        GlobalUtils.PlayerObject = transform;
        m_detector    = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMPlayerChange( transform.gameObject, new BiesIdle( gameObject ) );
        m_animator    = animationNode.gameObject.GetComponent<Animator>();
        CalculateMath();
//        Debug.Log( PlayerUtils.PlayerJumpForceMin.ToString() + " " + PlayerUtils.PlayerJumpForceMax.ToString() );
    }

    public bool isImmortal(){
        return invincible;
    }

    private void CalculateMath(){

    }

    private void UpdateCounters(){
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();
        PlayerFallOfWallHelper.IncrementCounters();
        PlayerSwipeLock.IncrementCounters();
        PlayerMoveOfWallHelper.IncrementCounters();
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


    [SerializeField] public float healthPoints = 10;

    public GlobalUtils.AttackInfo GetPlayerAttackInfo(){
        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();
        infoPack.stateName = m_controller.GetStateName();
        switch( infoPack.stateName ){
            case "BiesAttack1":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack1;
                infoPack.attackDamage   = BiesUtils.Attack1Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                infoPack.knockBackFrictionX = 0;
                break;
            }
            case "BiesAttack2":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack2;
                infoPack.attackDamage   = BiesUtils.Attack2Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                infoPack.knockBackFrictionX = 0;
                break;
            }
            case "BiesAttack3":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack3;
                infoPack.attackDamage   = BiesUtils.Attack3Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                infoPack.knockBackFrictionX = 0;
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
//        Debug.Log(other.gameObject.name);
    }



    public string GetCurrentFormName(){
        return m_controller.GetCurrentForm();
    }


    public void OnHit( GlobalUtils.AttackInfo infoPack ){
        if( !infoPack.isValid ) return;
        if( !invincible ) healthPoints -= infoPack.attackDamage;
        if( healthPoints > 0 ){
            if( infoPack.stunDuration > 0){
                m_controller.OverriteStates( "Stun", infoPack );
            }else{
                m_controller.OverriteStates( "Hurt", infoPack );
            }
        }else{
            m_controller.OverriteStates( "Dead", infoPack );
        }
        Debug.Log( "Player object is hurt : hp reduced to " + healthPoints.ToString());
    }

    void Update(){
        m_controller.Update();
        UpdateCounters();

        m_animator.SetBool("isGrounded", m_detector.isOnGround());
        m_animator.SetBool("isWallClose", m_detector.isWallClose());

        isOnGround  = m_detector.isOnGround();
        isWallClose = m_detector.isWallClose();
    //    Debug.Log( StateName );
        StateName   = m_controller.GetStateName();
        isColLeft =m_detector.isCollideWithLeftWall();
        isColRight= m_detector.isCollideWithLeftWall();
        
        directionLeft  = m_controller.GetDirection() == GlobalUtils.Direction.Left;
        directionRight = m_controller.GetDirection() == GlobalUtils.Direction.Right;


    // TEST SECTION

    if( Input.GetKeyDown(KeyCode.Keypad1)){
        GlobalUtils.AttackInfo info = new GlobalUtils.AttackInfo();
        
        info.isValid = true;
        info.attackDamage = 0;
        info.fromCameAttack = GlobalUtils.Direction.Left;
        info.knockBackFrictionX = 0;
        info.knockBackValue = new Vector2();
        info.stunDuration   = 0;
        OnHit( info );
    }


    if( Input.GetKeyDown(KeyCode.Keypad2)){
        GlobalUtils.AttackInfo info = new GlobalUtils.AttackInfo();
        
        info.isValid = true;
        info.attackDamage = 0;
        info.fromCameAttack = GlobalUtils.Direction.Left;
        info.knockBackFrictionX = 0;
        info.knockBackValue = new Vector2();
        info.stunDuration   = 3;
        OnHit( info );
    }


    if( Input.GetKeyDown(KeyCode.Keypad3)){
        GlobalUtils.AttackInfo info = new GlobalUtils.AttackInfo();
        
        info.isValid = true;
        info.attackDamage = 0;
        info.fromCameAttack = GlobalUtils.Direction.Left;
        info.knockBackFrictionX = 0;
        info.knockBackValue = new Vector2( 30, 4000 );
        info.stunDuration   = 0;
        OnHit( info );
    }


    if( Input.GetKeyDown(KeyCode.Keypad4)){
        GlobalUtils.AttackInfo info = new GlobalUtils.AttackInfo();
        
        info.isValid = true;
        info.attackDamage = 0;
        info.fromCameAttack = GlobalUtils.Direction.Left;
        info.knockBackFrictionX = 2;
        info.knockBackValue = new Vector2( 700, 4000 );
        info.stunDuration   = 0;
        OnHit( info );
    }


    if( Input.GetKeyDown(KeyCode.Keypad5)){
        GlobalUtils.AttackInfo info = new GlobalUtils.AttackInfo();
        
        info.isValid = true;
        info.attackDamage = 100;
        info.fromCameAttack = GlobalUtils.Direction.Left;
        info.knockBackFrictionX = 0;
        info.knockBackValue = new Vector2(  );
        info.stunDuration   = 0;
        OnHit( info );
    }
    }




}
