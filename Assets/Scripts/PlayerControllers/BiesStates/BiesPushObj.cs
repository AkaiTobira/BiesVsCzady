using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesPushObj : PlayerBaseState
{
    private bool isMovingLeft = false;
    Vector2 pushForce = new Vector2(0,0);
    Transform m_moveable = null;
    
    float distanceFromObject;

    private ICollisionWallDetector m_wallDetector;

    private FMOD.Studio.EventInstance instance;
    public BiesPushObj( GameObject controllable, GlobalUtils.Direction dir) : base( controllable ) {
        // play change direction animation;
        // at end of animation call :
        // TEMP


        instance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enviro/push object");
        instance.start();

        //m_animator.GetComponent<SoundSFX>().PlayLoopedSFX(0);
        name = "BiesPushObj";
        isMovingLeft = dir == GlobalUtils.Direction.Left;
        m_dir = dir;

        //rotationAngle = ( m_dir == GlobalUtils.Direction.Left) ? 180 :0 ; 
        // m_controllabledObject.GetComponent<Player>().animationNode.eulerAngles = new Vector3( 0, rotationAngle, slopeAngle);

        m_moveable    = m_ObjectInteractionDetector.GetPullableObject();
        pushForce.x   = BiesUtils.PlayerSpeed * (int)dir * 
                        m_moveable.GetComponent<CollisionDetectorMovable>().PushFriction;

        distanceFromObject = Vector3.Distance(  m_controllabledObject.transform.position,
                                                m_moveable.transform.position );


       // distanceToFixAnimation = new Vector3(30, 6 , 0);

        m_wallDetector = m_moveable.GetComponent<ICollisionWallDetector>();
    }

    public override void OnExit(){
        //m_animator.SetBool("isPushing", !m_isOver);
        CommonValues.PlayerVelocity = new Vector2(0,0);
        m_FloorDetector.Move(CommonValues.PlayerVelocity);
        //m_animator.GetComponent<SoundSFX>().StopLoopedSFX(0);
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
        
        m_animator.SetBool("isPushing", false);
    }

    protected override void UpdateDirection(){}

    public override void Process(){
        

        if( m_moveable ){
            Vector3 currentPullableObjectPosition = m_moveable.transform.position;
            m_moveable.GetComponent<CollisionDetector>().Move(pushForce* Time.deltaTime);
            Vector2 playerMoveVector = pushForce + 
                                       new Vector2( 0,-BiesUtils.GravityForce * Time.deltaTime );
            m_FloorDetector.Move( playerMoveVector* Time.deltaTime );
            float updatedDistanceFromObject = Vector3.Distance( m_controllabledObject.transform.position,
                                                                m_moveable.transform.position );

            m_moveable.transform.position = 
                ( m_moveable.transform.position - currentPullableObjectPosition )*
                distanceFromObject/updatedDistanceFromObject +
                currentPullableObjectPosition;
        }

        if( PlayerFallHelper.FallRequirementsMeet( m_FloorDetector.isOnGround()) ) {
            m_isOver = true;
        }else if( isRightOriented() && m_wallDetector.isCollideWithRightWall() ){
            m_isOver = true;
        }else if( isLeftOriented()  && m_wallDetector.isCollideWithLeftWall()  ){
            m_isOver = true;
        }else if( (m_moveable.transform.position - m_FloorDetector.GetComponent<Transform>().transform.position).magnitude > 100 ){
            m_isOver = true;
        }

        m_animator.SetBool("isPushing", !m_isOver);
    }

    public override void HandleInput(){
        if( !PlayerInput.isSpecialKeyHold() ) { 
            m_isOver = true;
        }else if( isRightOriented() && !PlayerInput.isMoveRightKeyHold() ){
            m_isOver = true;
        }else if(  isLeftOriented() && !PlayerInput.isMoveLeftKeyHold() ){
            m_isOver = true;
        }
    }

    public override string GetTutorialAdvice(){
        return "";
    }
}
