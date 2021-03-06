﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] public int numberOfRequiredKeys = 1;

    [SerializeField] Transform doorDetector = null;

    bool isHeroInArea = false;
    public bool hasBeenActivated = false;

    void Update()
    {
        if( isHeroInArea && !hasBeenActivated ){
            if( GlobalUtils.PlayerObject.GetComponent<Player>().keys >= numberOfRequiredKeys){
                if( PlayerInput.isActionKeyHold() ){
                    doorDetector.GetComponent<Animator>().SetTrigger("isOpen");
                    GlobalUtils.PlayerObject.GetComponent<Player>().keys -= numberOfRequiredKeys;
                    Destroy(gameObject);
                    hasBeenActivated = true;
                    GUIElements.GUIOverlay.keyInfoScreen.HideAtAreaDoorExit();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
//        Debug.Log( other.tag + " :: " + other.name );
        if( other.tag.Contains("Player")){
            isHeroInArea = true;
            GUIElements.GUIOverlay.keyInfoScreen.ShowAtAreaDoorEnter(numberOfRequiredKeys);
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag.Contains("Player")){
            isHeroInArea = false;
            GUIElements.GUIOverlay.keyInfoScreen.HideAtAreaDoorExit();
        }  
    }


}
