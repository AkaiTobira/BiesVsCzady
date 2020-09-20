using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAmbient : MonoBehaviour {


     public static SoundAmbient instance;

     [FMODUnity.EventRef]
     public string[] Sounds;
     public List<FMOD.Studio.EventInstance> soundevents = new List<FMOD.Studio.EventInstance>();

     private int currentActiveAmbient = 0;

     void Awake() {
          instance = GetComponent<SoundAmbient>();

          foreach( string sound in Sounds){
               soundevents.Add( FMODUnity.RuntimeManager.CreateInstance (sound) );
          }
     }

     void Start() {
          PlayAmbient( 0 );
     }

     public virtual void PlayAmbient( int index ){
          if( index >= Sounds.Length || index < 0 ){
            //Debug.LogError( "Sound Id is invalid " + soundId + " : maxArray is " + Sounds.Length);
            return;
        }

          FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundevents[index], GetComponent<Transform>(), GetComponent<Rigidbody2D>());
          FMOD.Studio.PLAYBACK_STATE fmodPbState;
          soundevents[index].getPlaybackState(out fmodPbState);
          if (fmodPbState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
              soundevents[index].start ();
          }
     }


     public virtual void StopAmbient(int index){

          if( index >= Sounds.Length || index < 0 ){
            //Debug.LogError( "Sound Id is invalid " + soundId + " : maxArray is " + Sounds.Length);
            return;
        }
          FMOD.Studio.PLAYBACK_STATE fmodPbState;
          soundevents[index].getPlaybackState(out fmodPbState);
          if (fmodPbState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
              soundevents[index].stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
          }
     }


     public void ChangeAmbient( int newAmbientIndex){
          StopAmbient( currentActiveAmbient );
          PlayAmbient( newAmbientIndex);
          currentActiveAmbient = newAmbientIndex;
     }

}