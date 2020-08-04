using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] int numberOfRequiredKeys = 1;

    [SerializeField] Transform doorDetector = null;

    bool isHeroInArea = false;
    bool hasBeenActivated = false;

    void Update()
    {
        if( isHeroInArea && !hasBeenActivated ){
            if( GlobalUtils.PlayerObject.GetComponent<Player>().keys >= numberOfRequiredKeys){
                if( PlayerInput.isActionKeyHold() ){
                    doorDetector.GetComponent<Animator>().SetTrigger("isOpen");
                    GlobalUtils.PlayerObject.GetComponent<Player>().keys -= numberOfRequiredKeys;
                    Destroy(gameObject);
                    hasBeenActivated = true;
                    GlobalUtils.GUIOverlay.keyInfoScreen.HideAtAreaDoorExit();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
//        Debug.Log( other.tag + " :: " + other.name );
        if( other.tag.Contains("Player")){
            isHeroInArea = true;
            GlobalUtils.GUIOverlay.keyInfoScreen.ShowAtAreaDoorEnter(numberOfRequiredKeys);
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag.Contains("Player")){
            isHeroInArea = false;
            GlobalUtils.GUIOverlay.keyInfoScreen.HideAtAreaDoorExit();
        }  
    }


}
