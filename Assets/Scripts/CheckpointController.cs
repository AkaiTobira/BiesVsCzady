using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetAcitve(){
        _animator.SetTrigger("Acitvate");
    }

    public void ResetCheckpoint(){
        _animator.Rebind();
    }

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            GlobalUtils.TaskMaster.UpdateCheckpoint( this );
        }  
    }

}
