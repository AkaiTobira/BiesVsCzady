using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransition : MonoBehaviour
{
    public Vector2 MoveSpeed = new Vector2();

    private Player m_player;

    void Start()
    {
        m_player = transform.parent.Find("Player").GetComponent<Player>();
    }

    public void setBlock( int setActive ){
//        Debug.Log( m_player.blockActive );
        m_player.blockActive = (setActive == 0 ) ? false : true ;
 //       Debug.Log( m_player.blockActive );
    }

}
