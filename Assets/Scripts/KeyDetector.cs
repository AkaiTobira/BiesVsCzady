using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{

    public Animator TextNode;

    void OnTriggerEnter2D(Collider2D other) {
        if( other.name.Contains( "Player") ){
            other.GetComponent<Player>().AddKey();
            GUIElements.GUIOverlay.keyInfoScreen.ShowAtAddKey();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enviro/key collect");
            if( TextNode ) TextNode.SetTrigger("Show");
            Destroy(gameObject);
        }
    }
}
