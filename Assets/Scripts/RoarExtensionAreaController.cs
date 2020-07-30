using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarExtensionAreaController : MonoBehaviour
{
    
    [SerializeField] StalactitController[] objectsToTakeDown = null;

    bool isHeroInArea = false;
    bool hasBeenActivated = false;

    enum AttackType{
        Roar = 2,
        Hit1  = 1,
        Hit2  = 4,
        Hit3  = 5
    }

    [SerializeField] AttackType[] workingTypesOfAttack = null;

    void Update()
    {
        if( isHeroInArea && !hasBeenActivated ){
            foreach( AttackType at in workingTypesOfAttack ){
                Debug.Log( at.ToString() + " :  " +  ( (int)at ).ToString());
                if( GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentState().Contains(( (int)at ).ToString()) ){
                    Debug.Log("Used"  + ( (int)at ).ToString());
                    GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();
                    infoPack.isValid = true;
                    infoPack.stateName = GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentState();
                    ActivateAllObjects(infoPack);
                    hasBeenActivated = true;
                }
            }
//            GlobalUtils.TutorialConsole.text += "\nC or RMB - break the stalactits";
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
