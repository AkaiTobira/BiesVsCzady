using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserAkaiController : AkaiController
{

    void Start()
    {
        m_FloorDetector   = transform.Find("Detector").GetComponent<ICollisionFloorDetector>();
        m_sightController = transform.Find("Detector").GetComponent<IFieldSightDetector>();
        m_animator        = transform.Find("Animator").GetComponent<Animator>();
        m_controller      = new SFSMEnemy( gameObject, new CzadIdle( gameObject ) );
        m_FloorDetector.Move( new Vector2(0.1f, 0) );
    
        currentHp = healthPoints;
        SetHpBarValues();

                _distanceToDebugInfo =   DebugConsole.transform.position - m_FloorDetector.GetComponent<Transform>().position;
    }

    void UpdatePlayerDetection(){
        if( playerDetectedByBox && !isAlreadyInCombat ){
            m_controller.OverriteStates( "ChaserCombatEngage" );
            isAlreadyInCombat = true;
        };
    }

    [HideInInspector] public List<Vector2> airNavPoints;

    protected override void Update() {
        
        m_controller.Update();

        UpdatePlayerDetection();
        UpdateDebugConsole();
        UpdateHurtDelayTimer();
        UpdateDeadTimer();

        if( toDeadTimer == 0 ) Destroy(gameObject);
    }


    void UpdateDebugConsole(){
        DebugConsole.transform.position = m_FloorDetector.GetComponent<Transform>().position +  _distanceToDebugInfo;
        DebugConsoleInfo2.text = m_controller.GetStackStatus();
        DebugConsoleInfo1.text = "";
        DebugConsoleInfo1.text += velocity.ToString() + "\n";
        DebugConsoleInfo1.text += "Player seen :" + m_sightController.isPlayerSeen().ToString() + "\n";
        DebugConsoleInfo1.text += "EnemyHp : " + currentHp.ToString() + "\n";

        Vector2 RayPosition = transform.Find("Detector").transform.position + new Vector3( 0, -7.5f, 0);
        Debug.DrawLine( RayPosition - new Vector2( combatRange, 0 ), RayPosition + new Vector2( combatRange, 0 ), new Color(1,0,1));
    }

    [Header("ChaseAttack")]
    public float chaseSpeed             = 2400;
    public Vector2 chaseKnockbackValues = new Vector2();
    public float chaseDamage            = 3;
    public float chasePreparation       = 1.0f;
    public float chaseBrakingTime       = 0;
    public float chaseAccelerationTime  = 0;
    public float chaseTargetPleaceNearPlayer = 50;
    public bool playerGetHitInChase = false;
    private Vector2 animationVel;

    public override GlobalUtils.AttackInfo GetAttackInfo(){

        string currentStateName = m_controller.GetStateName();

        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

        switch( currentStateName ){

            case "ChaserChaseAttack":
                playerGetHitInChase = true;
                Debug.Log( playerGetHitInChase );
                infoPack.isValid = true;
                infoPack.knockBackValue = chaseKnockbackValues;
                infoPack.stunDuration   = 0.0f;
                infoPack.lockFaceDirectionDuringKnockback = true;
                infoPack.attackDamage   = chaseDamage;
                infoPack.fromCameAttack = GlobalUtils.PlayerObject.position.x < m_FloorDetector.GetComponent<Transform>().position.x? 
                                            GlobalUtils.Direction.Left : GlobalUtils.Direction.Right;
            break;
            case "CzadStun":
                infoPack.isValid = false;
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
        if( infoPack.attackDamage > 500 ) m_controller.OverriteStates( "Dead", infoPack );
        if( m_controller.GetDirection() != infoPack.fromCameAttack ) return;
        currentHp -= infoPack.attackDamage;
        SetHpBarValues();
        if( currentHp > 0 ){
            if( infoPack.stunDuration > 0 && !infoPack.stateName.Contains("2")){
                m_controller.OverriteStates( "ChaserStun", infoPack );
            }else{
                if( delayOfHurtGoInTimer >= 0 ){
                    m_controller.OverriteStates( "ChaserHurt", infoPack );
                }
                delayOfHurtGoInTimer = delayOfHurtStartReEnter;
            }
        }else{
            m_controller.OverriteStates( "Dead", infoPack );
        }
    }
}
