﻿using BansheeGz.BGSpline.Curve;
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

    private bool lostgame = false;


    private BGCurve curve;
    private Transform bubbles;

    //First Start
    private float countdown = 2f;
    private bool wavespaned = false;



    // Bubble Attributes
    public int bubbleCountPerWave = 10;
    [Range(15, 100)]
    public float bubblespeed = 15.0f;


    // First Start - Intro
    private int introBubbleNumber;
    private float introBubblespeed;
    public float actualBubblespeed;
    public bool rollInRow = true;

    private bool spawnque = false;

    private GameMaster gameMasterAttributes;
    private Transform bubble;




    void Start()
    {
        this.gameMasterAttributes = transform.gameObject.GetComponent<GameMaster>();

        this.bubblePrefabs = this.gameMasterAttributes.bubbleprefabs;

        this.curve = this.gameMasterAttributes.curve;
        this.spawnPoint.transform.position = this.curve.Points[0].PositionLocal;

        this.bubbles = this.gameMasterAttributes.bubbles;

        
        MoveOnSpline.onLostGame += handleOnLostGame;

        this.introBubbleNumber = this.bubbleCountPerWave / 10;
        this.introBubblespeed = this.bubblespeed / 5f;
        this.actualBubblespeed = introBubblespeed;


    }


    void handleOnLostGame()
    {
        this.lostgame = true;
    }


  

    void Update()
    {


        if (countdown <= 0f && !this.wavespaned)
        {

            spawnBubble();
            this.wavespaned = true;
            this.spawnque = true;
            return;
        }
        else if (!this.wavespaned)
        {
            countdown -= Time.deltaTime;
        }



        if (this.spawnque && this.bubbles.childCount < this.bubbleCountPerWave && !this.lostgame)
        {
         

            if (this.bubbles.GetChild(this.bubbles.childCount -1).GetComponent<Bubble>().cursor.Distance >= this.gameMasterAttributes.bubbleSizeAverage && !this.gameMasterAttributes.stopAll)
            {
                
                spawnBubble();


                /* Debug.Log("eigener: " +  this.bubble.GetComponent<MoveOnSpline>().distanceCalc);

                 this.bubble.GetComponent<MoveOnSpline>().distanceCalc = this.bubbles.GetChild(this.bubbles.childCount - 2).GetComponent<MoveOnSpline>().distanceCalc - this.gameMasterAttributes.bubbleSizeAverage;
                 Debug.Log("neuer: " + this.bubble.GetComponent<MoveOnSpline>().distanceCalc);

                 Debug.Log(this.bubbles.GetChild(this.bubbles.childCount - 2).GetComponent<MoveOnSpline>().distanceCalc - this.gameMasterAttributes.bubbleSizeAverage);

                 Debug.Break();
                 */



            }
            if (this.bubbles.childCount == this.introBubbleNumber)
            {
                this.actualBubblespeed = this.bubblespeed;
                this.rollInRow = false;
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

        this.bubble = Instantiate(this.randomizePrefabs(), spawnPoint.position, spawnPoint.rotation);
        this.bubble.transform.parent = this.bubbles.transform;
        this.bubble.GetComponent<Bubble>().bubbleColor = this.prefabIndex;
        




    }


}
