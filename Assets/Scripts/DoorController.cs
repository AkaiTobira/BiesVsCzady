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
            int owendNumberOfKeys = GlobalUtils.PlayerObject.GetComponent<Player>().keys;
            if( owendNumberOfKeys >= numberOfRequiredKeys){
                if( PlayerInput.isActionKeyHold() ){
                    doorDetector.GetComponent<Animator>().SetTrigger("isOpen");
                    owendNumberOfKeys -= numberOfRequiredKeys;
                    Destroy(gameObject);
                    hasBeenActivated = true;
                    GlobalUtils.GUIOverlay.keyInfoScreen.HideAtAreaDoorExit();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            isHeroInArea = true;
            GlobalUtils.GUIOverlay.keyInfoScreen.ShowAtAreaDoorEnter(numberOfRequiredKeys);
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            isHeroInArea = false;
            GlobalUtils.GUIOverlay.keyInfoScreen.HideAtAreaDoorExit();
        }  
    }


}
