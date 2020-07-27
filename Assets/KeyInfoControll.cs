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
        m_text.text = GlobalUtils.PlayerObject.GetComponent<Player>().keys.ToString() + "/" + requiredNumberOfKeys.ToString();
        m_animator.SetTrigger("Show");
    }

    public void HideAtAreaDoorExit(){
        m_animator.SetTrigger("Hide");
    }
}
