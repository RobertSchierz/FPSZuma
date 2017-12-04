using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootedBubble : MonoBehaviour
{


    public bool hitted = false;
    public GameObject targetBubble;
    public Transform bubbles;
    private Bubble bubbleAttr;
    private GameObject gameMaster;
    private GameMaster gameMasterAttr;
    private MoveOnSpline moveOnSplineAttr;
    private Wavespawner waveSpawner;

    private bool insertBefore = false;
    private bool insertAfter = false;

    public int animateBubbleCount;



    void Start()
    {
        this.bubbleAttr = gameObject.GetComponent<Bubble>();
        this.gameMaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttr = this.gameMaster.GetComponent<GameMaster>();
        this.bubbles = this.bubbleAttr.bubbles;
        this.waveSpawner = this.bubbleAttr.waveSpawner;



    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts[0].otherCollider.gameObject.tag == "Bubble" && !this.hitted)
        {
            MoveOnSpline.OnInsertAnimationUpdate += checkInsertionAnimationUpdate;
            this.hitted = true;
            //this.gameMaster.stopAll = true;
            //this.waveSpawner.rollInRow = true;
            targetBubble = collision.contacts[0].otherCollider.gameObject;
            Bubble targetBubbleAttr = targetBubble.GetComponent<Bubble>();
            MoveOnSpline targetMoveOnSplineAttr = targetBubble.GetComponent<MoveOnSpline>();
            handleInsertBubble(targetBubbleAttr, targetMoveOnSplineAttr);

        }

    }

    void checkInsertionAnimationUpdate()
    {
        this.animateBubbleCount--;
        if (this.animateBubbleCount == 0)
        {
            handleExplosion();
            MoveOnSpline.OnInsertAnimationUpdate -= checkInsertionAnimationUpdate;
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

                if (targetBubble.GetComponent<MoveOnSpline>().waitAfterExplosion)
                {
                    this.moveOnSplineAttr.waitAfterExplosion = true;
                }

                handleExplosion();
            }
            else
            {
                targetBubbleAttr.beforeBubble.GetComponent<Bubble>().afterBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubbleAttr.beforeBubble;
                this.bubbleAttr.afterBubble = this.targetBubble.transform;
                targetBubbleAttr.beforeBubble = transform;
                this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttr.bubbleSizeAverage;
                if (targetBubble.GetComponent<MoveOnSpline>().waitAfterExplosion)
                {
                    this.moveOnSplineAttr.waitAfterExplosion = true;
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

                if (targetBubble.GetComponent<MoveOnSpline>().waitAfterExplosion)
                {
                    this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.beforeBubble.GetComponent<MoveOnSpline>().distanceCalc;
                    this.moveOnSplineAttr.waitAfterExplosion = true;
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

            if (!this.targetBubble.GetComponent<MoveOnSpline>().waitAfterExplosion)
            {
                if (!targetBubbleAttr.movedBubbleRow[i].GetComponent<MoveOnSpline>().waitAfterExplosion)
                {
                    targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().interpolate = true;
                    this.animateBubbleCount++;
                }

            }
            else
            {
                
                    targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().interpolate = true;
                    this.animateBubbleCount++;
                 
            }

            //targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().bubblesInserted++;


        }

    }

    /*private bool checkMultipleExplosion(int bubbleIndex, Bubble targetBubbleAttr)
    {
        if (targetBubbleAttr.movedBubbleRow[bubbleIndex].GetComponent<Bubble>().isFirstBubble)
        {
            return false;
        }else
        {
            if (targetBubbleAttr.movedBubbleRow[bubbleIndex + 1].GetComponent<MoveOnSpline>().distanceCalc > targetBubbleAttr.movedBubbleRow[bubbleIndex].GetComponent<MoveOnSpline>().distanceCalc + this.gameMasterAttr.bubbleSizeAverage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }*/

    //----------------------------------------Explosion---------------------------------//

    private void handleExplosion()
    {
        int leftColorBorderIndex = -1;
        int rightColorBorderIndex = -1;

        if (!this.bubbleAttr.isFirstBubble && !this.bubbleAttr.isLastBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left");
            rightColorBorderIndex = checkNeighboursOfBubble("right");
        }
        else if (this.bubbleAttr.isFirstBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left");
        }
        else if (this.bubbleAttr.isLastBubble)
        {
            rightColorBorderIndex = checkNeighboursOfBubble("right");
        }

        if (leftColorBorderIndex == -1) { leftColorBorderIndex = transform.GetSiblingIndex(); };
        if (rightColorBorderIndex == -1) { rightColorBorderIndex = transform.GetSiblingIndex(); };

        if (checkIfExplode(leftColorBorderIndex, rightColorBorderIndex))
        {
            explodeBubbles(leftColorBorderIndex, rightColorBorderIndex);
        }

    }

    private void setNewValuesForBubbleExplosion(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        if (rightColorBorderIndex == 0)
        {
            this.bubbles.GetChild(leftColorBorderIndex + 1).GetComponent<Bubble>().isFirstBubble = true;
        }
        else if (leftColorBorderIndex == this.bubbles.childCount - 1)
        {
            this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>().isLastBubble = true;
        }
        else
        {
            Bubble leftLeftOverBubble = this.bubbles.GetChild(leftColorBorderIndex + 1).GetComponent<Bubble>();
            Bubble rightLeftOverBubble = this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>();
            leftLeftOverBubble.beforeBubble = rightLeftOverBubble.transform;
            rightLeftOverBubble.afterBubble = leftLeftOverBubble.transform;

        }

    }

    private void setMovedBubbleRow(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        int bubblesExplodedCount = Math.Abs(leftColorBorderIndex - rightColorBorderIndex) + 1;
        for (int i = leftColorBorderIndex; i < this.bubbles.childCount; i++)
        {
            Transform[] infrontRowOfBubble = this.bubbles.GetChild(i).GetComponent<Bubble>().movedBubbleRow;
            List<Transform> tmpInfrontRowOfBubbleList = infrontRowOfBubble.ToList();
            try
            {
                tmpInfrontRowOfBubbleList.RemoveRange(rightColorBorderIndex, bubblesExplodedCount);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

            this.bubbles.GetChild(i).GetComponent<Bubble>().movedBubbleRow = new Transform[tmpInfrontRowOfBubbleList.Count];
            this.bubbles.GetChild(i).GetComponent<Bubble>().movedBubbleRow = tmpInfrontRowOfBubbleList.ToArray<Transform>();
        }
    }

    private void setExplosionWait(Transform[] rowInfront)
    {
        for (int i = 0; i < rowInfront.Length; i++)
        {
            rowInfront[i].GetComponent<MoveOnSpline>().waitAfterExplosion = true;
        }
        //Debug.Break();

    }



    private void explodeBubbles(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        Debug.Log("Exploded");
        setNewValuesForBubbleExplosion(leftColorBorderIndex, rightColorBorderIndex);
        for (int i = rightColorBorderIndex; i <= leftColorBorderIndex; i++)
        {
            Destroy(this.bubbles.GetChild(i).gameObject);
        }
        setMovedBubbleRow(leftColorBorderIndex, rightColorBorderIndex);

        if (!(rightColorBorderIndex == 0) && !(leftColorBorderIndex == this.bubbles.childCount - 1))
        {
            setExplosionWait(this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>().movedBubbleRow);
        }



        //Helper->
        for (int i = leftColorBorderIndex + 1; i < this.bubbles.childCount; i++)
        {
            this.bubbles.GetChild(i).GetComponent<MoveOnSpline>().helperWait = true;
        }

    }

    private bool checkIfExplode(int leftColorBorderIndex, int rightColorBorderIndex)
    {


        if (Mathf.Abs(leftColorBorderIndex - rightColorBorderIndex) >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int checkNeighboursOfBubble(string decision)
    {

        int leftColorBorderIndex = -1;
        int rightColorBorderIndex = -1;

        if (decision.Equals("left"))
        {
            for (int i = transform.GetSiblingIndex() + 1; i < this.bubbles.childCount; i++)
            {
                if (this.bubbles.GetChild(i).GetComponent<Bubble>().bubbleColor == this.bubbleAttr.bubbleColor)
                {
                    leftColorBorderIndex = i;
                }
                else
                {
                    break;
                }
            }
            return leftColorBorderIndex;
        }
        else if (decision.Equals("right"))
        {
            for (int i = transform.GetSiblingIndex() - 1; i >= 0; i--)
            {
                if (this.bubbles.GetChild(i).GetComponent<Bubble>().bubbleColor == this.bubbleAttr.bubbleColor)
                {
                    rightColorBorderIndex = i;
                }
                else
                {
                    break;
                }
            }
            return rightColorBorderIndex;
        }
        else
        {
            return -1;
        }

    }


}
