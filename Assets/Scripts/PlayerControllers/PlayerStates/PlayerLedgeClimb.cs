using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimb : PlayerBaseState
{

    protected float timeToEnd;
    protected AnimationTransition m_transition;

    protected float targetClimbHight = 0;
    protected float targetStayHightY  = 0; 
    protected float targetStayHightX = 0;

    protected GlobalUtils.Direction forSureDirection;

    public PlayerLedgeClimb( GameObject controllable, GlobalUtils.Direction dir , float someVariable) : base( controllable ) {
        CommonValues.PlayerVelocity = new Vector2(0,0);
        forSureDirection = dir;
        m_dir = dir;
        SetUpRotation();
        PlayerFallOfWallHelper.ResetCounter();
        SetUpVariables(someVariable);
    }

    private float CalculateHighOfLedge(BoxCollider2D ledgeBox){

        Transform interactable = m_ObjectInteractionDetector.GetClimbableObject();
        Vector2[] Vectors = new Vector2[4];

        Vectors[0] = interactable.TransformPoint(ledgeBox.offset + new Vector2(-ledgeBox.size.x,  ledgeBox.size.y) * 0.5f);
        Vectors[1] = interactable.TransformPoint(ledgeBox.offset + new Vector2( ledgeBox.size.x,  ledgeBox.size.y) * 0.5f);
        Vectors[2] = interactable.TransformPoint(ledgeBox.offset + new Vector2(-ledgeBox.size.x, -ledgeBox.size.y) * 0.5f);
        Vectors[3] = interactable.TransformPoint(ledgeBox.offset + new Vector2( ledgeBox.size.x, -ledgeBox.size.y) * 0.5f);

        Vector2 closestOne  = new Vector2(-10000000,-10000000);
        Vector2 closestOne2 = new Vector2(-10000000,-10000000);

        foreach( Vector2 v in Vectors){
            if( Vector2.Distance( m_FloorDetector.GetComponent<Transform>().position, v) < 
                Vector2.Distance( m_FloorDetector.GetComponent<Transform>().position, closestOne) ){
                    closestOne2 = closestOne;
                    closestOne  = v;
            }else if(
                Vector2.Distance( m_FloorDetector.GetComponent<Transform>().position, v) < 
                Vector2.Distance( m_FloorDetector.GetComponent<Transform>().position, closestOne2) ){
                    closestOne2 = v;
            }
        }

        return Mathf.Max( closestOne.y, closestOne.y);
    }

    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    private void SetUpVariables( float sommeVariable){ //TODO Calculation of target position will require slight update;
        
        BoxCollider2D ledgeBox  = m_ObjectInteractionDetector.GetClimbableObject().GetComponent<BoxCollider2D>();
        BoxCollider2D playerBox = m_FloorDetector.GetComponent<BoxCollider2D>();
        Vector2 pos             = m_FloorDetector.GetComponent<Transform>().position;

        targetClimbHight = CalculateHighOfLedge(ledgeBox);
        targetStayHightY = targetClimbHight + 30;//+ ( playerBox.bounds.max.y - playerBox.bounds.min.x )/2.0f;// + 900.0f;

        GlobalUtils.Direction obstacleDir = ( playerBox.bounds.max.x > ledgeBox.bounds.max.x ) ? 
                                                    GlobalUtils.Direction.Left : 
                                                    GlobalUtils.Direction.Right;
        targetStayHightX = pos.x + sommeVariable * (int)obstacleDir;

        Vector2 velocityS = new Vector2( 0, targetClimbHight - pos.y);
        m_FloorDetector.CheatMove( velocityS );
    //    m_FloorDetector.Move( new Vector2(0.001f * (int)m_dir, 0.001f)); 
    }

    protected Vector2 shiftValue;

    public override void Process(){

        Vector2 pos     = m_FloorDetector.GetComponent<Transform>().position;
        Vector2 pos2    = shiftValue;
        float rayLenght = 50.0f;

        Debug.DrawLine(pos + pos2 + new Vector2( rayLenght,  rayLenght), pos + pos2 + new Vector2( -rayLenght, -rayLenght), new Color(0,1,0));
        Debug.DrawLine(pos + pos2 + new Vector2( rayLenght, -rayLenght), pos + pos2 + new Vector2( -rayLenght,  rayLenght), new Color(0,1,0));

        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0 ){
            PlayerFallHelper.FallRequirementsMeet( true );
            m_FloorDetector.CheatMove( shiftValue );
            m_isOver = true;
        }
    }

    protected override void UpdateDirection(){
        m_dir = forSureDirection;
    }

/*
    public override void OnExit(){ }

    protected override void UpdateDirection(){}

    bool doITFuckingOnce = true;

    public override void Process(){

        if( doITFuckingOnce){
            Vector2 pos = m_FloorDetector.GetComponent<Transform>().position;
            Vector2 velocityS = new Vector2( 0, targetClimbHight - pos.y);
            m_FloorDetector.CheatMove( velocityS );
            doITFuckingOnce = false;
        }
       
        PlayerFallOfWallHelper.ResetCounter();

        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0 ) {
            Vector2 pos = m_FloorDetector.GetComponent<Transform>().position;

            pos = new Vector2( targetStayHightX, targetStayHightY +100 );

            m_FloorDetector.GetComponent<Transform>().position = pos;

      //      Debug.Log( m_FloorDetector.GetComponent<Transform>().position.ToString() + ":::" + 
     //       new Vector2 ( targetStayHightX ,targetStayHightY +100 ) );


            if( m_FloorDetector.isOnGround() ){
                CommonValues.PlayerVelocity.y = 0;
                m_isOver = true;
                m_nextState = new CatMove(m_controllabledObject, m_dir);
            }else{

                    Vector2 pos2 = m_FloorDetector.GetComponent<Transform>().position;

                    pos = new Vector2( targetStayHightX, targetStayHightY +100 );

                    m_FloorDetector.GetComponent<Transform>().position = pos;

                 CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
                 m_FloorDetector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
            }

        }
    }
*/
    public override void HandleInput(){}
}
