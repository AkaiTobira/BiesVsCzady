using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            GlobalUtils.TaskMaster.UpdateCheckpoint( transform.position );
        }  
    }

}
