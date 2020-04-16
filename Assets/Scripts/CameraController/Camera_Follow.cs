using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{

    [System.Serializable]
    public class KeyValuePairs
    {
        public KeyValuePairs(){}
        public KeyValuePairs(bool key, float value){
            enable = key;
            position = value;
        }
        public bool enable;
        public float position;
    }  

    [SerializeField] private Transform followedObject = null;

    [SerializeField] public  KeyValuePairs LeftClamping  = new KeyValuePairs( false, 0);
    [SerializeField] private KeyValuePairs RightClamping = new KeyValuePairs( false, 0);
    [SerializeField] private KeyValuePairs TopClamping   = new KeyValuePairs( false, 0);
    [SerializeField] private KeyValuePairs DownClamping  = new KeyValuePairs( false, 0);
    [SerializeField] private float m_smoothTime = 10.0f;

    Vector3 velocity  = Vector3.zero;

    float GetXPosition(){
        float minValue = (LeftClamping.enable)  ? LeftClamping.position  : followedObject.position.x;
        float maxValue = (RightClamping.enable) ? RightClamping.position : followedObject.position.x;
        return Mathf.Clamp( followedObject.position.x, minValue, maxValue);
    }

    float GetYPosition(){
        float minValue = (DownClamping.enable) ? DownClamping.position : followedObject.position.y;
        float maxValue = (TopClamping.enable)  ? TopClamping.position  : followedObject.position.y;
        return Mathf.Clamp( followedObject.position.y, minValue, maxValue);
    }

    void Update()
    {
        Vector3 targetPosition = followedObject.position;
        targetPosition.z = transform.position.z;
        targetPosition.x = GetXPosition();
        targetPosition.y = GetYPosition();
        
        transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref velocity, m_smoothTime);
    }

}
