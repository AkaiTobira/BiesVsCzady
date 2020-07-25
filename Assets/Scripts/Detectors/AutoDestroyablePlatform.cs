using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyablePlatform : MonoBehaviour
{
    
    [SerializeField] float rayNumber = 2;
    [SerializeField] float rayLenght = 100;
    [SerializeField] LayerMask playerLayer = 0;

    [HideInInspector] BoxCollider2D m_box;

    [SerializeField] float m_existingTimer = 0;

    private float existingTimer = 0;
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
                bool isValidHit = hit.collider.tag == "PlayerHurtBox";
                isValidHit     |= hit.collider.tag == "StalactitHurtBos";
                if( isValidHit ) {
                    startTimer = true;
                    existingTimer   = m_existingTimer;
                    maxOfTimerValue = m_existingTimer;
                }
            }
            Debug.DrawLine( rayOrigin, rayOrigin + upDirection * rayLenght, new Color( 0, 0.5f,0 ));
        }
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

    private bool reapear = false;
    [SerializeField] public float ReapearTimer = 15;

    private float timeToReapear = 0;

    void ProcessAutoDestroy(){
        if( !startTimer ) return;
        existingTimer = Mathf.Max( existingTimer - Time.deltaTime, 0 );

        var colorForAlphaReduction = GetComponent<SpriteRenderer>().color;
        colorForAlphaReduction.r = 0.0f;
        colorForAlphaReduction.a = 1.0f * ( existingTimer/maxOfTimerValue);
        GetComponent<SpriteRenderer>().color = colorForAlphaReduction;

        if( existingTimer == 0 ) {
            GetComponent<BoxCollider2D>().enabled = false;
            startTimer = false;
            reapear    = true;
            timeToReapear = ReapearTimer;
        };
    }

    void ProcessReapear(){
        timeToReapear = Mathf.Max( 0, timeToReapear - Time.deltaTime );
        var colorForAlphaReduction = GetComponent<SpriteRenderer>().color;
        colorForAlphaReduction.r = 1.0f;
        colorForAlphaReduction.a = 1.0f - ( timeToReapear/ReapearTimer);
        GetComponent<SpriteRenderer>().color = colorForAlphaReduction;

        if( timeToReapear == 0 ){
            GetComponent<BoxCollider2D>().enabled = true;
            m_box.enabled = true;
            reapear       = false;
            colorForAlphaReduction.r = 0.0f;
            GetComponent<SpriteRenderer>().color = colorForAlphaReduction;
        }
    }

    void Update()
    {
        CalculateColliderVertexPositions();

        if( !reapear){
            ProcessPlayerDetection();
            ProcessAutoDestroy();
        }else{
            ProcessReapear();
        }
        }
}
