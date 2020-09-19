using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSFX : MonoBehaviour {

    [FMODUnity.EventRef]
    public string[] Sounds;
    private List<FMOD.Studio.EventInstance> soundevents = new List<FMOD.Studio.EventInstance>();
    public int playOnStart;
    public bool shouldPlayOnStart;




    void Start (){
    }

    void OnEnable() {
        if(shouldPlayOnStart)
        {
            PlaySFX(playOnStart);
        }
    }

    void DetachInstance(){
        foreach( FMOD.Studio.EventInstance ei in soundevents){

          FMOD.Studio.PLAYBACK_STATE fmodPbState;
          ei.getPlaybackState(out fmodPbState);

          if( fmodPbState == FMOD.Studio.PLAYBACK_STATE.STOPPED ){
              FMODUnity.RuntimeManager.DetachInstanceFromGameObject( ei );
              soundevents.Remove( ei );
          }
        }
    }


    void PlaySFX( int soundId){
        if( soundId > Sounds.Length || soundId < 0 ){
            Debug.LogError( "Sound Id is invalid" + soundId + " : maxArray is" + Sounds.Length);
            return;
        }
        var sound = FMODUnity.RuntimeManager.CreateInstance (Sounds[soundId]);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        sound.start();
        soundevents.Add( sound );
    }
}