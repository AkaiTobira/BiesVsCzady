using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueArea : MonoBehaviour
{

    [SerializeField] GlobalUtils.DialogueInfo[] dialogue = null;

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            foreach( GlobalUtils.DialogueInfo dialog in dialogue){
                GUIElements.DialogueSystem.AddToSequence( dialog );
            }
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            Destroy(gameObject);
        }  
    }


}
