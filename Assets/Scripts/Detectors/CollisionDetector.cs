using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetector : MonoBehaviour, ICollisionFloorDetector
{

    protected const float DIR_LEFT  = -1;
    protected const float DIR_RIGHT =  1;
    protected const float DIR_UP    =  1;
    protected const float DIR_DOWN  = -1;

    [SerializeField] protected LayerMask m_collsionMask     = 0;
    [SerializeField] protected LayerMask m_selfCollsionMask = 0;
    [SerializeField] protected int verticalRayNumber   = 4 ;
    protected float verticalDistanceBeetweenRays       = 1;
    [SerializeField] protected int horizontalRayNumber = 4;
    protected float horizontalDistanceBeetweenRays     = 1;
    [SerializeField] protected float skinSize              = 15f;
    protected Vector2 transition          = new Vector2();
    protected CollisionInfo collisionInfo = new CollisionInfo();
    protected Borders borders             = new Borders();
    [SerializeField] public float maxClimbAngle   = 40.0f;
    [SerializeField] public float maxDescendAngle = 40.0f;

    [SerializeField] public bool autoGravityOn = false;
    protected BoxCollider2D m_boxCollider;


    public struct Borders{
        public float left, right, top, bottom;
    }

    protected struct CollisionInfo{
        public bool above, below, right, left;		
        
        public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector2 moveAmountOld;

        public int faceDir;

        public void Reset(){
            above = below = right = left = false;
            climbingSlope   = false;
            descendingSlope = false;
        }
    }

    void Awake() {
        m_boxCollider = GetComponent<BoxCollider2D>();
        CalculateBorders();
        CalculateDistanceBeetweenRay();
    }
    private void CalculateBorders(){
		Bounds bounds = m_boxCollider.bounds;
		bounds.Expand (skinSize * -2);

        borders.left    = bounds.min.x;
        borders.right   = bounds.max.x;
        borders.top     = bounds.max.y;
        borders.bottom  = bounds.min.y;
    }
    private void CalculateDistanceBeetweenRay(){
        horizontalDistanceBeetweenRays = (borders.top   - borders.bottom)/( horizontalRayNumber -1);
        verticalDistanceBeetweenRays   = (borders.right - borders.left)  /( verticalRayNumber   -1);
    }
    protected virtual void ProcessCollision(){
        ProcessSlopeDetection( Mathf.Sign(transition.x) );
        DescendSlope();
        ProcessCollisionHorizontal( Mathf.Sign(transition.x));
        ProcessCollisionVertical(   Mathf.Sign(transition.y));
        ProcessColisionOnTheSameLayer();
    }

    private void  ProcessColisionOnTheSameLayer(){
        float directionX = Mathf.Sign(transition.x);
        float directionY = Mathf.Sign(transition.y);


        Bounds bounds = m_boxCollider.bounds;
        bounds.Expand (1.1f);
        if( transition.x != 0) {
            float rayLenghtX  = Mathf.Abs (transition.x);
            for( int i = 0; i < horizontalRayNumber; i ++){

                Vector2 rayOrigin = new Vector2( (directionX == DIR_LEFT) ? 
                                                    bounds.min.x : 
                                                    bounds.max.x  ,
                                            bounds.min.y + i * horizontalDistanceBeetweenRays );

                RaycastHit2D hit = Physics2D.Raycast(
                    rayOrigin,
                    new Vector2(directionX, 0),
                    rayLenghtX,
                    m_selfCollsionMask
                );

                if( hit ){
                    rayLenghtX  = hit.distance;

                    collisionInfo.left  = (directionX == DIR_LEFT );
                    collisionInfo.right = (directionX == DIR_RIGHT);
                }

                Debug.DrawRay(
                    rayOrigin,
                    new Vector2(directionX, 0) * rayLenghtX,
                    new Color(1,0,0)
                );
                if( !collisionInfo.climbingSlope ){
                    transition.x = Mathf.Sign(transition.x) * ( rayLenghtX );
                }else{
                    if( Mathf.Abs(transition.x) > (rayLenghtX) ){
                        transition.x = Mathf.Sign(transition.x) * ( rayLenghtX );
                    }
                }
            }
        }

        if( transition.y != 0 ){
        float rayLenghtY  = Mathf.Abs (transition.y);

        for( int i = 0; i < verticalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( bounds.min.x + i * verticalDistanceBeetweenRays, 
                                            (directionY == DIR_DOWN) ? bounds.min.y : bounds.max.y  );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( 0, directionY),
                rayLenghtY,
                m_selfCollsionMask
            );

            if( hit ){
                rayLenghtY  = hit.distance;

                collisionInfo.below = (directionY == DIR_DOWN);
                collisionInfo.above = (directionY == DIR_UP  );
            }

            Debug.DrawRay(
                rayOrigin,
                new Vector2( 0, directionY) * rayLenghtY,
                new Color(0,1,0)
             );
        }
        if( !collisionInfo.climbingSlope )
        transition.y = Mathf.Sign(transition.y) * (rayLenghtY);
        }
    }

    protected void ProcessSlopeDetection(float directionX){
        float rayLenght  = Mathf.Abs (transition.x) + skinSize;
        Vector2 rayOrigin = new Vector2( (directionX == DIR_LEFT) ? 
                                                    borders.left : 
                                                    borders.right,
                                          borders.bottom - skinSize);

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2(directionX, 0),
                rayLenght,
                m_collsionMask
            );

            if( hit ){
                if( hit.distance == 0.0f) return;
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up); 
                if( slopeAngle == 0 ) return;
                if( slopeAngle < maxClimbAngle){
					if (collisionInfo.descendingSlope) {
						collisionInfo.descendingSlope = false;
//						transition = collisionInfo.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisionInfo.slopeAngleOld) {
						distanceToSlopeStart = hit.distance- skinSize;
                        transition.x -= distanceToSlopeStart * directionX;
					}
                	float moveDistance   = Mathf.Abs (transition.x);
		            float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		            if (transition.y <= climbVelocityY) {
		            	transition.y = climbVelocityY;
		            	transition.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * 
                                       moveDistance * Mathf.Sign(transition.x);
                        
                        collisionInfo.below         = true;
		            	collisionInfo.climbingSlope = true;
                        collisionInfo.slopeAngleOld = collisionInfo.slopeAngle;
		            	collisionInfo.slopeAngle    = slopeAngle;
		            }
				    transition.x     += distanceToSlopeStart * directionX;
		            transition.y = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * 
                                   Mathf.Abs(transition.x);
                    transition.x = transition.y / Mathf.Tan(slopeAngle * 
                                    Mathf.Deg2Rad) * Mathf.Sign(transition.x);
                }
            }

        Debug.DrawRay(
            rayOrigin,
            new Vector2(directionX * 4, 0) * rayLenght,
            new Color(0,0,0));
    }

	protected void DescendSlope() {
        if( collisionInfo.climbingSlope ) return;
		float directionX = Mathf.Sign (transition.x);
		
        Vector2 rayOrigin = new Vector2( (directionX == DIR_RIGHT) ? 
                                                    borders.left : 
                                                    borders.right,
                                          borders.bottom - skinSize);
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, m_collsionMask);

		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign(hit.normal.x) == directionX) {
					if (hit.distance - skinSize <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(transition.x)) {
						float moveDistance = Mathf.Abs(transition.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						transition.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (transition.x);
						transition.y -= descendVelocityY;

						collisionInfo.slopeAngle = slopeAngle;
						collisionInfo.descendingSlope = true;
						collisionInfo.below = true;
					}
				}
			}
		}
        Debug.DrawRay(
            rayOrigin,
            new Vector2(-4, 0),
            new Color(0,0,0)
        );
	}


    public Vector2 GetSlopeAngle(){
        Vector2 rayOrigin = new Vector2( //(collisionInfo.faceDir == DIR_LEFT) ? 
                                          borders.left + ( borders.right - borders.left)/2.0f,
                                          borders.bottom);

        RaycastHit2D hit = Physics2D.Raycast(
            rayOrigin,
            new Vector2(0, -1),
            200,
            m_collsionMask
        );

        Vector2 rayOriginDescend = new Vector2( (collisionInfo.faceDir == DIR_LEFT) ? 
                                                borders.left : borders.right,
                                                borders.bottom);

        RaycastHit2D hit2 = Physics2D.Raycast (rayOriginDescend, new Vector2( collisionInfo.faceDir, 0 ), 200, m_collsionMask);


        Debug.DrawRay(
            rayOriginDescend,
            new Vector2(200 * collisionInfo.faceDir, -0),
            new Color(1,1,1)
        );

        Debug.DrawRay(
            rayOrigin,
            new Vector2(0, -200),
            new Color(0,0,0)
        );

        float newNormal  = Vector2.Angle(hit.normal,  Vector2.up); 
        float slopeAngle2 = Vector2.Angle(hit2.normal, Vector2.up);
    //    if( hit.distance == 0) return 0;

     //   if( Mathf.Abs(slopeAngle2 - slopeAngle) > 5 ){ 
    //        slopeAngle *= -1;
    //    }

    //    if( collisionInfo.descendingSlope ){
    //        slopeAngle *= -1;
    //    }

    //    if( slopeAngle > 80 ) slopeAngle -= 90;

        return hit.normal;
        
    }


    protected void ProcessCollisionHorizontal( float directionX){
        if( transition.x == 0) return;
        float rayLenght  = Mathf.Abs (transition.x) + skinSize;

        for( int i = 0; i < horizontalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( (directionX == DIR_LEFT) ? 
                                                    borders.left : 
                                                    borders.right  ,
                                             borders.bottom + i * horizontalDistanceBeetweenRays );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2(directionX, 0),
                rayLenght,
                m_collsionMask
            );

            if( hit ){

                rayLenght  = hit.distance;

                collisionInfo.left  = (directionX == DIR_LEFT );
                collisionInfo.right = (directionX == DIR_RIGHT);
            }
            
            Debug.DrawRay(
                rayOrigin,
                new Vector2(directionX, 0) * rayLenght,
                new Color(1,0,0)
             );
        }
        if( !collisionInfo.climbingSlope ){
            transition.x = Mathf.Sign(transition.x) * ( rayLenght -skinSize);
        }else{
            if( Mathf.Abs(transition.x) > (rayLenght-skinSize) ){
                transition.x = Mathf.Sign(transition.x) * ( rayLenght -skinSize);
            }
        }
            
    }

    protected void ProcessCollisionVertical( float directionY ){
        if( transition.y == 0) return;
        float rayLenght  = Mathf.Abs (transition.y) + skinSize;

        for( int i = 0; i < verticalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( borders.left + i * verticalDistanceBeetweenRays, 
                                            (directionY == DIR_DOWN) ? borders.bottom : borders.top  );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( 0, directionY),
                rayLenght,
                m_collsionMask
            );

            if( hit ){
                rayLenght  = hit.distance;

                collisionInfo.below = (directionY == DIR_DOWN);
                collisionInfo.above = (directionY == DIR_UP  );
            }

            Debug.DrawRay(
                rayOrigin,
                new Vector2( 0, directionY) * rayLenght,
                new Color(0,1,0)
             );
        }
        if( !collisionInfo.climbingSlope )
        transition.y = Mathf.Sign(transition.y) * (rayLenght-skinSize);
    }

	protected void VerticalCollisions() {
        if( transition.y == 0) return;
		float directionY = Mathf.Sign (transition.y);
		float rayLength = Mathf.Abs (transition.y) + skinSize;

		for (int i = 0; i < verticalRayNumber; i ++) {

            Vector2 rayOrigin = new Vector2( borders.left + i * verticalDistanceBeetweenRays, 
                                            (directionY == DIR_DOWN) ? 
                                                    borders.bottom : 
                                                    borders.top  );

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, m_collsionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);

			if (hit) {
				transition.y = (hit.distance - skinSize) * directionY;
				rayLength = hit.distance;

				collisionInfo.below = directionY == -1;
				collisionInfo.above = directionY == 1;
			}
		}
	}

    public Vector2 GetTransition(){
        return transition;
    }

  //  public float GravityForce = PlayerUtils.GravityForce;

    protected virtual void ProcessAutoGravity(){
        if( autoGravityOn ){
            if( !collisionInfo.below){
                transition.y = -PlayerUtils.GravityForce * Time.deltaTime;
                Move( transition );
            }
        }
    }

    void Update()
    {
        CalculateBorders();
        ProcessAutoGravity();

        if( LOCKED_BY_FORCE ){
            CommonValues.PlayerVelocity = new Vector2();
            transition = new Vector2();
        }

        Debug.DrawRay(
            GetComponent<Transform>().position,
            new Vector2(1, 0) * 500,
            new Color(1,0,0));


    }

    protected virtual void ResetCollisionInfo(){
        collisionInfo.Reset();
    }

    public virtual void Move( Vector2 velocity ){
        if(LOCKED_BY_FORCE) return;
        transition = velocity;
        if( transition.x != 0) collisionInfo.faceDir = (int) Mathf.Sign(transition.x) ;
        ResetCollisionInfo();
        CalculateDistanceBeetweenRay();
        CalculateBorders();
        ProcessCollision();
        transform.Translate( transition );
        transition = new Vector2(0,0);
    }

    public virtual void Move( float x, float y ){
        if(LOCKED_BY_FORCE) return;
        transition = new Vector2(x, y);
        if( transition.x != 0) collisionInfo.faceDir = (int) Mathf.Sign(transition.x) ;
        ResetCollisionInfo();
        CalculateDistanceBeetweenRay();
        CalculateBorders();
        ProcessCollision();
        transform.Translate( transition );
        transition = new Vector2(0,0);
    }

    public virtual void Move( Vector2 velocity, bool updateFaceDir ){
        if(LOCKED_BY_FORCE) return;
        transition = velocity;
        if( transition.x != 0 && updateFaceDir) collisionInfo.faceDir = (int) Mathf.Sign(transition.x) ;
        ResetCollisionInfo();
        CalculateDistanceBeetweenRay();
        CalculateBorders();
        ProcessCollision();
        transform.Translate( transition );
        transition = new Vector2(0,0);
    }


    public GlobalUtils.Direction GetCurrentDirection(){
        return (GlobalUtils.Direction) collisionInfo.faceDir;
    }

    public void CheatMove( Vector2 velocity){
        transform.Translate( velocity );
    }

    public bool isOnCelling(){
        return collisionInfo.above;
    }

    public bool isOnGround(){
        return collisionInfo.below;
    }

    private bool LOCKED_BY_FORCE = false;

    public void setLock( bool foceStatus_yeyIamJedi){
        LOCKED_BY_FORCE = foceStatus_yeyIamJedi;
    }

}
