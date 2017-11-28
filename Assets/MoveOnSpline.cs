﻿using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpline : MonoBehaviour
{
    private BGCurve curve;
    private BGCcMath mathe;
    private BGCcCursor cursor;

    private float min = 0.0f;
    private float max = 1.0f;
    private float steps = 1.0f;
    private float distanceRatio;
    private float seconds;

    private Transform bubbles;
    private Transform beforeBubble;
    private Transform afterBubble;

    private bool isFirstBubble;
    private bool isLastBubble;

    public delegate void lostGame();
    public static event lostGame onLostGame;

    private GameObject gamemaster;
    private GameMaster gameMasterAttributes;

    private Wavespawner wavespawner;

    private Bubble bubbleAttributes;
    private float animationEnd;
    public float animationStart = 0f;

    public float distanceCalc;


    void Start()
    {

        this.gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.gamemaster.GetComponent<GameMaster>();
        this.wavespawner = this.gamemaster.GetComponent<Wavespawner>();

        this.bubbleAttributes = transform.gameObject.GetComponent<Bubble>();

        this.bubbles = this.gameMasterAttributes.bubbles;

        this.distanceRatio = this.min;

        this.curve = this.bubbleAttributes.curve;

        this.mathe = this.bubbleAttributes.mathe;

        this.cursor = this.bubbleAttributes.cursor;

        this.beforeBubble = this.bubbleAttributes.beforeBubble;

        this.afterBubble = this.bubbleAttributes.afterBubble;

        this.isFirstBubble = bubbleAttributes.isFirstBubble;

        this.isLastBubble = bubbleAttributes.isLastBubble;

        this.animationEnd = this.gameMasterAttributes.bubbleSizeAverage;




    }


    void Update()
    {
        this.seconds = this.gamemaster.GetComponent<Wavespawner>().actualBubblespeed; 

        if (this.distanceRatio <= this.max)
        {
            if (!this.gameMasterAttributes.stopAll)
            {
                if (this.bubbleAttributes.interpolate)
                {
                    insertAnimation();
                }
                else
                {
                    moveOnSpline();
                }
            }  
        }

               /*
                   if (this.isFirstBubble)
                   {
                       transform.position = Waypoints.points[Waypoints.points.Length - 1].position;
                       //Debug.Log("Verloren");
                       onLostGame();
                   }
               }

           */

        }

    private void moveOnSpline()
    {
        this.distanceCalc += (this.mathe.GetDistance() * Time.deltaTime) / this.seconds;

        this.cursor.Distance = this.distanceCalc ;
        transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);

        if (!this.bubbleAttributes.isFirstBubble && this.wavespawner.rollInRow)
        {
            if ((this.bubbleAttributes.beforeBubble.GetComponent<MoveOnSpline>().distanceCalc) - (this.distanceCalc) != this.gameMasterAttributes.bubbleSizeAverage)
            {
                this.distanceCalc = this.bubbleAttributes.beforeBubble.GetComponent<MoveOnSpline>().distanceCalc - (this.gameMasterAttributes.bubbleSizeAverage);
            }
        }
    }

    public void insertAnimation()
    {
        if(this.animationStart < this.animationEnd)
        {
            this.animationStart += ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds) * 3;
            this.distanceCalc += ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds) * 4;
            this.cursor.Distance = this.distanceCalc;
            transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
        }
        else
        {
            setAnimationValuesBack();
        }
  
    }

    private void setAnimationValuesBack()
    {
        /*for (int i = 0; i < this.bubbles.childCount; i++)
        {
            this.bubbles.GetChild(i).GetComponent<MoveOnSpline>().animationStart = 0.0f;
            this.bubbles.GetChild(i).GetComponent<Bubble>().interpolate = false;
            this.gameMasterAttributes.stopAll = false;
        }*/
        this.bubbleAttributes.interpolate = false;
        this.animationStart = 0.0f;

        int interpolationCounter = 0;
        for (int i = 0; i < this.bubbles.childCount; i++)
        {
            if (this.bubbles.GetChild(i).GetComponent<Bubble>().interpolate)
            {
                interpolationCounter++;
            }
        }
        if(interpolationCounter == 0)
        {
            sortBubbles();
        }
     
        
    }

    private void sortBubbles()
    {
        for (int i = 0; i < this.bubbles.childCount; i++)
        {
            Bubble childBubbleAttr = this.bubbles.GetChild(i).GetComponent<Bubble>();
            MoveOnSpline childMoveOnSplineAttr = this.bubbles.GetChild(i).GetComponent<MoveOnSpline>();
            if (!childBubbleAttr.isFirstBubble)
            {
                childMoveOnSplineAttr.distanceCalc = childBubbleAttr.beforeBubble.GetComponent<MoveOnSpline>().distanceCalc - this.gameMasterAttributes.bubbleSizeAverage;
            }
        }
    }


   

}
