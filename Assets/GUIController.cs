using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    void Start()
    {   
        GlobalUtils.GUIOverlay      = GetComponent<GUIController>();
        GlobalUtils.TutorialConsole = transform.Find("TutorialText").GetChild(0).GetComponent<Text>();
    }

    public void SwitchButtonVisibility(){
        var buttons_node = transform.Find("Buttons").gameObject;
        buttons_node.SetActive( ! buttons_node.activeSelf ); 
    }


}
