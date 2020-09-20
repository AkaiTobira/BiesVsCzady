using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAmbientStopable : SoundAmbient
{

    void Awake() {
          instance = GetComponent<SoundAmbientStopable>();

          foreach( string sound in Sounds){
               soundevents.Add( FMODUnity.RuntimeManager.CreateInstance (sound) );
          }
    }

    private void Start()
    {
        
    }
    public void PlaySFX(int soundId)
    {
        if( soundId >= Sounds.Length || soundId < 0 ){
            //Debug.LogError( "Sound Id is invalid " + soundId + " : maxArray is " + Sounds.Length);
            return;
        }
        var sound = FMODUnity.RuntimeManager.CreateInstance(Sounds[soundId]);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        sound.start();
        //soundevents.Add(sound);
    }
}
