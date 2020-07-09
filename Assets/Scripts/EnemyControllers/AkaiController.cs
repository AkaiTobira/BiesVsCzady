using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkaiController : IEntity
{
    [HideInInspector] public Vector2 velocity;

    [Header("MoveValues")]

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


    [SerializeField]  public float massFactor = 0.2f;

    public float delayOfFirstAttack  = 3;

    public float breakBeetweenAttacks = 0;

    public float attackDamage         = 0;

    public float combatRange          = 300;

    public Vector2 knockbackValues    = new Vector2();

    public float stunDuration         = 0;

    [Header("Debug")]

    [SerializeField] public GameObject DebugConsole;

    [SerializeField] public Text DebugConsoleInfo1;
    [SerializeField] public Text DebugConsoleInfo2;

    public IFieldSightDetector m_sightController;

    [HideInInspector] public bool isDead = false;

    void Start()
    {
        m_FloorDetector   = transform.Find("Detector").GetComponent<ICollisionFloorDetector>();
        m_sightController = transform.Find("Detector").GetComponent<IFieldSightDetector>();
        m_animator      = transform.Find("Animator").GetComponent<Animator>();
        m_controller    = new SFSMEnemy( gameObject, new CzadIdle( gameObject ) );
        m_FloorDetector.Move( new Vector2(0.1f, 0) );

    }


    [HideInInspector] public bool isAlreadyInCombat = false;

    void UpdatePlayerDetection(){
        if( m_sightController.isPlayerSeen() && !isAlreadyInCombat ){
            m_controller.OverriteStates( "CombatEngage" );
            isAlreadyInCombat = true;
        }
    }

    void Update() {
        m_controller.Update();

        UpdatePlayerDetection();
        UpdateDebugConsole();


        if( isDead ) Destroy(gameObject);
    }

    public void ResetPatrolValues(){
        autoCorrectionRight = 0;
        autoCorrectionLeft  = 0;
    }

    void UpdateDebugConsole(){
        DebugConsole.transform.position = m_FloorDetector.GetComponent<Transform>().position + new Vector3( -200, 500, 0);
        DebugConsoleInfo2.text = m_controller.GetStackStatus();
        DebugConsoleInfo1.text = "";
        DebugConsoleInfo1.text += velocity.ToString() + "\n";
        DebugConsoleInfo1.text += "Player seen :" + m_sightController.isPlayerSeen().ToString() + "\n";
        DebugConsoleInfo1.text += "EnemyHp : " + healthPoints.ToString() + "\n";

        Vector2 RayPosition = transform.Find("Detector").transform.position + new Vector3( 0, -75, 0);
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
            default: break;

        }

        return infoPack;
    }

    float healthPoints = 30;

    public override void OnHit(GlobalUtils.AttackInfo infoPack){
        if( !infoPack.isValid ) return;
         healthPoints -= infoPack.attackDamage;
        if( healthPoints > 0 ){
            if( infoPack.stunDuration > 0){
                m_controller.OverriteStates( "Stun", infoPack );
            }else{
                m_controller.OverriteStates( "Hurt", infoPack );
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
