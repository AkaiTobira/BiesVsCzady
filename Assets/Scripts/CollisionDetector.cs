using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetector : MonoBehaviour
{

    protected const float DIR_LEFT  = -1;
    protected const float DIR_RIGHT =  1;
    protected const float DIR_UP    =  1;
    protected const float DIR_DOWN  = -1;

    [SerializeField] protected LayerMask m_collsionMask = 0;
    [SerializeField] protected int verticalRayNumber   = 4 ;
    protected float verticalDistanceBeetweenRays       = 1;
    [SerializeField] protected int horizontalRayNumber = 4;
    protected float horizontalDistanceBeetweenRays     = 1;
    [SerializeField] protected float skinSize              = 15f;
    [SerializeField] protected Vector2 transition          = new Vector2();
    protected CollisionInfo collisionInfo = new CollisionInfo();
    protected Borders borders             = new Borders();

    [SerializeField] public Text DebuggText = null;
    [SerializeField] public Text DebuggText2 = null;
    [SerializeField] public Text DebuggText3 = null;

    [SerializeField] public float maxClimbAngle = 40.0f;

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

    BoxCollider2D m_boxCollider;

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
    }

    protected void ProcessSlopeDetection(float directionX){
        //if( transition.x == 0) return;
        float rayLenght  = Mathf.Abs (transition.x) + skinSize;
        Vector2 rayOrigin = new Vector2( (directionX == DIR_LEFT) ? 
                                                    borders.left : 
                                                    borders.right ,
                                          borders.bottom - skinSize );


            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2(directionX, 0),
                rayLenght,
                m_collsionMask
            );

            if( hit ){
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if( slopeAngle < maxClimbAngle){
                    Debug.Log("Slope Detected" + slopeAngle.ToString());

					float distanceToSlopeStart = 0;
					if (slopeAngle != collisionInfo.slopeAngleOld) {
						distanceToSlopeStart = hit.distance- skinSize;
                        transition.x -= distanceToSlopeStart * directionX;
					}
                    DebuggText3.text = transition.ToString();
                	float moveDistance   = Mathf.Abs (transition.x);
		            float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
                    DebuggText3.text += " : " + moveDistance.ToString();
                    DebuggText3.text += " : " + climbVelocityY.ToString();
		            if (transition.y <= climbVelocityY) {
		            	transition.y = climbVelocityY;
		            	transition.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * 
                                       moveDistance * Mathf.Sign(transition.x);
                        
                        collisionInfo.below         = true;
		            	collisionInfo.climbingSlope = true;
		            	collisionInfo.slopeAngle    = slopeAngle;
		            }
				    transition.x += distanceToSlopeStart * directionX;     
                    DebuggText3.text += " : " + transition.x.ToString();
		            transition.y = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * 
                                   Mathf.Abs(transition.x);
                    DebuggText3.text += " : " + transition.y.ToString();
                    transition.x = transition.y / Mathf.Tan(slopeAngle * 
                                    Mathf.Deg2Rad) * Mathf.Sign(transition.x);
                    DebuggText3.text += " : " + transition.x.ToString();
                }
            }
                Debug.DrawRay(
                rayOrigin,
                new Vector2(directionX * 10, 0) * rayLenght,
                new Color(0,0,0)
             );
    }

    protected void ProcessCollisionHorizontal( float directionX ){
        if( transition.x == 0) return;
        float rayLenght  = Mathf.Abs (transition.x) + skinSize;


	//	if (Mathf.Abs(transition.x) < skinSize) {
	//		rayLenght = 2*skinSize;
	//	}

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

            //    if (hit.distance == 0) {
			//		continue;
			//	}

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
        transition.y = Mathf.Sign(transition.y) * (rayLenght-skinSize);//Mathf.Max( rayLenght -skinSize, 0.0f );
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

[SerializeField] float i = 0.0f;
[SerializeField] float j = 0.0f;
[SerializeField] float k = 0.0f;

private void printForDebug(){
        float rayLenght  = 10;
		float directionY = Mathf.Sign (transition.y);
        float directionX = Mathf.Sign (transition.x);

		for (int i = 0; i < verticalRayNumber; i ++) {

            Vector2 rayOrigin = new Vector2( borders.left + i * verticalDistanceBeetweenRays, 
                                            (directionY == DIR_DOWN) ? 
                                                borders.bottom + skinSize: 
                                                borders.top    - skinSize );

			Debug.DrawRay(rayOrigin, Vector2.up * rayLenght * directionY,Color.red);
		}

        for( int i = 0; i < horizontalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( (directionX == DIR_LEFT) ? 
                                             borders.left + j: borders.right +k ,
                                             borders.bottom + i * horizontalDistanceBeetweenRays );

            Debug.DrawRay(
                rayOrigin,
                Vector2.right * directionY* rayLenght,
                Color.red
             );
        }
	}



    void Update()
    {

        CalculateBorders();
    //    CalculateDistanceBeetweenRay();
    //    printForDebug();
    //    ResetCollisionInfo();
    //    CalculateBorders();
     //   ProcessCollision();        
    }

    protected virtual void ResetCollisionInfo(){
        collisionInfo.Reset();
    }
    public virtual void Move( Vector2 velocity ){
        DebuggText.text = velocity.ToString();
        transition = velocity;        
        if( transition.x != 0) collisionInfo.faceDir = (int) Mathf.Sign(transition.x) ;
        ResetCollisionInfo();
        CalculateDistanceBeetweenRay();
        CalculateBorders();
        ProcessCollision();
        transform.Translate( transition );
        DebuggText2.text = transition.ToString();
    }

    public void CheatMove( Vector2 velocity){
        transform.Translate( velocity );
    }

}
