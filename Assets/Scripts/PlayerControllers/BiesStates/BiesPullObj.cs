using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesPullObj : BaseState
{
    private bool isFaceingLeft = false;
    Vector2 pullForce = new Vector2(0,0);
    Transform m_moveable = null;

    private bool lockMove = false;

    float distanceFromObject;

    public BiesPullObj( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

        isFaceingLeft = dir == GlobalUtils.Direction.Left;
        name = "BiesPullObj";
        m_dir = dir;
        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);

        m_moveable  = m_detector.GetPullableObject();

        pullForce.x = -BiesUtils.PlayerSpeed * (int)dir;
        pullForce.y = -BiesUtils.GravityForce * Time.deltaTime;
        pullForce *= m_moveable.GetComponent<CollisionDetectorMovable>().PullFriction;

        distanceFromObject = Vector3.Distance( m_controllabledObject.transform.position,
                                               m_moveable.transform.position );
    }

    public override void UpdateDirection(){
        m_controllabledObject.GetComponent<Player>().animationNode.position = 
            Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                m_controllabledObject.transform.position, ref animationVel, m_smoothTime);
    }

    public override void OnExit(){
        velocity = new Vector2(0,0);
        m_detector.Move(velocity);
    }
    public override void Process(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) ){
            m_isOver = true;
        } 

        if( !isFaceingLeft && m_detector.isCollideWithLeftWall() ) lockMove = true;
        if( isFaceingLeft && m_detector.isCollideWithRightWall() ) lockMove = true;

        if( m_moveable && !lockMove ){

            m_detector.Move( pullForce* Time.deltaTime );

            Vector3 oldMoveablePosition = m_moveable.transform.position;

            Vector2 adaptedPullForce = pullForce * Time.deltaTime;
            adaptedPullForce.y       = m_detector.GetTransition().y;

            m_moveable.GetComponent<CollisionDetector>().Move( adaptedPullForce );

            float currentDistanceFromObject = Vector3.Distance( m_controllabledObject.transform.position,
                                                  m_moveable.transform.position );

            if( Mathf.Abs(currentDistanceFromObject - distanceFromObject) > 2 ){
                m_moveable.transform.position = 
                    (m_moveable.transform.position - oldMoveablePosition) * 
                    currentDistanceFromObject/distanceFromObject + 
                    oldMoveablePosition; 
            }
        }
    }

    public override void HandleInput(){
        if( !PlayerInput.isSpecialKeyHold() ) { 
            m_isOver = true;
        }else if( !PlayerInput.isMoveRightKeyHold() && !PlayerInput.isMoveLeftKeyHold() ){
            m_isOver = true;
        }
    }
}
