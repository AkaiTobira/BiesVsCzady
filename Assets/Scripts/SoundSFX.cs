using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSFX : MonoBehaviour {

    [FMODUnity.EventRef]
    public string[] Sounds;
    private List<FMOD.Studio.EventInstance> soundevents;
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
        var sound = FMODUnity.RuntimeManager.CreateInstance (Sounds[soundId]);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        sound.start();
        soundevents.Add( sound );
    }
}