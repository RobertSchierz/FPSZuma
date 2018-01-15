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
    private Rigidbody rigidBodyAttr;

    public float animationEnd;
    public float animationStart = 0f;
    public delegate void insertAnimationUpdate();
    public static event insertAnimationUpdate OnInsertAnimationUpdate;

    public int explosionCounter = 0;


    public float distanceCalc;
    public ExplosionProvider explosionProvider;

    //public bool helperWait = false;




    void Start()
    {

        this.gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.gamemaster.GetComponent<GameMaster>();
        this.wavespawner = this.gamemaster.GetComponent<Wavespawner>();

        this.bubbleAttributes = transform.gameObject.GetComponent<Bubble>();
        this.rigidBodyAttr = this.bubbleAttributes.rigidBodyAttr;

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

        this.explosionProvider = new ExplosionProvider(this.bubbleAttributes);

    }


    void Update()
    {

        this.seconds = this.gamemaster.GetComponent<Wavespawner>().actualBubblespeed;

        if (this.cursor.DistanceRatio <= this.max && !Wavespawner.lostgame)
        {
            if (this.isFirstBubble && transform.position == Waypoints.points[Waypoints.points.Length - 1].position)
            {
                onLostGame();
            }

            checkDistances();

            if (this.bubbleAttributes.isShooted && !transform.GetComponent<ShootedBubble>().isInRow)
            {

                insertAnimation(3);

            }
            else
            {


                if (this.bubbleAttributes.isShooted && transform.GetComponent<ShootedBubble>().isInRow && transform.GetComponent<ShootedBubble>().explode)
                {
                    this.bubbleAttributes.isShooted = false;
                    this.explosionProvider.handleExplosion(transform, 1);
                }

                if (this.explosionCounter == 0)
                {
                    if (this.bubbleAttributes.interpolate)
                    {
                        insertAnimation(1);
                    }
                    else
                    {
                        moveOnSpline();
                    }
                }
                else
                {
                    if (this.bubbleAttributes.interpolate)
                    {
                        insertAnimation(1);

                    }
                    else
                    {
                        if (this.bubbleAttributes.rollback)
                        {
                            insertAnimation(2);
                        }
                        else
                        {
                            this.cursor.Distance = this.distanceCalc;
                            transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);

                        }
                    }

                }

            }
        }
        else
        {


            // Helper->
            transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
        }







    }

    public void insertAnimation(int decision)
    {

        switch (decision)
        {
            case 1:

                if (this.animationStart < this.animationEnd)
                {

                    if (this.explosionCounter != 0)
                    {
                        this.animationStart += ((this.mathe.GetDistance() * (Time.deltaTime * 2)) / this.seconds);
                        this.distanceCalc += ((this.mathe.GetDistance() * (Time.deltaTime * 2)) / this.seconds);
                    }
                    else
                    {
                        this.animationStart += ((this.mathe.GetDistance() * (Time.deltaTime * 2)) / this.seconds);
                        this.distanceCalc += ((this.mathe.GetDistance() * (Time.deltaTime * 3)) / this.seconds);
                    }


                }
                else
                {
                    //moveOnSpline();
                    setAnimationValuesBack();
                }

                moveToCalcDistance();

                break;

            case 2:

                try
                {
                    /*
                    Bubble rollbackBorderBubbleAttr = this.bubbleAttributes.rollbackBorderBubble.GetComponent<Bubble>();
                    MoveOnSpline rollbackBorderMoveOnSplineAttr = this.bubbleAttributes.rollbackBorderBubble.GetComponent<MoveOnSpline>();


                    if ((rollbackBorderMoveOnSplineAttr.distanceCalc - (Time.deltaTime)) <= (rollbackBorderBubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttributes.bubbleSizeAverage))
                    {
                        this.bubbleAttributes.rollback = false;
                    }*/


                    this.distanceCalc -= (this.mathe.GetDistance() * Time.deltaTime) / this.seconds * 4;

                    moveToCalcDistance();
                    break;


                }
                catch (System.Exception)
                {

                    throw;
                }


            case 3:

                ShootedBubble shootedBubbleAttr = transform.GetComponent<ShootedBubble>();


                if (this.cursor.Distance != 0.0f)
                {

                    if (Vector3.Distance(transform.position, this.mathe.CalcPositionByDistance(this.cursor.Distance)) > 0.01f)
                    {
                        if (this.bubbleAttributes.interpolate)
                        {
                            this.distanceCalc += this.gameMasterAttributes.bubbleSizeAverage;
                            this.bubbleAttributes.interpolate = false;
                        }

                        Vector3 direction = (this.mathe.CalcPositionByDistance(this.cursor.Distance) - transform.position);

                        if (Vector3.Distance(transform.position, this.mathe.CalcPositionByDistance(this.cursor.Distance)) > shootedBubbleAttr.distanceToInsertionspoint || shootedBubbleAttr.distanceToInsertionspoint == 0f)
                        {
                            this.rigidBodyAttr.drag = 1000;
                            this.rigidBodyAttr.angularDrag = 1000;
                            this.rigidBodyAttr.velocity = Vector3.zero;
                            this.rigidBodyAttr.MovePosition(transform.position + direction * (Time.deltaTime * 15));
                        }
                        else
                        {
                            this.rigidBodyAttr.velocity = Vector3.zero;
                            this.rigidBodyAttr.MovePosition(transform.position + direction * (Time.deltaTime * 20));


                        }



                        Debug.DrawRay(transform.position, direction, Color.red, Mathf.Infinity);
                        shootedBubbleAttr.distanceToInsertionspoint = Vector3.Distance(transform.position, this.mathe.CalcPositionByDistance(this.cursor.Distance));

                    }
                    else
                    {

                        shootedBubbleAttr.isInRow = true;
                        this.rigidBodyAttr.drag = 0;
                        this.rigidBodyAttr.angularDrag = 0;
                        this.rigidBodyAttr.isKinematic = true;
                        //this.bubbleAttributes.isShooted = false;
                        /*if (shootedBubbleAttr.explode)
                        {
                            shootedBubbleAttr.explosionProvider.handleExplosion(1);
                        }*/

                    }

                }

                if (transform.GetComponent<MoveOnSpline>().explosionCounter == 0 || this.cursor.Distance == 0.0f)
                {
                    this.distanceCalc += (this.mathe.GetDistance() * Time.deltaTime) / this.seconds;
                    this.cursor.Distance = this.distanceCalc;
                }



                break;
            default:
                Debug.LogError("Fehler bei Animation");
                break;



        }




        //transform.GetComponent<Rigidbody>().position = this.mathe.CalcPositionByDistance(this.cursor.Distance);


    }

    private void moveToCalcDistance()
    {
        this.cursor.Distance = this.distanceCalc;
        transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
    }


    private void checkDistances()
    {
        if (this.bubbleAttributes.rollback)
        {
            MoveOnSpline rollBackMoveOnSpline = null;
            Bubble rollBackBubbleBubbleAttr = null;


            for (int i = 0; i < this.bubbles.childCount; i++)
            {
                if (this.bubbles.GetChild(i).GetComponent<MoveOnSpline>().explosionCounter == this.explosionCounter)
                {
                    if (this.bubbles.GetChild(i).GetComponent<Bubble>().isRollbackBorderBubble)
                    {
                        rollBackMoveOnSpline = this.bubbles.GetChild(i).GetComponent<MoveOnSpline>();
                        rollBackBubbleBubbleAttr = this.bubbles.GetChild(i).GetComponent<Bubble>();
                    }
                }
            }

            /*MoveOnSpline rollBackMoveOnSpline = this.bubbleAttributes.rollbackBorderBubble.GetComponent<MoveOnSpline>();
            Bubble rollBackBubbleBubbleAttr = this.bubbleAttributes.rollbackBorderBubble.GetComponent<Bubble>();
            */

            if (!(rollBackMoveOnSpline == null || rollBackBubbleBubbleAttr == null))
            {
                if ((rollBackMoveOnSpline.distanceCalc - ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds * 4) <= rollBackBubbleBubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttributes.bubbleSizeAverage))
                {
                    this.explosionCounter = rollBackBubbleBubbleAttr.afterBubble.GetComponent<MoveOnSpline>().explosionCounter;
                    this.distanceCalc = this.bubbleAttributes.afterBubble.GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttributes.bubbleSizeAverage;
                    moveToCalcDistance();
                    this.bubbleAttributes.rollback = false;
                    this.bubbleAttributes.isRollbackBorderBubble = false;
                    this.explosionProvider.handleExplosion(rollBackBubbleBubbleAttr.transform, 2);

                }
            }else
            {
               // Debug.Break();
                this.bubbleAttributes.rollback = false;
            }

    
        }
        else
        {
            if (!this.bubbleAttributes.isFirstBubble)
            {

                if (this.explosionCounter != this.bubbleAttributes.beforeBubble.GetComponent<MoveOnSpline>().explosionCounter)
                {

                    float calculationValue = (this.bubbleAttributes.beforeBubble.GetComponent<MoveOnSpline>().distanceCalc - this.gameMasterAttributes.bubbleSizeAverage);

                    if (this.distanceCalc + ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds) >= calculationValue)
                    {
                        correctOverlapOfBubbles(this.distanceCalc - calculationValue);
                    }




                }
            }
        }
    }

    public void correctOverlapOfBubbles(float distanceCalcDifference)
    {
        this.gameMasterAttributes.audioManager.handleSound("BubblesTouch", 1);
        foreach (var infrontBubble in this.bubbleAttributes.beforeBubble.GetComponent<Bubble>().movedBubbleRow)
        {
            MoveOnSpline infrontBubbleMoveOnSpline = infrontBubble.GetComponent<MoveOnSpline>();
            Bubble infrontBubbleBubbleAttr = infrontBubble.GetComponent<Bubble>();

            if (infrontBubbleMoveOnSpline.explosionCounter == this.explosionCounter + 1)
            {
                infrontBubbleMoveOnSpline.distanceCalc += distanceCalcDifference;
                moveToCalcDistance();

                if (!this.bubbleAttributes.interpolate)
                {
                    infrontBubbleMoveOnSpline.explosionCounter = this.explosionCounter;

                }

            }
            else
            {
                if (infrontBubbleMoveOnSpline.explosionCounter != infrontBubbleBubbleAttr.afterBubble.GetComponent<MoveOnSpline>().explosionCounter && infrontBubbleBubbleAttr.afterBubble.GetComponent<MoveOnSpline>().explosionCounter == 0)
                {
                    infrontBubbleMoveOnSpline.explosionCounter = infrontBubbleBubbleAttr.afterBubble.GetComponent<MoveOnSpline>().explosionCounter;

                }
            }
        }
    }

    private void handleExplosionWait()
    {
        if (this.bubbleAttributes.afterBubble.GetComponent<MoveOnSpline>().explosionCounter == 0)
        {
            if (this.bubbleAttributes.afterBubble.GetComponent<MoveOnSpline>().distanceCalc >= (this.distanceCalc - this.gameMasterAttributes.bubbleSizeAverage))
            {
                foreach (var bubble in this.bubbleAttributes.movedBubbleRow)
                {
                    bubble.GetComponent<Bubble>().interpolate = false;
                    bubble.GetComponent<MoveOnSpline>().explosionCounter = 0;
                }
            }
        }



    }

    private void moveOnSpline()
    {
       // if (!this.helperWait)
       // {
            this.distanceCalc += (this.mathe.GetDistance() * Time.deltaTime) / this.seconds;
            this.cursor.Distance = this.distanceCalc;
            transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
      //  }



    }





    private void setAnimationValuesBack()
    {
        this.bubbleAttributes.interpolate = false;
        this.animationStart = 0.0f;
        OnInsertAnimationUpdate();

    }






}
