using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCameraMenu : MonoBehaviour {

    public GameObject mainmenuCanvas;
    CinemachineVirtualCamera camera;
    CinemachineTrackedDolly cameraDolly;

    // Use this for initialization
    void Start () {
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
            this.cameraDolly.m_PathPosition += (Time.deltaTime / 2 );
            yield return new WaitForEndOfFrame();
        }

        this.mainmenuCanvas.GetComponent<MainMenu>().cameraEndPosition= true;
    }

    // Update is called once per frame
    void Update () {

        
        if (this.cameraDolly.m_PathPosition < this.cameraDolly.m_Path.MaxPos - 1)
        {
            Debug.Log(this.cameraDolly.m_Path.MaxPos);
            this.cameraDolly.m_PathPosition += Time.deltaTime;
        }
         
	}
}
