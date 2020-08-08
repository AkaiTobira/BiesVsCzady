using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyInfoControll : MonoBehaviour
{
    public Text     m_text; 
    public Animator m_animator;

    void Start(){
        m_animator = GetComponent<Animator>();
    }

    public void ShowAtAddKey(){
        m_text.text = GlobalUtils.PlayerObject.GetComponent<Player>().keys.ToString();
        m_animator.SetTrigger("AddKey");
    }

    public void ShowAtAreaDoorEnter(int requiredNumberOfKeys){
        int acquiredKeys = GlobalUtils.PlayerObject.GetComponent<Player>().keys;
        m_text.text = acquiredKeys.ToString() + "/" + requiredNumberOfKeys.ToString();
        m_text.text += (acquiredKeys >= requiredNumberOfKeys) ?  "\n F - Otwórz" : "\nZbierz więcej";
        m_animator.SetTrigger("Show");
    }

    public void HideAtAreaDoorExit(){
        m_animator.SetTrigger("Hide");
    }
}
