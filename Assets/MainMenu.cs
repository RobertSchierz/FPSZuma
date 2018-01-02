using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject mainmenuCamera;
    public bool cameraEndPosition = false;

    void Update()
    {
        if (this.cameraEndPosition == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            this.cameraEndPosition = false;
        }
    }

	public void PlayGame()
    {
        this.mainmenuCamera.GetComponent<MoveCameraMenu>().startGame();
        
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
