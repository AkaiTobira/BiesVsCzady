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

    void Update() {
        if( playerEscaped ){
            if( timerOfPlayerVisible == 0) {
                parent.OnPlayerEscape();
                playerEscaped = false;
            }else{
                timerOfPlayerVisible = Mathf.Max( 0, timerOfPlayerVisible - Time.deltaTime );
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if( other.gameObject.tag.Contains("Player")){
            playerEscaped        = false;
            timerOfPlayerVisible = MAX_TIME_OF_WAIT;
            parent.OnPlayerDetection();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if( other.gameObject.tag.Contains("Player")){
            playerEscaped        = true;
        }
    }

}
