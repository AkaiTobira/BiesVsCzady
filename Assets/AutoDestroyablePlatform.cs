using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyablePlatform : MonoBehaviour
{
    
    [SerializeField] float rayNumber = 2;
    [SerializeField] float rayLenght = 100;
    [SerializeField] LayerMask playerLayer;

    [HideInInspector] BoxCollider2D m_box;

    [SerializeField] float m_existingTimer;

    private float distanceBeetweenRayH;

    private float distanceBeetweenRayV;
    private bool startTimer = false;
    private float maxOfTimerValue = 0;

    private Vector2 LeftOriginTop = new Vector2();
    private Vector2 RightOriginTop = new Vector2();
    
    private Vector2 LeftOriginBottom = new Vector2();
    private Vector2 RightOriginBottom = new Vector2();

    [SerializeField] bool reverseRays = false;

    void Start()
    {
        m_box = GetComponent<BoxCollider2D>();
        CalculateColliderVertexPositions();
    }


    void ProcessPlayerDetection(){
        if( startTimer ) return;
        for( int i = 0 ; i < rayNumber; i++){

            Vector2 rayOrigin;
            Vector2 upDirection;
            if( !reverseRays ){
                rayOrigin = new Vector2(RightOriginTop.x + distanceBeetweenRayH * i,
                                        RightOriginTop.y + distanceBeetweenRayV * i); 
                upDirection = transform.up;
            }else{
                rayOrigin = new Vector2(RightOriginBottom.x + distanceBeetweenRayH * i,
                                        RightOriginBottom.y + distanceBeetweenRayV * i); 
                upDirection = -transform.up;
            }


            RaycastHit2D hit = Physics2D.Raycast( rayOrigin, upDirection, rayLenght, playerLayer );

            if( hit ){
                if( hit.collider.tag == "PlayerHurtBox") {
                
                    startTimer = true;
                    maxOfTimerValue = m_existingTimer;
                }
            }

            Debug.DrawLine( rayOrigin, rayOrigin + upDirection * rayLenght, new Color( 0, 0.5f,0 ));

        }
;

    }


    public void  CalculateColliderVertexPositions ()  
    {
        RightOriginTop    = gameObject.transform.TransformPoint(m_box.offset + new Vector2(-m_box.size.x,  m_box.size.y) * 0.5f);
        LeftOriginTop     = gameObject.transform.TransformPoint(m_box.offset + new Vector2( m_box.size.x,  m_box.size.y) * 0.5f);
        RightOriginBottom = gameObject.transform.TransformPoint(m_box.offset + new Vector2(-m_box.size.x, -m_box.size.y) * 0.5f);
        LeftOriginBottom  = gameObject.transform.TransformPoint(m_box.offset + new Vector2(m_box.size.x,  -m_box.size.y) * 0.5f);

        distanceBeetweenRayH = (LeftOriginTop.x - RightOriginTop.x)/( rayNumber - 1.0f); 
        distanceBeetweenRayV = (LeftOriginTop.y - RightOriginTop.y)/( rayNumber - 1.0f); 
    }

    void ProcessAutoDestroy(){
        if( !startTimer ) return;
        m_existingTimer = Mathf.Max( m_existingTimer - Time.deltaTime, 0 );

        var colorForAlphaReduction = GetComponent<SpriteRenderer>().color;
        colorForAlphaReduction.a = 1.0f * ( m_existingTimer/maxOfTimerValue);
        GetComponent<SpriteRenderer>().color = colorForAlphaReduction;

        if( m_existingTimer == 0 ) Destroy( gameObject );
    }

    void Update()
    {
        CalculateColliderVertexPositions();

    //    var a = GetColliderVertexPositions( gameObject );

    //    for( int t = 0; t < 2; t ++){
    //        Debug.Log( a[t]);
    //        Debug.DrawLine( a[t],    a[t] +   (Vector2) transform.up * rayLenght, new Color( 0.5f, 0.5f,0 ));
    //    }

    //    Debug.DrawLine( m_box.bounds.min,    m_box.bounds.min +    Vector3.up * rayLenght, new Color( 0.5f, 0.5f,0 ));
    //    Debug.DrawLine( m_box.bounds.center, m_box.bounds.center + Vector3.up * rayLenght, new Color( 0.5f, 0.5f,0 ));

        ProcessPlayerDetection();
        ProcessAutoDestroy();
    }
}
