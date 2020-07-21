using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if( other.name.Contains( "Player") ){
            other.GetComponent<Player>().AddKey();
            Destroy(gameObject);
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
