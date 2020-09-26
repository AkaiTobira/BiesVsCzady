using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingAkaiController : AkaiController
{

    void Start()
    {
        m_FloorDetector   = transform.Find("Detector").GetComponent<ICollisionFloorDetector>();
        m_sightController = transform.Find("Detector").GetComponent<IFieldSightDetector>();
        m_animator      = transform.Find("Animator").GetComponent<Animator>();
        m_controller    = new SFSMEnemy( gameObject, new CzadIdle( gameObject ) );
        m_FloorDetector.Move( new Vector2(0.1f, 0) );
        saveHightValues = m_FloorDetector.GetComponent<Transform>().position.y;
        CreateAirNavPoints();

        currentHp = healthPoints;
        SetHpBarValues();
    }

    private void CreateAirNavPoints(){
        airNavPoints    = new List<Vector2>(posiblePositions.Count); 
        for( int i = 0; i < posiblePositions.Count; i ++ ){
            airNavPoints.Add( new Vector2());
        }
    }

    void UpdatePlayerDetection(){
        if( playerDetectedByBox && !isAlreadyInCombat ){
            m_controller.OverriteStates( "FlyingCombatEngage" );
            isAlreadyInCombat = true;
        }else if( isAlreadyInCombat && canFollowPlayer ) {
            
            GlobalUtils.Direction m_direciton = (GlobalUtils.Direction)
                                                Mathf.Sign(GlobalUtils.PlayerObject.position.x - 
                                                            m_FloorDetector.GetComponent<Transform>().position.x);

            lockedInAirPostion = 
            Vector2.SmoothDamp( 
                                lockedInAirPostion, 
                                new Vector2(
                                    GlobalUtils.PlayerObject.position.x + (( m_direciton == GlobalUtils.Direction.Left) ? -closeToFollow : closeToFollow),
                                    lockedInAirPostion.y
                                    ), 
                                ref animationVel, 
                                adjustPositonToPlayerTime);
            
            UpdateAirNevPoints();
        }
    }

    [HideInInspector] public List<Vector2> airNavPoints;

    private void UpdateAirNevPoints(){
        for( int i = 0; i < airNavPoints.Count; i++){
            airNavPoints[i] = lockedInAirPostion + posiblePositions[i];
        }
    }


    protected override void Update() {
        
        m_controller.Update();

        UpdatePlayerDetection();
        UpdateDebugConsole();
        DrawNavigationPoints();
        UpdateHurtDelayTimer();
        UpdateDeadTimer();

        if( toDeadTimer == 0 ) Destroy(gameObject);
    }

    void UpdateDebugConsole(){
        DebugConsole.transform.position = m_FloorDetector.GetComponent<Transform>().position + new Vector3( -20, 50, 0);
        DebugConsoleInfo2.text = m_controller.GetStackStatus();
        DebugConsoleInfo1.text = "";
        DebugConsoleInfo1.text += velocity.ToString() + "\n";
        DebugConsoleInfo1.text += "Player seen :" + m_sightController.isPlayerSeen().ToString() + "\n";
        DebugConsoleInfo1.text += "EnemyHp : " + currentHp.ToString() + "\n";

        Vector2 RayPosition = transform.Find("Detector").transform.position + new Vector3( 0, -7.5f, 0);
        Debug.DrawLine( RayPosition - new Vector2( combatRange, 0 ), RayPosition + new Vector2( combatRange, 0 ), new Color(1,0,1));

    }

    [Header("NavigationPoints")]

    public float startingHight = 0;
    public List<Vector2> posiblePositions = new List<Vector2>();

    public  float saveHightValues;

    public float moveCooldown;

    public float playerDropRange;

    public Vector2 lockedInAirPostion;

    public float maxFlyingSpeed;

    [Header("GlidingAttack")]

    public float glideSpeed = 2400;

    public Vector2 glideKnockbackValues    = new Vector2();

    public float glideDamage = 3;

    [Header("DistanceToPlayer")]

    public bool canFollowPlayer;

    public float adjustPositonToPlayerTime;

    [SerializeField] private float closeToFollow = 0;

    private Vector2 animationVel;

    private void DrawNavigationPoints(){
        lockedInAirPostion.y = saveHightValues + startingHight;
        float rayLenght = 5f;

        if( !isAlreadyInCombat ){

        Debug.DrawLine( 
            new Vector2( m_FloorDetector.GetComponent<Transform>().position.x - rayLenght, lockedInAirPostion.y  -rayLenght ),
            new Vector2( m_FloorDetector.GetComponent<Transform>().position.x + rayLenght, lockedInAirPostion.y  +rayLenght ),
            new Color( 1,0,0)
        );

        Debug.DrawLine( 
            new Vector2( m_FloorDetector.GetComponent<Transform>().position.x - rayLenght, lockedInAirPostion.y  +rayLenght ),
            new Vector2( m_FloorDetector.GetComponent<Transform>().position.x + rayLenght, lockedInAirPostion.y  -rayLenght ),
            new Color( 1,0,0)
        );

        foreach( Vector2 position in posiblePositions){
            Debug.DrawLine( 
                position + new Vector2( m_FloorDetector.GetComponent<Transform>().position.x -rayLenght, lockedInAirPostion.y-rayLenght),
                position + new Vector2( m_FloorDetector.GetComponent<Transform>().position.x +rayLenght, lockedInAirPostion.y+ rayLenght),
                new Color( 1,0,0)
            );
            Debug.DrawLine( 
                position + new Vector2( m_FloorDetector.GetComponent<Transform>().position.x -rayLenght, lockedInAirPostion.y+ rayLenght),
                position + new Vector2( m_FloorDetector.GetComponent<Transform>().position.x +rayLenght, lockedInAirPostion.y-rayLenght),
                new Color( 0,1,0)
            );
        }
        }else{


            
        Debug.DrawLine( 
            new Vector2( lockedInAirPostion.x - rayLenght, lockedInAirPostion.y -rayLenght ),
            new Vector2( lockedInAirPostion.x + rayLenght, lockedInAirPostion.y +rayLenght ),
            new Color( 1,0,0)
        );

        Debug.DrawLine( 
            new Vector2( lockedInAirPostion.x - rayLenght, lockedInAirPostion.y +rayLenght ),
            new Vector2( lockedInAirPostion.x + rayLenght, lockedInAirPostion.y -rayLenght ),
            new Color( 1,0,0)
        );

        foreach( Vector2 position in posiblePositions){
            Debug.DrawLine( 
                (Vector2)lockedInAirPostion + position + new Vector2( -rayLenght, -rayLenght),
                (Vector2)lockedInAirPostion + position + new Vector2(  rayLenght, + rayLenght),
                new Color( 1,0,0)
            );
            Debug.DrawLine( 
                (Vector2)lockedInAirPostion + position + new Vector2( -rayLenght, + rayLenght),
                (Vector2)lockedInAirPostion + position + new Vector2(  rayLenght, -rayLenght),
                new Color( 0,1,0)
            );
        }
        }

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
            case "FlyingEnemyGliding":
                infoPack.isValid = true;
                infoPack.knockBackValue = glideKnockbackValues;
                infoPack.stunDuration   = 0.0f;
                infoPack.lockFaceDirectionDuringKnockback = true;
                infoPack.attackDamage   = glideDamage;
                infoPack.fromCameAttack = m_FloorDetector.GetCurrentDirection();
            break;
            default: break;

        }

        return infoPack;
    }

    public override void OnHit(GlobalUtils.AttackInfo infoPack){
        if( !infoPack.isValid ) return;
        currentHp -= infoPack.attackDamage;
        SetHpBarValues();
        if( currentHp > 0 ){
            if( infoPack.stunDuration > 0){
                m_controller.OverriteStates( "FlyingStun", infoPack );
            }else{
                if( delayOfHurtGoInTimer >= 0 )
                m_controller.OverriteStates( "FlyingHurt", infoPack );
                delayOfHurtGoInTimer = delayOfHurtStartReEnter;
            }
        }else{
            m_controller.OverriteStates( "Dead", infoPack );
        }
    }

    

    void OnDrawGizmos(){

        float rayLenght = 5;

            Debug.DrawLine( 
                transform.position + new Vector3(  rayLenght, startingHight + rayLenght),
                transform.position + new Vector3( -rayLenght, startingHight -rayLenght),
                new Color( 1,0,0)
            );

            Debug.DrawLine( 
                transform.position + new Vector3(  rayLenght, startingHight -rayLenght),
                transform.position + new Vector3( -rayLenght, startingHight +rayLenght),
                new Color( 1,0,0)
            );

        Debug.DrawLine( transform.position, transform.position + new Vector3( patrolRangeLeft  - autoCorrectionRight , 0,0), new Color(0,1,1));
        Debug.DrawLine( transform.position, transform.position + new Vector3( patrolRangeRight - autoCorrectionLeft, 0,0), new Color(0,1,1));
    }

}
