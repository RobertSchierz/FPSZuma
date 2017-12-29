using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MoveCameraMenu : MonoBehaviour {

    CinemachineVirtualCamera camera;
    CinemachineTrackedDolly cameraDolly;

    // Use this for initialization
    void Start () {
        this.camera = transform.GetComponent<CinemachineVirtualCamera>();
        this.cameraDolly = this.camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.cameraDolly.m_PathPosition = 0;

    }
	
	// Update is called once per frame
	void Update () {

        if (this.cameraDolly.m_PathPosition != this.cameraDolly.m_Path.MaxPos)
        {
            this.cameraDolly.m_PathPosition += Time.deltaTime;
        }
         
		
	}
}
