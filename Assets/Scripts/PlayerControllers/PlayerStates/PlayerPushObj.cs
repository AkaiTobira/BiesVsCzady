using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPushObj : BaseState
{
    private bool isMovingLeft = false;
    Vector2 pushForce = new Vector2(0,0);
    Transform m_moveable = null;
    
    float distanceFromObject;

    public PlayerPushObj( GameObject controllable, PlayerUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP

        name = "PlayerPushObj";
        isMovingLeft = dir == PlayerUtils.Direction.Left;
        m_dir = dir;

        rotationAngle = ( m_dir == PlayerUtils.Direction.Left) ? 180 :0 ; 
        m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);

        m_moveable    = m_detector.GetPullableObject();
        pushForce.x   = PlayerUtils.PlayerSpeed * (int)dir * 
                        m_moveable.GetComponent<CollisionDetectorMovable>().PushFriction;

        distanceFromObject = Vector3.Distance(  m_controllabledObject.transform.position,
                                                m_moveable.transform.position );

    }

    public override void OnExit(){
        velocity = new Vector2(0,0);
        m_detector.Move(velocity);
    }

    public override void UpdateDirection(){
        m_controllabledObject.GetComponent<Player>().animationNode.position = 
            Vector3.SmoothDamp( m_controllabledObject.GetComponent<Player>().animationNode.position, 
                                m_controllabledObject.transform.position, ref animationVel, m_smoothTime);
    }

    public override void Process(){
        if( PlayerFallHelper.FallRequirementsMeet( m_detector.isOnGround()) )m_isOver = true;

        if( m_moveable ){
            Vector3 currentPullableObjectPosition = m_moveable.transform.position;
            m_moveable.GetComponent<CollisionDetector>().Move(pushForce* Time.deltaTime);
            Vector2 playerMoveVector = pushForce + 
                                       new Vector2( 0,-PlayerUtils.GravityForce * Time.deltaTime );
            m_detector.Move( playerMoveVector* Time.deltaTime );
            float updatedDistanceFromObject = Vector3.Distance( m_controllabledObject.transform.position,
                                                                m_moveable.transform.position );

            m_moveable.transform.position = 
                ( m_moveable.transform.position - currentPullableObjectPosition )*
                distanceFromObject/updatedDistanceFromObject +
                currentPullableObjectPosition;
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
