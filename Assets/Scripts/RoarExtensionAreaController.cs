using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarExtensionAreaController : MonoBehaviour
{
    
    [SerializeField] StalactitController[] objectsToTakeDown;

    bool isHeroInArea = false;
    bool hasBeenActivated = false;

    void Update()
    {
        if( isHeroInArea && !hasBeenActivated ){
            if( GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentState().Contains("2") ){
                GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();
                infoPack.isValid = true;
                infoPack.stateName = GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentState();
                ActivateAllObjects(infoPack);
                hasBeenActivated = true;
            }
        }
    }

    void ActivateAllObjects(GlobalUtils.AttackInfo infoPack){
        foreach( StalactitController obj in objectsToTakeDown){
            if( obj ) obj.OnHit(infoPack);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            isHeroInArea = true;
        }  
    }

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            isHeroInArea = false;
        }  
    }

}
