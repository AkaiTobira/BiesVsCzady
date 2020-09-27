using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBoxBehaviour : MonoBehaviour
{

    [SerializeField] public AkaiController parent;

    [SerializeField] private float MAX_TIME_OF_WAIT = 2.0f;

    private float timerOfPlayerVisible = 0;
    private bool  startTimer    = false;
    private bool  playerEscaped = false;

    private bool inArea = false;

    [SerializeField] private LayerMask m_layers;

    void Update() {
        if( playerEscaped ){
            if( timerOfPlayerVisible == 0) {
                parent.OnPlayerEscape();
                playerEscaped = false;
                inArea        = false;
            }else{
                timerOfPlayerVisible = Mathf.Max( 0, timerOfPlayerVisible - Time.deltaTime );
            }
        }

        CheckObstaclesToPlayer();
    }

    void CheckObstaclesToPlayer(){
        if( !inArea ) return;
        
        Vector2 toPlayer = GlobalUtils.PlayerObject.transform.position - parent.GetPosition();
        //toPlayer = toPlayer/toPlayer.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(   parent.GetPosition(), 
                                                toPlayer,
                                                Mathf.Infinity,
                                                m_layers );


        if( hit.collider.CompareTag( "Player")){
            parent.OnPlayerDetection();
        }

        Debug.DrawLine(parent.GetPosition(), toPlayer * 10000, Color.green);

    }

    void OnTriggerEnter2D(Collider2D other) {
        if( other.gameObject.tag.Contains("Player")){
            playerEscaped        = false;
            timerOfPlayerVisible = MAX_TIME_OF_WAIT;
            inArea               = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if( other.gameObject.tag.Contains("Player")){
            playerEscaped        = true;
        }
    }

}
