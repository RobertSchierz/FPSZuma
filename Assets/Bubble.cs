using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Bubble : MonoBehaviour
{
    [Header("Controller Attributes")]
    public GameObject Gamemaster;
    public GameMaster gameMasterAttributes;
    public Wavespawner waveSpawner;

    [Space]
    [Header("Own Attributes")]
    public GameObject bubble;
    public int speciality;
    public bool isShooted = false;
    public int bubbleColor;

    [Space]
    [Header("Math Attributes")]
    public BGCurve curve;
    public BGCcMath mathe;
    public BGCcCursor cursor;



    [Space]
    [Header("Chain Attributes")]
    public Transform bubbles;
    public Transform beforeBubble;
    public Transform afterBubble;
    public bool isFirstBubble = false;
    public bool isLastBubble = false;
    public Transform[] movedBubbleRow;
    public bool interpolate = false;
    

    void Start()
    {
        this.Gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.Gamemaster.GetComponent<GameMaster>();
        this.waveSpawner = this.Gamemaster.GetComponent<Wavespawner>();

        this.bubble = transform.gameObject;
        this.bubbles = this.gameMasterAttributes.bubbles;
        this.curve = this.gameMasterAttributes.curve;
        this.mathe = this.curve.GetComponent<BGCcMath>();
        this.cursor = this.mathe.gameObject.AddComponent<BGCcCursor>();



        this.movedBubbleRow = new Transform[this.bubbles.childCount];

        if (!this.isShooted)
        {
            this.checkBubbleState();
            if (!this.isFirstBubble) {
                this.bubble.GetComponent<MoveOnSpline>().distanceCalc = this.bubble.GetComponent<Bubble>().beforeBubble.GetComponent<MoveOnSpline>().distanceCalc - this.gameMasterAttributes.bubbleSizeAverage;
            }

        }

    }

   
    public void checkBubblerowInfront()
    {
       
            this.movedBubbleRow = new Transform[this.bubble.transform.GetSiblingIndex() +1];
            for (int i = 0; i <= this.bubble.transform.GetSiblingIndex(); i++)
            {
                this.movedBubbleRow[i] = this.bubbles.GetChild(i);
            }
        
      
    }


    public void checkBubbleState()
    {
        if (this.bubbles.GetChild(0).gameObject == gameObject)
        {
            this.isFirstBubble = true;
            this.movedBubbleRow[0] = transform;
            
        }
        else
        {
            this.beforeBubble = this.bubbles.GetChild(this.bubbles.childCount - 2);
            for (int i = 0; i < this.bubbles.childCount; i++)
            {
                this.movedBubbleRow[i] = this.bubbles.GetChild(i);
            }
        }

        if (this.bubbles.childCount == this.waveSpawner.bubbleCountPerWave)
        {
            this.isLastBubble = true;
        }
       
        
        if (!this.isFirstBubble)
        {
            this.bubbles.GetChild(this.bubbles.childCount - 2).GetComponent<Bubble>().afterBubble = transform;
        }

        
    }

    void Update()
    {
        this.bubble.transform.rotation = Quaternion.identity;      
    }

}
