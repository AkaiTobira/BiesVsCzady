﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            LockAreaOverseer.isChangeLocked = true;
            GlobalUtils.PlayerObject.
               GetComponent<Player>().animationNode.
               GetComponent<Animator>().SetBool("SneakySneaky", true);
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            LockAreaOverseer.isChangeLocked = false;
            GlobalUtils.PlayerObject.
               GetComponent<Player>().animationNode.
               GetComponent<Animator>().SetBool("SneakySneaky", false);
        }  
    }


}
