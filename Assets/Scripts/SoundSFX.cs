using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSFX : MonoBehaviour {

    [FMODUnity.EventRef]
    public string[] SFXSounds;
    
    [FMODUnity.EventRef]
    public string[] LoopedSounds;
    private List<FMOD.Studio.EventInstance> _aciveSounds = new List<FMOD.Studio.EventInstance>();
    private List<FMOD.Studio.EventInstance> _aciveLoopedSounds = new List<FMOD.Studio.EventInstance>();
    
    public int _playOnStart;
    public int _playLoopedOnStart;
    public bool _shouldPlayOnStart;
    public bool _shouldPlayLoopedOnStart;

    void Start (){
   
    }

    void OnEnable() {
        if(_shouldPlayOnStart)
        {
            PlaySFX(_playOnStart);
        }

        foreach( string sound in LoopedSounds){
            var NewSound = FMODUnity.RuntimeManager.CreateInstance (sound);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(NewSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
            _aciveLoopedSounds.Add( NewSound );
        }

        if(_shouldPlayLoopedOnStart)       
        {
            PlaySFX(_playLoopedOnStart);
        }
    }

    public void TurnOffAllSounds(){
        foreach(  FMOD.Studio.EventInstance instance in _aciveSounds  ){
            instance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        }
        foreach(  FMOD.Studio.EventInstance instance in _aciveLoopedSounds  ){
            instance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        }
    }

    void DetachSFXInstance(){
        List<int> indexes = new List<int>();

        for( int i = 0; i < _aciveSounds.Count; i++){
            
            FMOD.Studio.PLAYBACK_STATE fmodPbState;
            _aciveSounds[i].getPlaybackState(out fmodPbState);

            if( fmodPbState == FMOD.Studio.PLAYBACK_STATE.STOPPED ){
                indexes.Add( i );
            }
        }

        indexes.Reverse();

        foreach( int index in indexes){
            FMODUnity.RuntimeManager.DetachInstanceFromGameObject( _aciveSounds[index] );
            _aciveSounds.Remove( _aciveSounds[index] );
        }
    }


    public void PlayLoopedSFX( int soundId ){
        if( soundId > LoopedSounds.Length || soundId < 0 ){
            Debug.LogError( "Sound Id is invalid" + soundId + " : maxArray is" + SFXSounds.Length);
            return;
        }

        
        if ( LoopedSounds[soundId] == null){
            Debug.LogError( "Array of sounds : index " + soundId + " is null" );
            return;
        }

        FMOD.Studio.PLAYBACK_STATE fmodPbState;
        _aciveLoopedSounds[soundId].getPlaybackState(out fmodPbState);
        if( fmodPbState == FMOD.Studio.PLAYBACK_STATE.STOPPED ){
            _aciveLoopedSounds[soundId].start();
        }
    }

    public void StopLoopedSFX( int soundId ){
        if( soundId > LoopedSounds.Length || soundId < 0 ){
            Debug.LogError( "Sound Id is invalid" + soundId + " : maxArray is" + LoopedSounds.Length);
            return;
        }

        if ( LoopedSounds[soundId] == null){
            Debug.LogError( "Array of sounds : index " + soundId + " is null" );
            return;
        }

        _aciveLoopedSounds[soundId].stop(  FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
    }


    public void PlaySFX( int soundId ){
        DetachSFXInstance();
        if( soundId > SFXSounds.Length || soundId < 0 ){
            Debug.LogError( "Sound Id is invalid" + soundId + " : maxArray is" + SFXSounds.Length);
            return;
        }

        if ( SFXSounds[soundId] == null){
            Debug.LogError( "Array of sounds : index " + soundId + " is null" );
            return;
        }

        var sound = FMODUnity.RuntimeManager.CreateInstance (SFXSounds[soundId]);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        sound.start();
        _aciveSounds.Add( sound );
    }

    public void PlaySFX3D(int soundId)
    {
        DetachSFXInstance();
        if (soundId > SFXSounds.Length || soundId < 0)
        {
            Debug.LogError( "Sound Id is invalid" + soundId + " : maxArray is" + SFXSounds.Length);
            return;
        }

        if (SFXSounds[soundId] == null)
        {
            Debug.LogError("Array of sounds : index " + soundId + " is null");
            return;
        }

        //FMODUnity.RuntimeManager.PlayOneShot(SFXSounds[soundId]);
        var sound = FMODUnity.RuntimeManager.CreateInstance(SFXSounds[soundId]);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, GetComponent<Transform>(), GetComponent<Rigidbody>());
        sound.start();
        _aciveSounds.Add(sound);
    }
}