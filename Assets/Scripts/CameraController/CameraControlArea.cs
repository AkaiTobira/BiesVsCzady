﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlArea : MonoBehaviour
{

    [SerializeField] public  Camera_Follow.KeyValuePairs LeftClamping  = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Camera_Follow.KeyValuePairs RightClamping = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Camera_Follow.KeyValuePairs TopClamping   = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Camera_Follow.KeyValuePairs DownClamping  = new Camera_Follow.KeyValuePairs(false, 0);

    void SetCameraValues(){
        GlobalUtils.Camera.SetValues(LeftClamping, RightClamping, TopClamping, DownClamping);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if( other.name.Contains( "Player") ){
            SetCameraValues();
            Debug.Log("ON");
        }
        
    }


    void OnTriggerExit2D(Collider2D other) {
        if( other.name.Contains( "Player") ){
            GlobalUtils.Camera.SetValues(new Camera_Follow.KeyValuePairs(false, 0), 
                                         new Camera_Follow.KeyValuePairs(false, 0), 
                                         new Camera_Follow.KeyValuePairs(false, 0), 
                                         new Camera_Follow.KeyValuePairs(false, 0));
        }
    }

}
