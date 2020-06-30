using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLedgeClimb : BaseState
{

    private float timeToEnd;
    private AnimationTransition m_transition;

    private float targetClimbHight = 0;
    private float targetStayHightY  = 0; 
    private float targetStayHightX = 0;

    public CatLedgeClimb( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        name = "CatLedgeClimb";
        CommonValues.PlayerVelocity = new Vector2(0,0);
        m_dir = dir;
        SetUpRotation();
        PlayerFallOfWallHelper.ResetCounter();
        SetUpVariables();

    }

    private void SetUpVariables(){ //TODO Calculation of target position will require slight update;
        BoxCollider2D ledgeBox  = m_detector.GetClimbableObject().GetComponent<BoxCollider2D>();
        BoxCollider2D playerBox = m_detector.GetComponent<BoxCollider2D>();
        Vector2 pos             = m_detector.GetComponent<Transform>().position;

        targetClimbHight = Mathf.Max( ledgeBox.bounds.max.y, ledgeBox.bounds.min.y);
        targetStayHightY = targetClimbHight;//+ ( playerBox.bounds.max.y - playerBox.bounds.min.x )/2.0f;// + 900.0f;

        GlobalUtils.Direction obstacleDir = ( playerBox.bounds.max.x > ledgeBox.bounds.max.x ) ? 
                                                    GlobalUtils.Direction.Left : 
                                                    GlobalUtils.Direction.Right;
        targetStayHightX = pos.x + 321f * (int)obstacleDir;

        Vector2 velocityS = new Vector2( 0, targetClimbHight - pos.y);
        m_detector.CheatMove( velocityS );
    }

    protected override void UpdateFloorAligment(){
        float distanceToRight  = Vector3.Distance( m_detector.GetClimbableObject().transform.up, m_animator.transform.up );  
        //= Vector3.Angle( m_detector.GetClimbableObject().transform.up,    m_animator.transform.up);
        float distanceToUp     = Vector3.Distance( m_detector.GetClimbableObject().transform.right, m_animator.transform.up ); 
        //= Vector3.Angle( m_detector.GetClimbableObject().transform.right, m_animator.transform.up);

        m_animator.transform.up = ( distanceToRight < distanceToUp ) ? 
                                    m_detector.GetClimbableObject().transform.up : 
                                    m_detector.GetClimbableObject().transform.right;        
    }

    private void SetUpRotation(){
        rotationAngle = isLeftOriented() ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);
    }

    protected override void  SetUpAnimation(){
        m_animator.SetTrigger("CatClimb");
        timeToEnd = getAnimationLenght("CatLedgeClimb");

        m_transition = m_controllabledObject.
                       GetComponent<Player>().animationNode.
                       GetComponent<AnimationTransition>();
    }

    public override void OnExit(){ }

    protected override void UpdateDirection(){}

    bool doITFuckingOnce = true;

    public override void Process(){

        if( doITFuckingOnce){
            Vector2 pos = m_detector.GetComponent<Transform>().position;
            Vector2 velocityS = new Vector2( 0, targetClimbHight - pos.y);
            m_detector.CheatMove( velocityS );
            doITFuckingOnce = false;
        }
       
        PlayerFallOfWallHelper.ResetCounter();

        timeToEnd -= Time.deltaTime;
        if( timeToEnd < 0 ) {
            Vector2 pos = m_detector.GetComponent<Transform>().position;

            pos = new Vector2( targetStayHightX, targetStayHightY +100 );

            m_detector.GetComponent<Transform>().position = pos;

      //      Debug.Log( m_detector.GetComponent<Transform>().position.ToString() + ":::" + 
     //       new Vector2 ( targetStayHightX ,targetStayHightY +100 ) );


            if( m_detector.isOnGround() ){
                CommonValues.PlayerVelocity.y = 0;
                m_isOver = true;
                m_nextState = new CatMove(m_controllabledObject, m_dir);
            }else{

                    Vector2 pos2 = m_detector.GetComponent<Transform>().position;

                    pos = new Vector2( targetStayHightX, targetStayHightY +100 );

                    m_detector.GetComponent<Transform>().position = pos;

                 CommonValues.PlayerVelocity.y += -CatUtils.GravityForce * Time.deltaTime;
                 m_detector.Move(CommonValues.PlayerVelocity * Time.deltaTime);
            }

        }
    }

    public override void HandleInput(){}
}
