using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class MoveCameraMenu : MonoBehaviour
{

    public GameObject mainmenuCanvas;
    CinemachineVirtualCamera camera;
    CinemachineTrackedDolly cameraDolly;

    private MainMenu mainMenu;



    void Start()
    {
        this.mainMenu = this.mainmenuCanvas.GetComponent<MainMenu>();
        this.camera = transform.GetComponent<CinemachineVirtualCamera>();
        this.cameraDolly = this.camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.cameraDolly.m_PathPosition = 0;

    }

    public void startGame()
    {

        this.camera.m_LookAt = null;
        StartCoroutine(MoveCameraToEnd());
    }

    public IEnumerator MoveCameraToEnd()
    {

        while (this.cameraDolly.m_PathPosition < this.cameraDolly.m_Path.MaxPos)
        {
            this.cameraDolly.m_PathPosition += (Time.deltaTime / 2);
            yield return new WaitForEndOfFrame();
        }

        this.mainMenu.cameraEndPosition = true;
    }


 
    void Update()
    {

        if (this.cameraDolly.m_PathPosition < this.cameraDolly.m_Path.MaxPos - 1)
        {
            this.cameraDolly.m_PathPosition += Time.deltaTime;
        }

    }
}
