using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxHandler : MonoBehaviour
{

    [SerializeField] IEntity     mainEntity = null;
    void OnTriggerEnter2D(Collider2D other){
        if(  other.gameObject.name.Contains( "Attack" ) ){
            var smt = other.gameObject.GetComponent<AttackBoxHandler>().mainScript.GetAttackInfo();
            mainEntity.OnHit( smt );
        }
    }

    public IEntity GetMainEntity(){
        return mainEntity;
    }

}
