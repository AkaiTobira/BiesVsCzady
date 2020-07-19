using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] int numberOfRequiredKeys = 1;

    [SerializeField] Transform doorDetector = null;

    void OnTriggerStay2D(Collider2D other) {

        if( other.name.Contains("Player")){
            if( PlayerInput.isActionKeyHold() ){
                if( other.GetComponent<Player>().keys >= numberOfRequiredKeys){
                    doorDetector.GetComponent<Animator>().SetTrigger("isOpen");
                    other.GetComponent<Player>().keys -= numberOfRequiredKeys;
                    Destroy(gameObject);
                }
            }
        }
    
    }


}
