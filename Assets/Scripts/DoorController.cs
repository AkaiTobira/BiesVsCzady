using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField] int numberOfRequiredKeys = 1;

    [SerializeField] Transform doorDetector = null;

    bool isHeroInArea = false;
    bool hasBeenActivated = false;

    void Update()
    {
        if( isHeroInArea && !hasBeenActivated ){

            Debug.Log( " SMT ");
            int owendNumberOfKeys = GlobalUtils.PlayerObject.GetComponent<Player>().keys;
            GlobalUtils.TutorialConsole.text += "\n" + owendNumberOfKeys.ToString() + "/" + numberOfRequiredKeys.ToString() + " keys";
            if( owendNumberOfKeys >= numberOfRequiredKeys){
                GlobalUtils.TutorialConsole.text += "\nF - open door";
                if( PlayerInput.isActionKeyHold() ){
                    doorDetector.GetComponent<Animator>().SetTrigger("isOpen");
                    owendNumberOfKeys -= numberOfRequiredKeys;
                    Destroy(gameObject);
                    hasBeenActivated = true;
                }
            }else{
                GlobalUtils.TutorialConsole.text += "\ninsufficient number of keys to open";
            }
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
