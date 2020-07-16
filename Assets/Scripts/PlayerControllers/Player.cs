using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IEntity
{

    [SerializeField] public bool invincible = true;

    private ICollisionWallDetector m_WallDetector;
    private ICollisionInteractableDetector m_InteractionDetector;

    void Start()
    {
        GlobalUtils.PlayerObject = transform;
        m_FloorDetector       = GetComponent<CollisionDetectorPlayer>();
        m_WallDetector        = GetComponent<CollisionDetectorPlayer>();
        m_InteractionDetector = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMPlayerChange( transform.gameObject, new BiesIdle( gameObject ) );
        m_animator    = animationNode.gameObject.GetComponent<Animator>();
    }

    public bool isImmortal(){
        return invincible;
    }

    private void UpdateCounters(){
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();
        PlayerFallOfWallHelper.IncrementCounters();
        PlayerSwipeLock.IncrementCounters();
        PlayerMoveOfWallHelper.IncrementCounters();
        PlayerJumpOffWall.IncrementCounters();

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

    public override GlobalUtils.AttackInfo GetAttackInfo(){
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
                infoPack.stunDuration       = 2.0f;
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
            case "BiesAttack4":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack1;
                infoPack.attackDamage   = BiesUtils.Attack1Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                infoPack.knockBackFrictionX = 0;
                break;
            }
            case "BiesAttack5":
            {
                infoPack.isValid = true;
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack1;
                infoPack.attackDamage   = BiesUtils.Attack1Damage;
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


    public void SwitchGoodMode(){
        invincible = !invincible;
    }

    void OnTriggerEnter2D(Collider2D other) {
//        Debug.Log(other.gameObject.name);
    }



    public string GetCurrentFormName(){
        string stateName = m_controller.GetStateName();


        return m_controller.GetStateName();
    }

    public bool  CanBeHurt(){
        if( m_controller.GetStateName().Contains("Dead")) return false;
        if( m_controller.GetStateName().Contains("Hurt")) return false;
        return true;
    }


    public override void OnHit( GlobalUtils.AttackInfo infoPack ){
        if( !infoPack.isValid ) return;
        if( !CanBeHurt() )      return;
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

    [SerializeField] public int keys = 0;

    public void AddKey(){ keys++; }


    //TOREMOVE
    [SerializeField] float someSillyValue = 0;
    [SerializeField] float someSillyValue2 = 0;
    void Update(){
        m_controller.Update();
        UpdateCounters();

        CommonValues.tempModulator  = someSillyValue;
        CommonValues.tempModulator2 = someSillyValue2;
        


        m_animator.SetBool("isGrounded", m_FloorDetector.isOnGround());
        m_animator.SetBool("isWallClose", m_WallDetector.isWallClose());

        isOnGround  = m_FloorDetector.isOnGround();
        isWallClose = m_WallDetector.isWallClose();
    //    Debug.Log( StateName );
        StateName   = m_controller.GetStateName();
        isColLeft   =m_WallDetector.isCollideWithLeftWall();
        isColRight  = m_WallDetector.isCollideWithLeftWall();
        
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
