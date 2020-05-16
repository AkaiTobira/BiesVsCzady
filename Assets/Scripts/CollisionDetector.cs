using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public struct Borders{
        public float left, right, top, bottom;
    }

    protected struct CollisionInfo{
        public bool above, below, right, left;
        public int faceDir;

        public void Reset(){
            above = below = right = left = false;
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
    protected void ProcessCollisionHorizontal( float directionX ){
        if( transition.x == 0) return;
        float rayLenght  = Mathf.Abs (transition.x) + skinSize;

        for( int i = 0; i < horizontalRayNumber; i ++){
            Vector2 rayOrigin = new Vector2( (directionX == DIR_LEFT) ? borders.left : borders.right  ,
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
        transition.x = Mathf.Sign(transition.x) * ( rayLenght -skinSize);
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
        transition.y = Mathf.Sign(transition.y) * (rayLenght-skinSize);//Mathf.Max( rayLenght -skinSize, 0.0f );
    }

	protected void VerticalCollisions() {
        if( transition.y == 0) return;
		float directionY = Mathf.Sign (transition.y);
		float rayLength = Mathf.Abs (transition.y) + skinSize;

		for (int i = 0; i < verticalRayNumber; i ++) {

            Vector2 rayOrigin = new Vector2( borders.left + i * verticalDistanceBeetweenRays, 
                                            (directionY == DIR_DOWN) ? borders.bottom : borders.top  );

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

    void Update()
    {
    //    ResetCollisionInfo();
    //    CalculateBorders();
     //   ProcessCollision();        
    }

    protected virtual void ResetCollisionInfo(){
        collisionInfo.Reset();
    }
    public virtual void Move( Vector2 velocity ){
        transition = velocity;        
        if( transition.x != 0) collisionInfo.faceDir = (int) Mathf.Sign(transition.x) ;
        ResetCollisionInfo();
        CalculateBorders();
        ProcessCollision();
        transform.Translate( transition );
    }
}
