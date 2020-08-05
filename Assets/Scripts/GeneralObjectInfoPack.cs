using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObjectInfoPack
{

    public void saveBasics( Transform t){
        position = new Vector2( t.position.x, t.position.y );
        rotation = new Quaternion( t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w );
        scale    = new Vector2( t.localScale.x, t.localScale.y );
    }

    public bool isValid;

    public Vector2 position;
    public Quaternion rotation;
    public Vector2 scale;
}
