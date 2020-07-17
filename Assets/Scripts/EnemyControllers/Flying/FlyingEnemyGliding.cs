using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyGliding : FlyingEnemyBaseState
{

    PointInterpolation.PointPack infoPack;
    float distanceOfGlide;
    float timeOfMove  = 0.1f;
    float timeOfGlide = 0;

    GlobalUtils.Direction dir;
    protected ICollisionWallDetector m_wallDetector;
    public FlyingEnemyGliding( GameObject controllable ) : base(controllable){
        name = "FlyingEnemyGliding";
        m_dir = (GlobalUtils.Direction)Mathf.Sign(GlobalUtils.PlayerObject.position.x - m_FloorDetector.GetComponent<Transform>().position.x);
        fillInfoPack();
        m_wallDetector = controllable.GetComponent<Transform>().Find("Detector").GetComponent<ICollisionWallDetector>();
    }

    public override void UpdateAnimator(){
        UpdateAnimatorAligment();
        UpdateFloorAligment();
        UpdateAnimatorPosition();
    }

    private void  fillInfoPack(){

        float tempMultipler = ( isLeftOriented() ) ? 1 : -1;

        Vector2 StartPoint    = m_FloorDetector.GetComponent<Transform>().position;
        Vector2 ControlPoint1 = GlobalUtils.PlayerObject.position + new Vector3( 1500f * tempMultipler, 300f );
        Vector2 ControlPoint2 = GlobalUtils.PlayerObject.position + new Vector3( 1300f * tempMultipler, 0f );
        Vector2 EndPoint      = GlobalUtils.PlayerObject.position - new Vector3( 1500f * tempMultipler, 0f );;

        distanceOfGlide = 
        Vector3.Distance(ControlPoint1,StartPoint)    +
        Vector3.Distance(ControlPoint2,ControlPoint1) +
        Vector3.Distance(EndPoint, ControlPoint2) * 0.6f; 

        timeOfGlide = distanceOfGlide/entityScript.glideSpeed;

        infoPack = new PointInterpolation.PointPack(StartPoint, ControlPoint1, ControlPoint2, EndPoint);
    }

    private void DebugDraw(){
        float rayLenght = 50;

        Vector3[] tempArray = new Vector3[4];
        tempArray[0] = infoPack.startPoint;
        tempArray[1] = infoPack.controlPoint2;
        tempArray[2] = infoPack.controlPoint1;
        tempArray[3] = infoPack.endPoint;

        foreach( Vector3 v in tempArray){
            Debug.DrawLine( 
                v + new Vector3(  rayLenght,  rayLenght),
                v + new Vector3( -rayLenght, -rayLenght),
                new Color( 1,0,0)
            );

            Debug.DrawLine( 
                v + new Vector3(  rayLenght, -rayLenght),
                v + new Vector3( -rayLenght, rayLenght),
                new Color( 1,0,0)
            );
        }
    }

    private float overTimeTimer = 1;

    public void ProcessMove(){
        timeOfMove += Time.deltaTime;
        float t = timeOfMove/timeOfGlide ;
        if( t > 1  ) 
        {
            if( overTimeTimer < 0) m_isOver = true;
            overTimeTimer -= Time.deltaTime;
            m_FloorDetector.Move( entityScript.velocity * Time.deltaTime); 
        }else{
            Vector3 nextPoint = new Vector3();
            nextPoint = PointInterpolation.DeCasteljausAlgorithm( t, infoPack);
            entityScript.velocity = (nextPoint - m_FloorDetector.GetComponent<Transform>().position).normalized * entityScript.glideSpeed;
            m_FloorDetector.Move( entityScript.velocity * Time.deltaTime); 
        }
    }

    public override void  Process(){

        DebugDraw();
        ProcessMove();
        ProcessStateOver();
    }

    public void ProcessStateOver(){
        if( m_wallDetector.isCollideWithLeftWall() && isLeftOriented() ){ 
            GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

            infoPack.isValid      = true;
            infoPack.stunDuration = 5f;
            m_isOver = true;
            m_nextState = new CzadStun( m_controllabledObject, infoPack);
        }else if( m_wallDetector.isCollideWithRightWall() && isRightOriented()){
            GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

            infoPack.isValid      = true;
            infoPack.stunDuration = 5f;
            m_isOver = true;
            m_nextState = new CzadStun( m_controllabledObject, infoPack);
        }

    }


}
