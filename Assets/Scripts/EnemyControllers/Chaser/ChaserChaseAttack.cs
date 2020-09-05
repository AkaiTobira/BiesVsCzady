using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserChaseAttack : EnemyBaseState
{

    float distanceOfGlide;
    float timeOfMove  = 0.1f;
    //float timeOfGlide = 0;

    GlobalUtils.Direction dir;
    protected ICollisionWallDetector m_wallDetector;

    new ChaserAkaiController entityScript = null;

    private Vector2 turnAroundPoint;

    public ChaserChaseAttack( GameObject controllable ) : base(controllable){
        name = "ChaserChaseAttack";
        m_dir = (GlobalUtils.Direction)Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x);
        m_wallDetector = controllable.GetComponent<Transform>().Find("Detector").GetComponent<ICollisionWallDetector>();
        entityScript   = controllable.GetComponent<ChaserAkaiController>();
        newDirection   = m_dir;
        
        Vector3 playerPosition   = GlobalUtils.PlayerObject.transform.position;
        Vector3 detectorPosition = m_FloorDetector.GetComponent<Transform>().position;

        GlobalUtils.Direction targetSide = GlobalUtils.GetClosestSideToPosition(playerPosition, detectorPosition );

        turnAroundPoint = playerPosition + 
                            new Vector3( entityScript.chaseTargetPleaceNearPlayer * 
                                        (float)GlobalUtils.ReverseDirection( targetSide ), 0 );
        
        turnAroundSide = GlobalUtils.ReverseDirection( targetSide );
    }

    public override void UpdateAnimator(){
        m_animator.SetBool("isGliding", true);
        UpdateAnimatorAligment();
        UpdateFloorAligment(); 
        UpdateAnimatorPosition();
    }

    private void UpdateTurnAroundPoint(){
        Vector3 playerPosition   = GlobalUtils.PlayerObject.transform.position;
        Vector3 detectorPosition = m_FloorDetector.GetComponent<Transform>().position;

        GlobalUtils.Direction targetSide = GlobalUtils.GetClosestSideToPosition(playerPosition, detectorPosition );

        turnAroundPoint = playerPosition + 
                            new Vector3( entityScript.chaseTargetPleaceNearPlayer * 
                                        (float)turnAroundSide, 0 );

        DebugDrawHelper.DrawX( turnAroundPoint );
    }


/*
    Vector3 targetPosition = playerPosition + 
                        new Vector3( entityScript.combatRange * 
                                    (float)GlobalUtils.GetClosestSideToPosition(playerPosition,
                                                                                detectorPosition ), 0 );
    targetPosition = targetPosition - detectorPosition;
    m_nextState = new CzadAttackMove( m_controllabledObject, targetPosition );
*/

    //private float overTimeTimer = 1;

/*
    private void ProcessSwipe(){
        if( !swipeOn ) return;
        CommonValues.PlayerVelocity.x = ( m_swipe == GlobalUtils.Direction.Left)  ? 
                        Mathf.Max(  -m_settings.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x - m_settings.MoveSpeedInAir * Time.deltaTime): 
                        Mathf.Min(  m_settings.maxMoveDistanceInAir,
                                    CommonValues.PlayerVelocity.x + m_settings.MoveSpeedInAir * Time.deltaTime);

        m_dir = (GlobalUtils.Direction) Mathf.Sign(CommonValues.PlayerVelocity.x);
    }
*/

    GlobalUtils.Direction newDirection;
    GlobalUtils.Direction turnAroundSide;

    public void ProcessAcceleration(){
        float acceleration = (entityScript.chaseSpeed / entityScript.chaseAccelerationTime) * Time.deltaTime;
        float currentValue = Mathf.Min( Mathf.Abs( entityScript.velocity.x ) + acceleration,  entityScript.chaseSpeed );
        entityScript.velocity.x = currentValue * (int)m_dir;
    }

    public void ProcessBraking(){
        float acceleration      = (entityScript.chaseSpeed / entityScript.chaseBrakingTime) * Time.deltaTime;
        float currentValue      = Mathf.Max( Mathf.Abs( entityScript.velocity.x ) - acceleration, 0);
        entityScript.velocity.x = currentValue * (int)m_dir;

        if( currentValue == 0 ){
            newDirection   = GlobalUtils.ReverseDirection(m_dir);
            turnAroundSide = GlobalUtils.ReverseDirection(turnAroundSide);
            if( entityScript.playerGetHitInChase ) {
                m_isOver = true;
                entityScript.playerGetHitInChase = false;
            }
        }
    }

    private void ProcessLeftMove(){
        if( turnAroundPoint.x < m_FloorDetector.GetComponent<Transform>().position.x  ){
            ProcessAcceleration();
        }else{
            ProcessBraking();
        }
    }

    private void ProcessRightMove(){
        if( turnAroundPoint.x > m_FloorDetector.GetComponent<Transform>().position.x  ){
            ProcessAcceleration();
        }else{
            ProcessBraking();
        }
    }

    private void  ProcessGravity(){
        if( !m_FloorDetector.isOnGround() ){
            entityScript.velocity.y -= entityScript.gravityForce * Time.deltaTime;
            m_FloorDetector.Move( entityScript.velocity * Time.deltaTime);
        }else{
            entityScript.velocity.y = 0;
        }
    }

    public void ProcessMove(){
        timeOfMove += Time.deltaTime;
        ProcessGravity();
        if( newDirection != m_dir ) { m_dir = newDirection; };

        if( isLeftOriented() )        ProcessLeftMove();
        else if ( isRightOriented() ) ProcessRightMove();

        if( !m_wallDetector.isWallClose() )
        m_FloorDetector.Move( entityScript.velocity * Time.deltaTime );
        

        if( m_isOver){
            m_animator.SetBool("isGliding", !m_isOver);
        }
    }

    public override void  Process(){
        ProcessMove();
        ProcessStateOver();

        UpdateTurnAroundPoint();
    }

    public void ProcessStateOver(){
        if( m_wallDetector.isWallClose() ){

            if( m_wallDetector.isCollideWithLeftWall() && isLeftOriented() ){ 
                GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

                infoPack.isValid      = true;
                infoPack.stunDuration = 5f;
                m_isOver = true;
                infoPack.knockBackValue = new Vector2( 10, 0);
                infoPack.lockFaceDirectionDuringKnockback = true;
                infoPack.fromCameAttack = GlobalUtils.Direction.Left;

                m_nextState = new CzadStun( m_controllabledObject, infoPack);
                
            }else if( m_wallDetector.isCollideWithRightWall() && isRightOriented()){
                GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

                infoPack.isValid      = true;
                infoPack.stunDuration = 5f;
                m_isOver = true;

                infoPack.knockBackValue = new Vector2( 10, 0);
                infoPack.lockFaceDirectionDuringKnockback = true;
                infoPack.fromCameAttack = GlobalUtils.Direction.Right;

                m_nextState = new CzadStun( m_controllabledObject, infoPack);
            }
        }

//        Debug.Log( Mathf.Abs( GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x ) );

        if(  Mathf.Abs( GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x ) - m_FloorDetector.GetComponent<CollisionDetectorEnemy>().sightLenght > 0  ){
            m_isOver = true;
        }

        if( m_isOver){
            m_animator.SetBool("isGliding", false);
        }
    }
}
