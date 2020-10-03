using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu = null;
    public GameObject endMenu   = null;
    public GameObject controllMenu   = null;

    public GameObject pauseScreen   = null;
    


    public void PauseGame(){
        Time.timeScale = 0.00001f;
        pauseMenu.SetActive(true);
        if( GlobalUtils.PlayerObject.GetComponent<Player>().gameOver && endMenu.activeSelf ){
            endMenu.SetActive(false);
        }
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PauseMenu", 1, false);
    }

    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape )){
            //Debug.Log( pauseMenu.activeSelf );
            if( !pauseMenu.activeSelf ) PauseGame();
            else ReplayGame();
            //Application.Quit();
        }
    }

    public void SwitControlls(){
        if( pauseScreen.activeSelf ){
            controllMenu.SetActive( true );
            pauseScreen.SetActive(false);
        }else{
            controllMenu.SetActive( false );
            pauseScreen.SetActive(true);
        }
    }

    public void ResetAllSettings(){
        pauseMenu.SetActive(false);
            controllMenu.SetActive( false );
            pauseScreen.SetActive(true);
        Time.timeScale = 1;
    }

    public void ReplayGame(){
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PauseMenu", 0, false);

        
        if( GlobalUtils.PlayerObject.GetComponent<Player>().gameOver && !endMenu.activeSelf ){
            endMenu.SetActive(true);
        }

        ResetAllSettings();
    }

    public void Exit(){
        Application.Quit();
    }

    public void ToMainMenu(){
        Debug.Log("DummyFunctionality");
    }

}
