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

    private float distanceBeetweenRay;

    private bool startTimer = false;
    private float maxOfTimerValue = 0;


    void Start()
    {
        m_box = GetComponent<BoxCollider2D>();
    }


    void ProcessPlayerDetection(){
        if( startTimer ) return;
        for( int i = 0 ; i < rayNumber; i++){

            Vector2 rayOrigin = new Vector2( m_box.bounds.min.x + distanceBeetweenRay * i,
                                             m_box.bounds.max.y + 1); 

            RaycastHit2D hit = Physics2D.Raycast( rayOrigin, Vector2.up, rayLenght, playerLayer );

            if( hit ){
                if( hit.collider.tag == "PlayerHurtBox") {
                
                    startTimer = true;
                    maxOfTimerValue = m_existingTimer;
                }
            }

            Debug.DrawLine( rayOrigin, rayOrigin + Vector2.up * rayLenght, new Color( 0, 0.5f,0 ));

        }
;

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
        distanceBeetweenRay = (m_box.bounds.max.x - m_box.bounds.min.x)/( rayNumber - 1.0f); 
        
        ProcessPlayerDetection();
        ProcessAutoDestroy();
    }
}
