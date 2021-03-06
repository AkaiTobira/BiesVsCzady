﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkaiController : IEntity, IEnemy
{
    [SerializeField] protected float healthPoints = 30;
    protected float currentHp;
    [SerializeField] public float onTouchDamage   = 1;

    public Vector2 onTouchKnockbackValues = new Vector2();

    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public bool isAlreadyInCombat = false;

    [SerializeField] private HpBarController hpBar;

    [Header("MoveValues")]

    public bool isPositionLocked = false;

    public float moveBrakingTime = 0;
    public float moveAccelerationTime = 0;

    public float maxMoveSpeed    = 0;

    public float hurtSpeedDropFrictionX = 0; 

    public float gravityForce    = 0;

    [Header("PatrolBehaviour")]
    public bool canPatrol       = false;

    public float patrolRangeLeft    = 0;
    public float patrolRangeRight    = 0;

    public float autoCorrectionLeft = 0;
    public float autoCorrectionRight = 0;


    [Header("RandomMoveBehaviour")]
    public bool canRandomMove   = false;

    public float maxMoveDistance = 0;

    [Header("MeeleAttackBehaviour")]
    public bool canMeeleAttack;
    public float timeOfBeeingHurt = 3.0f;
    public float delayOfHurtStartReEnter = 4.0f;
    [SerializeField]  public float massFactor = 0.2f;
    public float delayOfFirstAttack  = 3;
    public float breakBeetweenAttacks = 0;
    public float attackDamage         = 0;
    public float combatRange          = 300;
    public Vector2 knockbackValues    = new Vector2();
    public float stunDuration         = 0;

    [Header("JumpAttackBehaviour")]

    public bool canJumpOnPlayer = false;
    public float jumpAttackBreak = 2;
    public Vector2 jumpAttackKnockbackValue = new Vector2();
    public float jumpAttackDamage = 1;

    [Header("ShotAttackBehaviour")]

    public bool canShot;
    public float breakBeetweenShots =  1.0f ;

    [Range(0,10)] public float probabilityOfShot = 1;

    public float shotRange = 1000;

    [Header("Debug")]

    [SerializeField] public GameObject DebugConsole;

    [SerializeField] public Text DebugConsoleInfo1;
    [SerializeField] public Text DebugConsoleInfo2;

    public IFieldSightDetector m_sightController;

    [HideInInspector] public float toDeadTimer = 10000000.0f;
    [HideInInspector] public float delayOfHurtGoInTimer = 0.0f;

    protected Vector3 _distanceToDebugInfo = new Vector3();

    void Start()
    {
        m_FloorDetector   = transform.Find("Detector").GetComponent<ICollisionFloorDetector>();
        m_sightController = transform.Find("Detector").GetComponent<IFieldSightDetector>();
        m_animator      = transform.Find("Animator").GetComponent<Animator>();
        m_controller    = new SFSMEnemy( gameObject, new CzadIdle( gameObject ) );
        m_FloorDetector.Move( new Vector2(0.1f, 0) );
        
        currentHp = healthPoints;
        SetHpBarValues();

        _distanceToDebugInfo =   DebugConsole.transform.position - m_FloorDetector.GetComponent<Transform>().position;
    }

    protected void SetHpBarValues(){
        if( hpBar == null ) return;
        hpBar.UpdateHp( currentHp, healthPoints);
    }

    public Vector3 GetPosition(){
        return m_FloorDetector.GetComponent<Transform>().position;
    }

    void UpdatePlayerDetection(){ 
        if( playerDetectedByBox && !isAlreadyInCombat ){
            m_controller.OverriteStates( "CombatEngage" );
            isAlreadyInCombat = true;
        }
    }

    protected virtual void Update() {
        m_controller.Update();
        UpdatePlayerDetection();
        UpdateHurtDelayTimer();
        UpdateDebugConsole();
        UpdateDeadTimer();

        if( toDeadTimer == 0 ) Destroy(gameObject);
    }

    protected void UpdateDeadTimer(){
        toDeadTimer = Mathf.Max( toDeadTimer - Time.deltaTime, 0);
    }

    protected void UpdateHurtDelayTimer(){
        delayOfHurtGoInTimer = Mathf.Max( delayOfHurtGoInTimer - Time.deltaTime, 0);
    }

    public bool playerDetectedByBox = false;

    public void OnPlayerDetection(){
        playerDetectedByBox = true;
    }

    public void OnPlayerEscape(){
        if( !m_controller.GetStateName().Contains("Stun") ){
            playerDetectedByBox = false;
            isAlreadyInCombat   = false;
            m_controller        = new SFSMEnemy( gameObject, new CzadIdle( gameObject ) );
            m_animator.Rebind();
            m_FloorDetector.Move( new Vector2(0.1f, 0) );
        }
    }

    public void ResetPatrolValues(){
        autoCorrectionRight = 0;
        autoCorrectionLeft  = 0;
    }

    void UpdateDebugConsole(){

        DebugConsole.transform.position = m_FloorDetector.GetComponent<Transform>().position + _distanceToDebugInfo;
        DebugConsoleInfo2.text = m_controller.GetStackStatus();
        DebugConsoleInfo1.text = "";
        DebugConsoleInfo1.text += velocity.ToString() + "\n";
        DebugConsoleInfo1.text += "Player seen :" + m_sightController.isPlayerSeen().ToString() + "\n";
        DebugConsoleInfo1.text += "EnemyHp : " + currentHp.ToString() + "\n";

        Vector2 RayPosition = transform.Find("Detector").transform.position + new Vector3( 0, -7.5f, 0);
        Debug.DrawLine( RayPosition - new Vector2( combatRange, 0 ), RayPosition + new Vector2( combatRange, 0 ), new Color(1,0,1));
    }

    public override GlobalUtils.AttackInfo GetAttackInfo(){

        string currentStateName = m_controller.GetStateName();

        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

        switch( currentStateName ){

            case "CzadAttackMelee":
                infoPack.isValid = true;
                infoPack.knockBackValue = knockbackValues;
                infoPack.stunDuration   = 0.0f;
                infoPack.lockFaceDirectionDuringKnockback = true;
                infoPack.attackDamage   = attackDamage;
                infoPack.fromCameAttack = m_FloorDetector.GetCurrentDirection();
            break;


            default: 
                infoPack.isValid = true;
                infoPack.knockBackValue = onTouchKnockbackValues;
                infoPack.stunDuration   = 0.0f;
                infoPack.lockFaceDirectionDuringKnockback = true;
                infoPack.attackDamage   = onTouchDamage;
                infoPack.fromCameAttack = GlobalUtils.PlayerObject.position.x < m_FloorDetector.GetComponent<Transform>().position.x? 
                                            GlobalUtils.Direction.Left : GlobalUtils.Direction.Right;
            break;
        }

        return infoPack;
    }

    public override void OnHit(GlobalUtils.AttackInfo infoPack){
        if( !infoPack.isValid ) return;
        currentHp -= infoPack.attackDamage;
        SetHpBarValues();
        if( currentHp > 0 ){
            if( infoPack.stunDuration > 0){
                m_controller.OverriteStates( "Stun", infoPack );
            }else{
                if( delayOfHurtGoInTimer <= 0.0f){
                    m_controller.OverriteStates( "Hurt", infoPack );
                }
                delayOfHurtGoInTimer = delayOfHurtStartReEnter;
            }
        }else{
            m_controller.OverriteStates( "Dead", infoPack );
        }
    }



    void OnDrawGizmos(){
        Debug.DrawLine( transform.position, transform.position + new Vector3( patrolRangeLeft  - autoCorrectionRight , 0,0), new Color(0,1,1));
        Debug.DrawLine( transform.position, transform.position + new Vector3( patrolRangeRight - autoCorrectionLeft, 0,0), new Color(0,1,1));

    }

}
