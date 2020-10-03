using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static string nextSceneName = "";

    public static LoadManager instance = null;

    void Start() {
        if( instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    

    
    public void LoadScene( string name ){
        nextSceneName = name;

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene ()
    {
        AsyncOperation loadScreen = SceneManager.LoadSceneAsync( "LoadScreen" );
        while(!loadScreen.isDone){
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation loadGameScene = SceneManager.LoadSceneAsync( nextSceneName );
        while( !loadGameScene.isDone ){
            yield return new WaitForEndOfFrame();
        }
    }

}