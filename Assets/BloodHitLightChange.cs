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

    [SerializeField] float addLightIntensivity;
    [SerializeField] float timeOfReturningToSavedColor2 = 2.0f;


    float returningToNormalTimer = 0;

    void Start()
    {
        m_light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        savedColor = m_light.color;
        savedLightIntensivity = m_light.intensity;

        GUIElements.LightHit = GetComponent<BloodHitLightChange>();

  //      Debug.Log( savedColor + " " + savedLightIntensivity );
    }

    bool hurtLight = false;


    public void ApplyLightColors(){
        hurtLight = false;
        m_light.color     = Color.white;
        m_light.intensity = addLightIntensivity;
        returningToNormalTimer = timeOfReturningToSavedColor2;
  //      Debug.Log( "APPLYColors");
    }

    public void ApplyHurtColors(){
        hurtLight = true;
        m_light.color     = hurtColor;
        m_light.intensity = hurtLightIntensivity;
        returningToNormalTimer = timeOfReturningToSavedColor;
  //      Debug.Log( "APPLYColors");
    }

    void Update()
    {
        
        returningToNormalTimer = Mathf.Max( 0 , returningToNormalTimer - Time.deltaTime );
        if( returningToNormalTimer == 0) return;

        if( hurtLight ){
            float t = returningToNormalTimer/timeOfReturningToSavedColor;
            m_light.color     = savedColor             * (1f - t) + hurtColor * t;
            m_light.intensity = savedLightIntensivity  * (1f - t) + hurtLightIntensivity * t;
        }else{
            float t = returningToNormalTimer/timeOfReturningToSavedColor2;
            m_light.color     = savedColor             * (1f - t) + Color.white * t;
            m_light.intensity = savedLightIntensivity  * (1f - t) + addLightIntensivity * t;
        }


    }
}
