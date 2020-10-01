using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu = null;
    [FMODUnity.ParamRef]
    public string lpfOn;

    public void PauseGame(){
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(lpfOn, 1, false);
    }

    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape )){
            Debug.Log( pauseMenu.activeSelf );
            if( !pauseMenu.activeSelf ) PauseGame();
            else ReplayGame();
        }
    }


    public void ReplayGame(){
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(lpfOn, 0, false);
    }

    public void Exit(){
        Application.Quit();
    }

    public void ToMainMenu(){
        Debug.Log("DummyFunctionality");
    }

}
