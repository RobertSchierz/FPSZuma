﻿using BansheeGz.BGSpline.Curve;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour
{

    private Transform[] bubblePrefabs;
    private int lastBubblePrefabindex;
    public Transform spawnPoint;
    private int prefabIndex;

    [Range(0, 10)]
    public int leveldifficult = 0;

 
    public static bool lostgame = false;
    


    private BGCurve curve;
    private Transform bubbles;

    //First Start
    private float countdown = 2f;
    private bool wavespaned = false;



    // Bubble Attributes
    public int bubbleCountPerWave = 1000000;

    [Range(15, 100)]
    public float bubblespeed = 15.0f;



    // First Start - Intro
    private int introBubbleNumber;
    private float introBubblespeed;
    public float actualBubblespeed;
    //public bool rollInRow = true;

    private bool spawnque = false;

    private GameMaster gameMasterAttributes;
    private Transform bubble;

    private int bubblesSpawned = 0;

    public GameObject gameoverCanvas;


    public GameObject minimapCanvas;
    public GameObject virtualcam1;
    public GameObject virtualcam2;

    private CinemachineVirtualCamera cvirtualcamera1;
    private CinemachineTrackedDolly ccameraDolly1;

    private CinemachineVirtualCamera cvirtualcamera2;
    private CinemachineTrackedDolly ccameraDolly2;

    private bool startCamera = false;
    private bool startCamera2 = false;

    public Transform chest;
    private bool chestAnimationPlayed = false;

    public GameObject highscoreCanvas;
    private bool destroyAllBubbles = false;


    void Start()
    {

        this.cvirtualcamera1 = this.virtualcam1.GetComponent<CinemachineVirtualCamera>();
        this.cvirtualcamera2 = this.virtualcam2.GetComponent<CinemachineVirtualCamera>();


        this.gameMasterAttributes = transform.gameObject.GetComponent<GameMaster>();

        this.bubblePrefabs = this.gameMasterAttributes.bubbleprefabs;

        this.curve = this.gameMasterAttributes.curve;
        this.spawnPoint.transform.position = this.curve.Points[0].PositionLocal;

        this.bubbles = this.gameMasterAttributes.bubbles;


        MoveOnSpline.onLostGame += handleOnLostGame;

        this.introBubbleNumber = /*this.bubbleCountPerWave */ 10;
        this.introBubblespeed = this.bubblespeed / 2f;
        this.actualBubblespeed = introBubblespeed;

    }


    void handleOnLostGame()
    {
        Debug.Log("Verloren");
        lostgame = true;
        StartCoroutine(lostGameHandler());
        
    }

    IEnumerator lostGameHandler()
    {
        this.gameMasterAttributes.audioManager.handleSound("End", 1);
        this.gameoverCanvas.SetActive(true);
        this.minimapCanvas.SetActive(false);
        yield return new WaitForSeconds(3);
        this.gameoverCanvas.SetActive(false);
        startCameraToHighscore();
    }

    private void startCameraToHighscore()
    {
        this.cvirtualcamera1.enabled = true;
        this.ccameraDolly1 = this.cvirtualcamera1.GetCinemachineComponent<CinemachineTrackedDolly>();
        this.ccameraDolly1.m_PathPosition = 0;
        this.startCamera = true;
    }

    IEnumerator highscoreCanvashandler()
    {
        yield return new WaitForSeconds(3);
        this.highscoreCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator waitForDestroy()
    {

        for (int i = 0; i < this.bubbles.childCount; i++)
        {
           
            transform.GetComponent<GameMaster>().startExplosionCoroutine(this.bubbles.GetChild(0).position, 2);
            Destroy(this.bubbles.GetChild(0).gameObject);
            this.gameMasterAttributes.audioManager.handleSound("FinalBallExplosion", 1);
            yield return new WaitForSeconds(0.2f);
            
        }
        
    }

    void Update()
    {


        if (this.startCamera)
        {
            if (this.ccameraDolly1.m_PathPosition < this.ccameraDolly1.m_Path.MaxPos)
            {
                this.ccameraDolly1.m_PathPosition += (Time.deltaTime);

                if (this.ccameraDolly1.m_PathPosition > 2.0f)
                {
                    if (!this.destroyAllBubbles)
                    {
                        this.destroyAllBubbles = true;
                        StartCoroutine(waitForDestroy());
                     
                    }
                   
                }

            }else
            {
                this.cvirtualcamera1.enabled = false;
                this.cvirtualcamera2.enabled = true;
                this.ccameraDolly2 = this.cvirtualcamera2.GetCinemachineComponent<CinemachineTrackedDolly>();
                this.ccameraDolly2.m_PathPosition = 0;
                this.startCamera = false;
                this.startCamera2 = true;

            }

        }

        if (this.startCamera2)
        {
            if (this.ccameraDolly2.m_PathPosition < this.ccameraDolly2.m_Path.MaxPos)
            {
                

                if (this.ccameraDolly2.m_PathPosition > this.ccameraDolly2.m_Path.MaxPos -1)
                {
                    this.ccameraDolly2.m_PathPosition += (Time.deltaTime / 2);
                    if (!this.chestAnimationPlayed)
                    {
                        this.chest.GetComponent<Animation>().Play();
                        this.chestAnimationPlayed = true;
                        
                        StartCoroutine(highscoreCanvashandler());
                    }
                }
                else
                {
                    this.ccameraDolly2.m_PathPosition += (Time.deltaTime);
                }

            }
            
                
                
            
        }


        if (countdown <= 0f && !this.wavespaned)
        {

            spawnBubble();
            this.wavespaned = true;
            this.spawnque = true;
            this.gameMasterAttributes.audioManager.handleSound("Wavespawn", 1);
            return;
        }
        else if (!this.wavespaned)
        {
            countdown -= Time.deltaTime;
        }



        if (this.spawnque && this.bubblesSpawned < this.bubbleCountPerWave && !lostgame)
        {
            

            if (this.bubbles.GetChild(this.bubbles.childCount - 1).GetComponent<Bubble>().cursor.Distance >= this.gameMasterAttributes.bubbleSizeAverage && !this.gameMasterAttributes.stopAll)
            {

                spawnBubble();

            }
            if (this.bubbles.childCount == this.introBubbleNumber)
            {
                this.actualBubblespeed = this.bubblespeed;
                this.gameMasterAttributes.audioManager.handleSound("Wavespawn", 2);
            }

        }
    }

    public Transform randomizePrefabs()
    {
        int randomPrefabIndex = Random.Range(0, 4);



        if (this.leveldifficult != 0)
        {

            if (this.lastBubblePrefabindex == randomPrefabIndex)
            {
                if (calculateNewMix())
                {
                    randomPrefabIndex = Random.Range(0, 4);
                }
            }
        }


        this.lastBubblePrefabindex = randomPrefabIndex;

        this.prefabIndex = randomPrefabIndex;

        return this.bubblePrefabs[randomPrefabIndex];
    }

    bool calculateNewMix()
    {
        int randomDetermitter = Random.Range(this.leveldifficult, 11);
        if (randomDetermitter > this.leveldifficult)
        {
            if (this.leveldifficult > 5 && this.leveldifficult < 10)
            {
                if (Random.Range(0, 2) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        else
        {
            return true;
        }


    }

    void spawnBubble()
    {
        this.bubblesSpawned++;
        this.bubble = Instantiate(this.randomizePrefabs(), spawnPoint.position, spawnPoint.rotation);
        this.bubble.transform.parent = this.bubbles.transform;
        this.bubble.GetComponent<Bubble>().bubbleColor = this.prefabIndex;

    }


}
