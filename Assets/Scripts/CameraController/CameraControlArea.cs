using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlArea : MonoBehaviour
{

    [SerializeField] public  Camera_Follow.KeyValuePairs LeftClamping  = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Camera_Follow.KeyValuePairs RightClamping = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Camera_Follow.KeyValuePairs TopClamping   = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Camera_Follow.KeyValuePairs DownClamping  = new Camera_Follow.KeyValuePairs(false, 0);
    [SerializeField] public Vector3 centerOfCamera = new Vector3(0,-6,0);
    [SerializeField] public float camerSize = 170;
    [SerializeField] public float additionalSmoothTime = 0;


    void SetCameraValues(){
        GUIElements.Camera.SetValues(LeftClamping, RightClamping, TopClamping, DownClamping, centerOfCamera, camerSize);
        GUIElements.Camera.EnableMoreSmooth(additionalSmoothTime);
    }

    void OnTriggerEnter2D(Collider2D other) {

        if( other.name.Equals( "Player") ){
            SetCameraValues();
        }
        
    }


    void OnTriggerExit2D(Collider2D other) {
        if( other.name.Contains( "Player") ){
            SetDefaultValuesOfCamera();
        }
    }

    public void DisableCameraArea(){
            SetDefaultValuesOfCamera();
            gameObject.SetActive(false);
    }

    private void SetDefaultValuesOfCamera(){
            GUIElements.Camera.SetValues(new Camera_Follow.KeyValuePairs(false, 0), 
                                         new Camera_Follow.KeyValuePairs(false, 0), 
                                         new Camera_Follow.KeyValuePairs(false, 0), 
                                         new Camera_Follow.KeyValuePairs(true, 15),
                                         new Vector3(0,-6,0),
                                         GUIElements.Camera.defaultSize
                                         );
            GUIElements.Camera.DisableMoreSmooth();
    }

}
