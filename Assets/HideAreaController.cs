using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAreaController : MonoBehaviour
{

    public bool state = true;

    public void ResetValues(){
        state = true;
        GetComponent<Animator>().Rebind();
    }


    public void SetVisible(){
        state = true;
        GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void SetNotVisible(){
        state = false;
        GetComponent<Animator>().SetTrigger("FadeOut");
    }

}
