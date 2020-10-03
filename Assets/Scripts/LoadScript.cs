using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour
{
    public static string nextSceneName = "";
    public string onEscapeReturnScene = "";
    public void LoadScene( string name ){
        LoadManager.instance.LoadScene( name );
    }

    void Update() {
        OnEscapeReturn( );
    }

    public void OnEscapeReturn( ){
        if( !Input.GetKeyDown(KeyCode.Escape)) return;
        if( onEscapeReturnScene == "") return;
        LoadManager.instance.LoadScene( onEscapeReturnScene );
    }

    public void Exit(){
        Application.Quit();
    }

}