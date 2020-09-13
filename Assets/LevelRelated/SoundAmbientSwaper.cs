using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAmbientSwaper : MonoBehaviour
{
    public int leftAmbientIndex;
    public int rightAmbientIndex;
    

    void OnTriggerExit2D(Collider2D other){
        if( other.tag == "PlayerHurtBox"){
            Vector2 playerPosition = GlobalUtils.PlayerObject.transform.position;
            Vector2 areaPosition = transform.position;

            if(playerPosition.x < areaPosition.x)
            {
                SoundAmbient.instance.ChangeAmbient(leftAmbientIndex);
            }
            else
            {
                SoundAmbient.instance.ChangeAmbient(rightAmbientIndex);
            }
  
        }  
    }


}
