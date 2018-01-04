using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {


    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;


	void Update () {

        if (Wavespawner.lostgame)
        {
            this.pauseMenuUI.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Escape) && !Wavespawner.lostgame)
        {
            if (gameIsPaused)
            {
                resume();
            }else
            {
                pause();
            }
        }
	}

    public void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
