using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipSwitcher : MonoBehaviour
{

    bool isVisible = false;

    void Update()
    {
        if( GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentFormName().Contains("Cat") ){
            isVisible = true;
        }else{ 
            isVisible = false;
        }
        

        for( int i = 0; i< transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive( isVisible );
        }
    }
}
