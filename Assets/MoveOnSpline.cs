using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public float slowBackSeconds = 2.0f;
    public float slowBackAnimationStart;

    public delegate void insertAnimationUpdate();
    public static event insertAnimationUpdate OnInsertAnimationUpdate;

    public int explosionCounter = 0;


    public float distanceCalc;
    public ExplosionProvider explosionProvider;

    public bool slowAfterRollback = false;
    public float slowdownFactor = 2.0f;

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

        calcTheCase();

        this.seconds = this.gamemaster.GetComponent<Wavespawner>().actualBubblespeed;

    }

    private void calcTheCase()
    {
        if (this.cursor.DistanceRatio <= this.max && !Wavespawner.lostgame)
        {
            if (transform.position == Waypoints.points[Waypoints.points.Length - 1].position)
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


                if (this.slowAfterRollback)
                {
                    if (this.bubbleAttributes.interpolate)
                    {
                        insertAnimation(1);
                    }

                    insertAnimation(4);



                }
                else
                {
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

                float bubbleBeforeAfterCalcValue = 0.0f;

                if (shootedBubbleAttr.insertAfter)
                {
                    bubbleBeforeAfterCalcValue = (-this.gameMasterAttributes.bubbleSizeAverage);
                } else if (shootedBubbleAttr.insertBefore)
                {
                    bubbleBeforeAfterCalcValue = (+this.gameMasterAttributes.bubbleSizeAverage);
                } else if (shootedBubbleAttr.insertAfter == false && shootedBubbleAttr.insertBefore == false)
                {
                    bubbleBeforeAfterCalcValue = (+this.gameMasterAttributes.bubbleSizeAverage);
                }


                


                if (this.cursor.Distance != 0.0f)
                {



                    if (Vector3.Distance(transform.position, this.mathe.CalcPositionByDistance(shootedBubbleAttr.targetMoveonspline.cursor.Distance + bubbleBeforeAfterCalcValue)) > 0.01f)
                    {


                        Vector3 direction = (this.mathe.CalcPositionByDistance(shootedBubbleAttr.targetMoveonspline.cursor.Distance + bubbleBeforeAfterCalcValue) - transform.position);

                        if (Vector3.Distance(transform.position, this.mathe.CalcPositionByDistance(shootedBubbleAttr.targetMoveonspline.cursor.Distance + bubbleBeforeAfterCalcValue)) > shootedBubbleAttr.distanceToInsertionspoint || shootedBubbleAttr.distanceToInsertionspoint == 0f)
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
                        shootedBubbleAttr.distanceToInsertionspoint = Vector3.Distance(transform.position, this.mathe.CalcPositionByDistance(shootedBubbleAttr.targetMoveonspline.cursor.Distance + bubbleBeforeAfterCalcValue));

                    }

                    else
                    {
                        this.distanceCalc = shootedBubbleAttr.targetMoveonspline.distanceCalc + bubbleBeforeAfterCalcValue;
                        if (shootedBubbleAttr.targetMoveonspline.slowAfterRollback)
                        {
                            this.slowAfterRollback = true;
                            this.slowdownFactor = shootedBubbleAttr.targetMoveonspline.slowdownFactor;
                            this.slowBackAnimationStart = shootedBubbleAttr.targetMoveonspline.slowdownFactor;
                        }
                        shootedBubbleAttr.isInRow = true;
                        this.rigidBodyAttr.drag = 0;
                        this.rigidBodyAttr.angularDrag = 0;
                        this.rigidBodyAttr.isKinematic = true;


                    }

                }

                if (transform.GetComponent<MoveOnSpline>().explosionCounter == 0 || this.cursor.Distance == 0.0f)
                {
                    this.distanceCalc += (this.mathe.GetDistance() * Time.deltaTime) / this.seconds;
                    this.cursor.Distance = this.distanceCalc;
                }



                break;

            case 4:

                if (this.slowBackAnimationStart < this.slowBackSeconds)
                {

                    this.distanceCalc -= (this.mathe.GetDistance() * (Time.deltaTime * this.slowdownFactor)) / this.seconds;


                    this.slowBackAnimationStart += Time.deltaTime;


                    if (this.slowdownFactor > 0.0f)
                    {
                        this.slowdownFactor -= Time.deltaTime;
                    }
                    else
                    {
                        this.slowdownFactor = 0.0f;
                    }


                    moveOnSpline();
                }
                else
                {
                    this.slowBackAnimationStart = 0.0f;
                    this.slowAfterRollback = false;
                    this.slowdownFactor = 2.0f;
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

                    handleslowRollback(this.explosionProvider.handleExplosion(rollBackBubbleBubbleAttr.transform, 2), rollBackMoveOnSpline, rollBackBubbleBubbleAttr);

                }
            }
            else
            {
                this.bubbleAttributes.rollback = false;
                // handleslowRollback();
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

    private void handleslowRollback(bool explode, MoveOnSpline rollbackMoveonSpline, Bubble rollbackBubble)
    {
        Debug.Log(explode);
        if (explode == false && rollbackMoveonSpline.transform == transform)
        {

            // Debug.Break();

            for (int i = 0; i < this.bubbles.childCount; i++)
            {
                MoveOnSpline tempMoveonSpline = this.bubbles.GetChild(i).GetComponent<MoveOnSpline>();
                if (tempMoveonSpline.explosionCounter == rollbackMoveonSpline.explosionCounter || tempMoveonSpline.explosionCounter == rollbackMoveonSpline.explosionCounter - 1)
                {
                    tempMoveonSpline.slowAfterRollback = true;
                }
            }


            /*
            for (int i = 0; i < this.bubbles.childCount; i++)
            {
                MoveOnSpline moveonsplineAttr = this.bubbles.GetChild(i).GetComponent<MoveOnSpline>();
                Bubble bubbleeAttr = this.bubbles.GetChild(i).GetComponent<Bubble>();

                if (moveonsplineAttr.explosionCounter == this.explosionCounter)
                {
                    if (!this.bubbleAttributes.isLastBubble)
                    {
                        moveonsplineAttr.slowAfterRollback = true;
                        moveonsplineAttr.slowBackAnimationStart = this.bubbles.GetChild(i+1).GetComponent<MoveOnSpline>().slowBackAnimationStart;
                        moveonsplineAttr.slowdownFactor = this.bubbles.GetChild(i + 1).GetComponent<MoveOnSpline>().slowdownFactor;
                    }else
                    {
                        moveonsplineAttr.slowAfterRollback = true;
                        moveonsplineAttr.slowBackAnimationStart = this.bubbles.GetChild(i - 1).GetComponent<MoveOnSpline>().slowBackAnimationStart;
                        moveonsplineAttr.slowdownFactor = this.bubbles.GetChild(i - 1).GetComponent<MoveOnSpline>().slowdownFactor;
                    }

                    // moveonsplineAttr.slowAfterRollback = true;

                }
                else if (moveonsplineAttr.explosionCounter == this.explosionCounter + 1)
                {
                    Debug.Log(explode);
                    if (!explode)
                    {
                        // moveonsplineAttr.slowAfterRollback = true;
                    }
                    else
                    {
                        // moveonsplineAttr.slowAfterRollback = false;
                    }
                }
            }*/
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

    //Helper
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
