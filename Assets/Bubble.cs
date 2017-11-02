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
    public GameMaster gamemasterattributes;
    public Wavespawner wavespawner;

    [Space]
    [Header("Own Attributes")]
    public GameObject bubble;
    public int speciality;
    public bool isshooted = false;

    [Space]
    [Header("Math Attributes")]
    public BGCurve curve;
    public BGCcMath mathe;
    public BGCcCursor cursor;
    public float distance;
    public float distanceratio;


    [Space]
    [Header("Chain Attributes")]
    public Transform bubbles;
    public Transform beforebubble;
    public Transform afterbubble;
    public bool isfirstbubble = false;
    public bool islastbubble = false;
    public Transform[] movedbubblerow;
    public int bubblesinserted;

    void Start()
    {
        this.Gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gamemasterattributes = this.Gamemaster.GetComponent<GameMaster>();
        this.wavespawner = this.Gamemaster.GetComponent<Wavespawner>();

        this.bubble = transform.gameObject;
        this.bubbles = this.gamemasterattributes.bubbles;
        this.curve = this.gamemasterattributes.curve;
        this.mathe = this.curve.GetComponent<BGCcMath>();
        this.cursor = this.mathe.gameObject.AddComponent<BGCcCursor>();

        this.bubblesinserted = 0;

        this.movedbubblerow = new Transform[this.bubbles.childCount];

        if (!this.isshooted)
        {
            this.checkBubbleState();
        }

    }

   
    public void checkBubblerowInfront()
    {
       
            this.movedbubblerow = new Transform[this.bubble.transform.GetSiblingIndex() +1];
            for (int i = 0; i <= this.bubble.transform.GetSiblingIndex(); i++)
            {
                this.movedbubblerow[i] = this.bubbles.GetChild(i);
            }
        
      
    }


    public void checkBubbleState()
    {
        if (this.bubbles.GetChild(0).gameObject == gameObject)
        {
            this.isfirstbubble = true;
            this.movedbubblerow[0] = transform;
            //this.beforebubble = transform;
        }
        else
        {
            this.beforebubble = this.bubbles.GetChild(this.bubbles.childCount - 2);
            for (int i = 0; i < this.bubbles.childCount; i++)
            {
                this.movedbubblerow[i] = this.bubbles.GetChild(i);
            }
        }

        if (this.bubbles.childCount == this.wavespawner.bubblecountperwave)
        {
            this.islastbubble = true;
        }
       
        
        if (!this.isfirstbubble)
        {
            this.bubbles.GetChild(this.bubbles.childCount - 2).GetComponent<Bubble>().afterbubble = transform;
        }

        
    }

    void Update()
    {
        this.bubble.transform.rotation = Quaternion.identity;
        this.distance = this.cursor.Distance;
        this.distanceratio = this.cursor.DistanceRatio;
    }

}
