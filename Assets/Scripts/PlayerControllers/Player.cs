using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private LayerMask m_floorLayerMask = 8; 
    private SFSMBase m_controller;
    private PlayerUtils.Direction m_dir = PlayerUtils.Direction.Left;

    private BoxCollider2D m_boxCollider;

    public void changeDirection( PlayerUtils.Direction dir ){
        if( m_dir == dir ) return;
        Vector3 localTransform = transform.localScale;
        localTransform.x = Mathf.Abs( localTransform.x ) * (float)dir ;
        transform.localScale = localTransform;
        m_dir = dir;
    }

    void Start()
    {
        m_boxCollider = GetComponent<BoxCollider2D>(); 
        m_controller  = new SFSMBase( transform.gameObject, new PlayerIdle( transform.gameObject ) );
    }

    public bool isOnGrounded(){
        float extraHeight = 0.1f;
        RaycastHit2D rHit = Physics2D.BoxCast(m_boxCollider.bounds.center,
                                              m_boxCollider.bounds.size,
                                              0,
                                              Vector2.down, 
                                              extraHeight,
                                              m_floorLayerMask);
        Color rayColor;
        if( rHit.collider != null ){
            rayColor = Color.green;
        }else {rayColor = Color.red;}
        Debug.DrawRay( m_boxCollider.bounds.center, Vector2.down * ( m_boxCollider.bounds.extents.y + extraHeight), rayColor, Time.deltaTime, false );
        return rHit.collider != null ;
    }

    // Update is called once per frame
    void Update()
    {
        m_controller.Update();
    }
}
