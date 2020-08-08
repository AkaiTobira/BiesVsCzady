using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceEnterLockArea : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            LockAreaOverseer.isChangeLocked = true;
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            LockAreaOverseer.isChangeLocked = false;
            Destroy( gameObject );
        }  
    }


}
