﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IEntity
{

    [SerializeField] public bool invincible = false;

    [SerializeField] public float MaxHealthPoints = 10;

    [SerializeField] public HpBarController hpBar;
    public float healthPoints = 10;

    private ICollisionWallDetector m_WallDetector;
    private ICollisionInteractableDetector m_InteractionDetector;

    private float inAnimatorBaseSpeed;

    private float baseInputLockAfterRebirth = 1.0f;

    void Awake() {
        GlobalUtils.PlayerObject = transform;
    }

    public void GetWings(){

        GUIElements.LightHit.ApplyLightColors();
        baseInputLockAfterRebirth = 1000f;
        getWingsAnimationTimer = 0.74f;
        gameOver = true;
    }


    private float getWingsAnimationTimer = 0;


    void Start()
    {
        m_FloorDetector       = GetComponent<CollisionDetectorPlayer>();
        m_WallDetector        = GetComponent<CollisionDetectorPlayer>();
        m_InteractionDetector = GetComponent<CollisionDetectorPlayer>();
        m_controller  = new SFSMPlayerChange( transform.gameObject, new BiesIdle( gameObject ) );
        m_animator    = animationNode.gameObject.GetComponent<Animator>();
        healthPoints  = MaxHealthPoints;

        var inAnimator      = transform.parent.GetComponent<Animator>();
        inAnimatorBaseSpeed = inAnimator.speed;
        inAnimator.speed    = timeOfInvincibility/getAnimationLenght(inAnimator, "Invincibility");
    }

    protected float getAnimationLenght( Animator a, string animationName){
        RuntimeAnimatorController ac = a.runtimeAnimatorController;   
        for (int i = 0; i < ac.animationClips.Length; i++){
            if (ac.animationClips[i].name == animationName)
                return ac.animationClips[i].length;
        }
        return 0.0f;
    }

    public void ResetPlayer(){
        baseInputLockAfterRebirth = 1.0f;
        animationNode.GetComponent<Animator>().Rebind();
        m_controller  = new SFSMPlayerChange( transform.gameObject, new BiesIdle( gameObject ) );
        healthPoints  = MaxHealthPoints;
        hpBar.UpdateHp( healthPoints, MaxHealthPoints );
    }

    public bool isImmortal(){
        return invincible;
    }

    private void UpdateCounters(){
        baseInputLockAfterRebirth -= Time.deltaTime;
        PlayerJumpHelper.IncrementCounters();
        PlayerFallHelper.IncrementCounters();
        PlayerFallOfWallHelper.IncrementCounters();
        PlayerSwipeLock.IncrementCounters();
        PlayerMoveOfWallHelper.IncrementCounters();
        PlayerJumpOffWall.IncrementCounters();
        PlayerRoarHelper.IncrementCounters();
        PlayerAnimatorLock.IncrementCounters();
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
                infoPack.knockBackValue = BiesUtils.KnockBackValueRoar;
                infoPack.attackDamage   = BiesUtils.RoarDamage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                infoPack.knockBackFrictionX = 0;
                infoPack.stunDuration       = BiesUtils.RoarStunDuration;
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
                infoPack.knockBackValue = BiesUtils.KnockBackValueAttack2;
                infoPack.attackDamage   = BiesUtils.Attack2Damage;
                infoPack.fromCameAttack = m_controller.GetDirection();
                infoPack.knockBackFrictionX = 0;
                break;
            }
            case "BiesAttack5":
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


    public void SwitchGoodMode(){
        invincible = !invincible;
    }


    public string GetCurrentFormName(){
        string stateName = m_controller.GetStateName();


        return m_controller.GetStateName();
    }

//    private 


    public float timeOfInvincibility = 1.5f;
    private float invincibilityDuration = 0f;
    public bool  CanBeHurt(){
        if( invincibilityDuration > 0 ) return false;
        if( m_controller.GetStateName().Contains("Dead")) return false;
        if( m_controller.GetStateName().Contains("Hurt")) return false;
        if( blockActive ) return false;
        return true;
    }

    public bool blockActive = false;


    public override void OnHit( GlobalUtils.AttackInfo infoPack ){
        if( !infoPack.isValid ) return;
        if( !CanBeHurt() )      return;
        if( !invincible ) {
            healthPoints -= infoPack.attackDamage;
            GUIElements.LightHit.ApplyHurtColors();
        }
        if( healthPoints > 0 ){
            if( infoPack.stunDuration > 0){
                m_controller.OverriteStates( "Stun", infoPack );
            }else{
                m_controller.OverriteStates( "Hurt", infoPack );
                invincibilityDuration = timeOfInvincibility;
                transform.parent.GetComponent<Animator>().enabled = true;
            }
        }else{
            m_controller.OverriteStates( "Dead", infoPack );
        }

        hpBar.UpdateHp( healthPoints, MaxHealthPoints );

    //    Debug.Log( "Player object is hurt : hp reduced to " + healthPoints.ToString());
    }

    [SerializeField] public int keys = 0;

    public string GetStackInfo(){
        return m_controller.StackStatusPrint();
    }

    public void AddKey(){ keys++; }


    private void HurtBehaviourTests(){
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
    public void UpdateInvincibility(){
        if( invincibilityDuration < 0) return;
        invincibilityDuration -= Time.deltaTime;

        var inAnimator = transform.parent.GetComponent<Animator>();
        float newSpeed = timeOfInvincibility/getAnimationLenght(inAnimator, "Invincibility");

        if( Mathf.Abs(newSpeed - inAnimator.speed) > 0.0001f ){
            inAnimator.speed = newSpeed;
        }

        if( invincibilityDuration < 0 ) {
            inAnimator.enabled = false;
            Color c = m_animator.GetComponent<SpriteRenderer>().color;
            c.a = 1;
            m_animator.GetComponent<SpriteRenderer>().color = c;
        }
    }


    private bool anotherOne = false;
    public bool gameOver = false;
    private float showCongratulationTimer = 4;
    void Update(){
        if( baseInputLockAfterRebirth < 0 && Time.timeScale == 1) {
            m_controller.Update();
        }else{
            m_animator.transform.position = m_FloorDetector.GetComponent<Transform>().position;
        }
        UpdateCounters();
        UpdateInvincibility();

        m_animator.SetBool("isGrounded", m_FloorDetector.isOnGround());
        m_animator.SetBool("isWallClose", m_WallDetector.isWallClose());

        isOnGround  = m_FloorDetector.isOnGround();
        isWallClose = m_WallDetector.isWallClose();
        StateName   = m_controller.GetStateName();
        isColLeft   = m_WallDetector.isCollideWithLeftWall();
        isColRight  = m_WallDetector.isCollideWithLeftWall();
        
        directionLeft  = m_controller.GetDirection() == GlobalUtils.Direction.Left;
        directionRight = m_controller.GetDirection() == GlobalUtils.Direction.Right;

        HurtBehaviourTests();
        GodModeSwitch();
        ChangeLowHPParameterFMOD();

        CommonValues.tempModulator  = t1;
        CommonValues.tempModulator2 = t2;

        ProcessEndGame();
    }

    private void ProcessEndGame(){

        getWingsAnimationTimer = Mathf.Max( getWingsAnimationTimer -Time.deltaTime, 0 );
        if( getWingsAnimationTimer == 0 && gameOver){
            if( !anotherOne ) m_animator.SetTrigger("GetWings");
            anotherOne = true;
            showCongratulationTimer = Mathf.Max( showCongratulationTimer  - Time.deltaTime, 0 );
            if( showCongratulationTimer == 0){
                GUIElements.GUIinfo.SetActive(false);
                hpBar.gameObject.SetActive(false);
                GUIElements.endScreen.enabled = true;
                GUIElements.endScreen.SetTrigger("Show");
                showCongratulationTimer = 1000000;
            }
        }
    }


    public float t1;
    public float t2;

    void GodModeSwitch(){
        if( Input.GetKeyDown(KeyCode.F1)){
            SwitchGoodMode();
        }
    }

    public void ChangeLowHPParameterFMOD()
    {
        if(healthPoints < 6)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LowHP", 1, false);
        }
        else
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("LowHP", 0, false);
        }


    }

}
