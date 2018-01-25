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

    //public bool helpmenu = false;


    private MainMenu mainMenu;


    // Use this for initialization
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

   /* public void helpMenuTrue()
    {
        this.helpmenu = true;
    }

    public void helpMenuFalse()
    {
        this.helpmenu = false;
    }*/


 
    void Update()
    {

        if (this.cameraDolly.m_PathPosition < this.cameraDolly.m_Path.MaxPos - 1)
        {
            this.cameraDolly.m_PathPosition += Time.deltaTime;
        }


        /*  if (!this.mainMenu.startgame)
          {
              if (!this.helpmenu)
              {
                  if (this.cameraDolly.m_PathPosition <= this.cameraDolly.m_Path.MaxPos - 2)
                  {
                      Debug.Log("1");
                      this.cameraDolly.m_PathPosition += Time.deltaTime;
                  }
                  else
                  {
                      if (this.cameraDolly.m_PathPosition > this.cameraDolly.m_Path.MaxPos - 2)
                      {
                          this.cameraDolly.m_PathPosition -= Time.deltaTime;
                      }
                  }
              }



              if (this.helpmenu)
              {
                  if (this.cameraDolly.m_PathPosition <= this.cameraDolly.m_Path.MaxPos - 1)
                  {
                      Debug.Log("2");
                      this.cameraDolly.m_PathPosition += Time.deltaTime;
                  }
              }
          }else
          {
              if (this.cameraDolly.m_PathPosition <= this.cameraDolly.m_Path.MaxPos)
              {
                 // this.cameraDolly.m_PathPosition += Time.deltaTime;
              }else
              {
                  this.mainMenu.startgame = false;
              }
          }*/

    }
}
