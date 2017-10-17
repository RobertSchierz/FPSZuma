﻿using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bubble : MonoBehaviour
{
    [Header("Controller Attributes")]
    public GameObject Gamemaster;
    private GameMaster gamemasterattributes;
    private Wavespawner wavespawner;

    [Space]
    [Header("Own Attributes")]
    public GameObject bubble;
    public int speciality;

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


        this.checkBubbleState();


        



    }

    void checkBubbleState()
    {
        if (this.bubbles.GetChild(0).gameObject == gameObject)
        {
            this.isfirstbubble = true;
        }
        else
        {
            this.beforebubble = this.bubbles.GetChild(this.bubbles.childCount - 2);
        }

        if (this.bubbles.childCount == this.wavespawner.bubblecountperwave)
        {
            this.islastbubble = true;
        }else
        {
            if (!this.isfirstbubble)
            {
                this.bubbles.GetChild(this.bubbles.childCount - 2).GetComponent<Bubble>().afterbubble = transform;
            }
            
        }
    }

    void update()
    {

    }

}
