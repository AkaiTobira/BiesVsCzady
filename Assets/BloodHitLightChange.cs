using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class BloodHitLightChange : MonoBehaviour
{
    
    float savedLightIntensivity;
    Color savedColor;

    UnityEngine.Experimental.Rendering.Universal.Light2D m_light;

    [SerializeField] Color hurtColor;
    [SerializeField] float hurtLightIntensivity;

    [SerializeField] float timeOfReturningToSavedColor = 2.0f;

    float returningToNormalTimer = 0;

    void Start()
    {
        m_light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        savedColor = m_light.color;
        savedLightIntensivity = m_light.intensity;

        GUIElements.LightHit = GetComponent<BloodHitLightChange>();

        Debug.Log( savedColor + " " + savedLightIntensivity );
    }

    public void ApplyHurtColors(){
        m_light.color     = hurtColor;
        m_light.intensity = hurtLightIntensivity;
        returningToNormalTimer = timeOfReturningToSavedColor;
        Debug.Log( "APPLYColors");
    }

    void Update()
    {
        returningToNormalTimer = Mathf.Max( 0 , returningToNormalTimer - Time.deltaTime );
        if( returningToNormalTimer == 0) return;

        float t = returningToNormalTimer/timeOfReturningToSavedColor;

        Debug.Log( returningToNormalTimer + " " + t);

        m_light.color     = savedColor             * (1f - t) + hurtColor * t;
        m_light.intensity = savedLightIntensivity  * (1f - t) + hurtLightIntensivity * t;
    }
}
