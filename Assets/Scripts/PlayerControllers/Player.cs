using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private SFSMBase m_controller;
    private PlayerUtils.Direction m_dir = PlayerUtils.Direction.Left;

    public void changeDirection( PlayerUtils.Direction dir ){
        if( m_dir == dir ) return;

        Vector3 localTransform = transform.localScale;
        localTransform.x = Mathf.Abs( localTransform.x ) * (float)dir ;
      //  localTransform.z = 0;
      //  localTransform.y = 0;
        transform.localScale = localTransform;
        m_dir = dir;
    }

    void Start()
    {
        m_controller = new SFSMBase( transform.gameObject, new PlayerIdle( transform.gameObject ) );
    }

    // Update is called once per frame
    void Update()
    {
        m_controller.Update();
    }
}
