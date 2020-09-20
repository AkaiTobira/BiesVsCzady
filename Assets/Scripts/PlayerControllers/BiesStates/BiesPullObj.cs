using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesPullObj : PlayerBaseState
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
    //    m_animator.GetComponent<SoundAmbientStopable>().PlayAmbient(0);
        isFaceingLeft = dir == GlobalUtils.Direction.Left;
        name = "BiesPullObj";
        m_dir = dir;
        rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);

        m_moveable  = m_ObjectInteractionDetector.GetPullableObject();

        pullForce.x = -BiesUtils.PlayerSpeed * (int)dir;
        pullForce.y = -BiesUtils.GravityForce * Time.deltaTime;
        pullForce *= m_moveable.GetComponent<CollisionDetectorMovable>().PullFriction;

        distanceFromObject = Vector3.Distance( m_controllabledObject.transform.position,
                                               m_moveable.transform.position );

        distanceToFixAnimation = new Vector3(30, 6 , 0);
    }

    protected override void UpdateDirection(){}

    public override void OnExit(){
        m_animator.SetBool("isPulling", !m_isOver);
        CommonValues.PlayerVelocity = new Vector2(0,0);
        m_FloorDetector.Move(CommonValues.PlayerVelocity);
    //    m_animator.GetComponent<SoundAmbientStopable>().StopAmbient(0);
    }
    public override void Process(){
        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) ){
            m_isOver = true;
        } 

        if( !isFaceingLeft && m_WallDetector.isCollideWithLeftWall() ) lockMove = true;
        if( isFaceingLeft && m_WallDetector.isCollideWithRightWall() ) lockMove = true;

        if( m_moveable && !lockMove ){

            m_FloorDetector.Move( pullForce* Time.deltaTime );

            Vector3 oldMoveablePosition = m_moveable.transform.position;

            Vector2 adaptedPullForce = pullForce * Time.deltaTime;
            adaptedPullForce.y       = m_FloorDetector.GetTransition().y;

            m_moveable.GetComponent<CollisionDetector>().Move( adaptedPullForce );

            float currentDistanceFromObject = Vector3.Distance( m_controllabledObject.transform.position,
                                                  m_moveable.transform.position );

            if( Mathf.Abs(currentDistanceFromObject - distanceFromObject) > 2 ){
                m_moveable.transform.position = 
                    (m_moveable.transform.position - oldMoveablePosition) * 
                    currentDistanceFromObject/distanceFromObject + 
                    oldMoveablePosition; 
            }
            
            m_animator.SetBool("isPulling", !m_isOver);
        }
    }

    public override void HandleInput(){
        if( !PlayerInput.isSpecialKeyHold() ) { 
            m_isOver = true;
        }else if( isLeftOriented() && !PlayerInput.isMoveRightKeyHold() ){
            m_isOver = true;
            pullForce.x *= -1;
            m_FloorDetector.Move( pullForce * Time.deltaTime );
        }else if( isRightOriented() && !PlayerInput.isMoveLeftKeyHold() ){
            m_isOver = true;
            pullForce.x *= -1;
            m_FloorDetector.Move( pullForce * Time.deltaTime );
        }
    }

    public override string GetTutorialAdvice(){
        return "";
    }

}
