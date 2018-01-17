using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootedBubble : MonoBehaviour
{


    public bool hitted = false;


    public GameObject targetBubble;
    public Bubble targetBubbleattr;
    public MoveOnSpline targetMoveonspline;


    public Transform bubbles;
    private Bubble bubbleAttr;
    private GameObject gameMaster;
    private GameMaster gameMasterAttr;
    private MoveOnSpline moveOnSplineAttr;
    private Wavespawner waveSpawner;

    private bool insertBefore = false;
    private bool insertAfter = false;

    public int animateBubbleCount;
    private int explosionBubblesCount = 0;
    public bool isInRow = false;
    public float distanceToInsertionspoint = 0f;
    public ExplosionProvider explosionProvider;
    public bool explode = false;



    void Start()
    {
        this.bubbleAttr = gameObject.GetComponent<Bubble>();
        this.gameMaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttr = this.gameMaster.GetComponent<GameMaster>();
        this.bubbles = this.bubbleAttr.bubbles;
        this.waveSpawner = this.bubbleAttr.waveSpawner;
        this.explosionProvider = new ExplosionProvider(this.bubbleAttr);


    }

    void OnCollisionEnter(Collision collision)
    {
       

        if (collision.contacts[0].otherCollider.gameObject.tag == "Bubble" && !this.hitted)
        {

            this.targetBubble = collision.contacts[0].otherCollider.gameObject;
            this.targetBubbleattr = this.targetBubble.GetComponent<Bubble>();
            this.targetMoveonspline = this.targetBubble.GetComponent<MoveOnSpline>();


            if (!this.targetBubbleattr.rollback)
            {
                targetBubblehandler();
            }
        }

    }

    private void targetBubblehandler()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        MoveOnSpline.OnInsertAnimationUpdate += checkInsertionAnimationUpdate;
        this.hitted = true;

        Bubble targetBubbleAttr = targetBubble.GetComponent<Bubble>();
        MoveOnSpline targetMoveOnSplineAttr = targetBubble.GetComponent<MoveOnSpline>();
        handleInsertBubble(targetBubbleAttr, targetMoveOnSplineAttr);
        this.gameMasterAttr.audioManager.handleSound("CollideBubble1", 1);
    }

    void checkInsertionAnimationUpdate()
    {
        this.animateBubbleCount--;
        if (this.animateBubbleCount == 0)
        {
      
            MoveOnSpline.OnInsertAnimationUpdate -= checkInsertionAnimationUpdate;
            this.explode = true;

        }
    }




    private void handleInsertBubble(Bubble targetbubbleattr, MoveOnSpline targetMoveOnSplineAttr)
    {
        Vector3 bubblePosBefore = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.cursor.Distance + this.gameMasterAttr.bubbleSizeAverage);
        Vector3 bubblePosAfter = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.cursor.Distance - this.gameMasterAttr.bubbleSizeAverage);

        float distanceBubblePosBefore = Vector3.Distance(bubblePosBefore, transform.position);
        float distanceBubblePosAfter = Vector3.Distance(bubblePosAfter, transform.position);

        if (distanceBubblePosAfter > distanceBubblePosBefore)
        {
            this.insertBefore = true;
            //Debug.Log("before");

        }
        else if (distanceBubblePosAfter < distanceBubblePosBefore)
        {
            this.insertAfter = true;
            //Debug.Log("after");

        }

        insertedBubbleHandler(targetbubbleattr, targetMoveOnSplineAttr);



    }


    private void insertedBubbleHandler(Bubble targetbubbleattr, MoveOnSpline targetMoveOnSplineAttr)
    {
        if (this.insertBefore)
        {
            gameObject.AddComponent<MoveOnSpline>();
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedBubbleRow.Length - 1);
            setHirarchyIndex(targetbubbleattr, this.targetBubble.transform.GetSiblingIndex());
            this.moveOnSplineAttr = gameObject.GetComponent<MoveOnSpline>();


            setNewValuesForBubbles(targetbubbleattr, targetMoveOnSplineAttr);
        }
        else if (this.insertAfter)
        {
            gameObject.AddComponent<MoveOnSpline>();
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedBubbleRow.Length);
            setHirarchyIndex(targetbubbleattr, this.targetBubble.transform.GetSiblingIndex() + 1);
            this.moveOnSplineAttr = gameObject.GetComponent<MoveOnSpline>();

      
         

            setNewValuesForBubbles(targetbubbleattr, targetMoveOnSplineAttr);

        }
    }


    private void setNewValuesForBubbles(Bubble targetBubbleAttr, MoveOnSpline targetMoveOnSplineAttr)
    {
        if (this.insertBefore)
        {
            if (targetBubbleAttr.isFirstBubble)
            {
                targetBubbleAttr.isFirstBubble = false;
                this.bubbleAttr.isFirstBubble = true;
                targetBubbleAttr.beforeBubble = transform;
                this.bubbleAttr.afterBubble = targetBubbleAttr.transform;
                this.moveOnSplineAttr.distanceCalc = targetMoveOnSplineAttr.distanceCalc + this.gameMasterAttr.bubbleSizeAverage;

                if (targetBubble.GetComponent<MoveOnSpline>().explosionCounter != 0)
                {
                    this.moveOnSplineAttr.explosionCounter = targetBubble.GetComponent<MoveOnSpline>().explosionCounter;
                }

                this.explosionProvider.handleExplosion(transform, 1);
            }
            else
            {
                targetBubbleAttr.beforeBubble.GetComponent<Bubble>().afterBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubbleAttr.beforeBubble;
                this.bubbleAttr.afterBubble = this.targetBubble.transform;
                targetBubbleAttr.beforeBubble = transform;
                this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttr.bubbleSizeAverage;
                if (targetBubble.GetComponent<MoveOnSpline>().explosionCounter != 0)
                {
                    this.moveOnSplineAttr.explosionCounter = targetBubble.GetComponent<MoveOnSpline>().explosionCounter;
                }

                if (this.bubbleAttr.beforeBubble.GetComponent<MoveOnSpline>().explosionCounter != this.moveOnSplineAttr.explosionCounter)
                {
                    this.explosionProvider.handleExplosion(transform, 1);
                }
            }

        }
        else if (this.insertAfter)
        {
            if (targetBubbleAttr.isLastBubble)
            {
                targetBubbleAttr.isLastBubble = false;
                this.bubbleAttr.isLastBubble = true;
                targetBubbleAttr.afterBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubble.transform;
                this.moveOnSplineAttr.distanceCalc = targetMoveOnSplineAttr.distanceCalc;

            }
            else
            {
                targetBubbleAttr.afterBubble.GetComponent<Bubble>().beforeBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubble.transform;
                this.bubbleAttr.afterBubble = targetBubbleAttr.afterBubble;
                targetBubbleAttr.afterBubble = transform;

                if (targetBubble.GetComponent<MoveOnSpline>().explosionCounter != 0)
                {
                    this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.beforeBubble.GetComponent<MoveOnSpline>().distanceCalc;
                    this.moveOnSplineAttr.explosionCounter = targetBubble.GetComponent<MoveOnSpline>().explosionCounter;
                }
                else
                {
                    this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttr.bubbleSizeAverage;
                }

            }
        }
    }



    private void setHirarchyIndex(Bubble targetBubbleAttr, int newHirarchyIndex)
    {
        transform.SetParent(this.bubbles);

        transform.SetSiblingIndex(newHirarchyIndex);
        for (int i = transform.GetSiblingIndex(); i < this.bubbles.childCount; i++)
        {
            this.bubbles.GetChild(i).GetComponent<Bubble>().checkBubblerowInfront();
        }
    }
    
    private void addInsertedBubbleAttrToRow(Bubble targetBubbleAttr, int rowinfrontlength)
    {
        for (int i = 0; i < rowinfrontlength; i++)
        {

            if (this.targetBubble.GetComponent<MoveOnSpline>().explosionCounter == 0)
            {
                if (targetBubbleAttr.movedBubbleRow[i].GetComponent<MoveOnSpline>().explosionCounter == 0)
                {
                    targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().interpolate = true;
                    this.animateBubbleCount++;
                }

            }
            else
            {
                if (targetBubble.GetComponent<MoveOnSpline>().explosionCounter == targetBubbleAttr.movedBubbleRow[i].GetComponent<MoveOnSpline>().explosionCounter)
                {
                    targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().interpolate = true;
                    this.animateBubbleCount++;
                }
            }
        }

    }
}
