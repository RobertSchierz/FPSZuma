﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class HighscoreAnzeige : MonoBehaviour {



    public GameObject virtualcam;
    private CinemachineVirtualCamera cvirtualcamera;

    public GameObject virtualcam2;
    private CinemachineVirtualCamera cvirtualcamera2;

    public GameObject virtualcam3;
    private CinemachineVirtualCamera cvirtualcamera3;

    public GameObject virtualcam4;
    private CinemachineVirtualCamera cvirtualcamera4;

    public GameObject track1;
    public GameObject track2;
    public GameObject track3;


    private CinemachineTrackedDolly dolly;
    private CinemachineTrackedDolly dolly2;
    private CinemachineTrackedDolly dolly3;
    private CinemachineTrackedDolly dolly4;

    public Transform entryPrefab;
    public Transform grid;


    public bool startCamToHighscore = false;
    public bool startCamToMainmenu = false;
    public bool startCamZoom = false;
    public bool startCamZoomOut = false;




    void Start () {
        this.cvirtualcamera = this.virtualcam.GetComponent<CinemachineVirtualCamera>();
        this.cvirtualcamera2 = this.virtualcam2.GetComponent<CinemachineVirtualCamera>();
        this.cvirtualcamera3 = this.virtualcam3.GetComponent<CinemachineVirtualCamera>();
        this.cvirtualcamera4 = this.virtualcam4.GetComponent<CinemachineVirtualCamera>();


        this.dolly2 = this.cvirtualcamera2.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.dolly3 = this.cvirtualcamera3.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.dolly3.m_PathPosition = this.dolly3.m_Path.MaxPos;
        this.dolly4 = this.cvirtualcamera4.GetCinemachineComponent<CinemachineTrackedDolly>();

    }


	void Update () {

        if (this.startCamToHighscore && this.dolly2.m_PathPosition < (this.dolly2.m_Path.MaxPos))
        {
            this.dolly2.m_PathPosition += Time.deltaTime;
        }else
        {
            this.startCamToHighscore = false;
     
        }

        if (this.startCamToMainmenu)
        {
            if (this.dolly3.m_PathPosition >= this.dolly3.m_Path.MinPos)
            {
                this.dolly3.m_PathPosition -= Time.deltaTime;
                
            }
            else
            {
                this.cvirtualcamera3.enabled = false;
                this.cvirtualcamera.enabled = true;
                this.startCamToMainmenu = false;
            }

        }

        if (this.startCamZoom && this.dolly4.m_PathPosition < (this.dolly4.m_Path.MaxPos))
        {
            if (this.startCamZoomOut)
            {
                this.startCamZoomOut = false;
            }

            this.dolly4.m_PathPosition += Time.deltaTime;
        }

        if (startCamZoomOut)
        {
            if (this.dolly4.m_PathPosition > (this.dolly4.m_Path.MinPos))
            {
                this.dolly4.m_PathPosition -= Time.deltaTime;
            }
            else
            {
                startCamZoomOut = false;
                this.cvirtualcamera4.enabled = false;
                this.cvirtualcamera.enabled = true;
            }
          
        }


    }

    public void zoomCam()
    {
        if (this.startCamToMainmenu)
        {
            this.cvirtualcamera4.enabled = false;
            this.startCamToMainmenu = false;
        }

        this.cvirtualcamera.enabled = false;
        this.cvirtualcamera2.enabled = false;
        this.cvirtualcamera3.enabled = false;
        this.cvirtualcamera4.enabled = true;
        this.startCamZoom = true;
    }

    public void zoomBack()
    {
        this.startCamZoom = false;
        this.startCamZoomOut = true;
    }

    public void resetAll()
    {
        try
        {
            for (int i = 0; i < this.grid.childCount; i++)
            {
                Destroy(this.grid.GetChild(i).gameObject);
            }

            File.Create(Application.persistentDataPath + "/savedHighscores.dat");
            SaveLoadScript.instance.setHighscoreToNull();
        }
        catch (System.Exception)
        {

            Debug.Log("Datei ist nicht vorhanden");
        }
      
    }

  


    public void handleCam()
    {

        if (this.startCamZoomOut)
        {
            this.cvirtualcamera4.enabled = false;
            this.startCamZoomOut = false;
        }
        this.cvirtualcamera.enabled = false;
        this.cvirtualcamera2.enabled = true;
        this.startCamToHighscore = true;

        Hashtable highscores;
        highscores = SaveLoadScript.instance.getHighscores();

        

        if (highscores != null)
        {
            for (int i = 0; i < this.grid.childCount; i++)
            {
                Destroy(this.grid.GetChild(i).gameObject);
            }

            Hashtable sortedHighscore = new Hashtable();

            

            foreach (DictionaryEntry entry in highscores)
            {
                sortedHighscore.Add(entry.Key.ToString(), int.Parse(entry.Value.ToString()));
            }


            foreach (DictionaryEntry entry in sortedHighscore.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value))
            {
                
                Transform holder = Instantiate(this.entryPrefab, this.grid);
                holder.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Key.ToString();
                holder.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.Value.ToString();


            }
        }
   
    }

    public void backToMainMenu()
    {

        this.startCamToMainmenu = true;
        this.cvirtualcamera2.enabled = false;
        this.cvirtualcamera3.enabled = true;


    }

}
