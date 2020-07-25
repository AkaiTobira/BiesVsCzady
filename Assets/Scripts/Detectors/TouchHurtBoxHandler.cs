using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHurtBoxHandler : MonoBehaviour
{

    [SerializeField] IEntity     mainEntity = null;
    void OnTriggerEnter2D(Collider2D other){
        if(  other.tag.Contains( "EnemyHurtBox" ) ){
            var smt = other.gameObject.GetComponent<HurtBoxHandler>().GetMainEntity().GetAttackInfo();
            mainEntity.OnHit( smt );
        }
    }

    public IEntity GetMainEntity(){
        return mainEntity;
    }

}
