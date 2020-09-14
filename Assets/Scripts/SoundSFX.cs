using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSFX : MonoBehaviour {

    [FMODUnity.EventRef]
    public string[] Sounds;
    public FMOD.Studio.EventInstance[] soundevents;
    public int playOnStart;
    public bool shouldPlayOnStart;

    void Start (){

        soundevents = new FMOD.Studio.EventInstance[Sounds.Length];
        for( int i = 0; i < Sounds.Length; i++ ){
            soundevents[i] = FMODUnity.RuntimeManager.CreateInstance (Sounds[i]);

        }

        
    }

    private void OnEnable()
    {
        if(shouldPlayOnStart)
        {
            PlaySFX(playOnStart);
        }
    }

    void PlaySFX( int soundId){
        var sound = FMODUnity.RuntimeManager.CreateInstance (Sounds[soundId]);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        sound.start();
    }
}