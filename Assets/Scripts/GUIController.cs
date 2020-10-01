using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{

    public KeyInfoControll keyInfoScreen;

    public Animator endScreen;

    void Start()
    {   
        GUIElements.GUIOverlay      = GetComponent<GUIController>();
        GUIElements.TutorialConsole = transform.Find("TutorialText").GetChild(0).GetComponent<Text>();
        GUIElements.endScreen       = endScreen;
    }

    public void SwitchButtonVisibility(){
        var buttons_node = transform.Find("Buttons").gameObject;
        buttons_node.SetActive( ! buttons_node.activeSelf ); 
    }

    public void SwitchTutorialConsoleVisibility(){
        var buttons_node = transform.Find("TutorialText").gameObject;
        buttons_node.SetActive( ! buttons_node.activeSelf ); 
    }

}
