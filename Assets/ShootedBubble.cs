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
    private int explosionBubblesCount = 0;
    public bool isInRow = false;
    public float distanceToInsertionspoint = 0f;
    //public HelperMethods helper;



    void Start()
    {
        this.bubbleAttr = gameObject.GetComponent<Bubble>();
        this.gameMaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttr = this.gameMaster.GetComponent<GameMaster>();
        this.bubbles = this.bubbleAttr.bubbles;
        this.waveSpawner = this.bubbleAttr.waveSpawner;
        //this.helper = new HelperMethods(this.bubbleAttr);


    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.contacts[0].otherCollider.gameObject.tag == "Bubble" && !this.hitted)
        {
            transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
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
            //this.helper.handleExplosion();
            MoveOnSpline.OnInsertAnimationUpdate -= checkInsertionAnimationUpdate;
            handleExplosion();
            
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

                handleExplosion();
                //this.helper.handleExplosion();
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
                    handleExplosion();
                    //this.helper.handleExplosion();
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


    //----------------------------------------Explosion---------------------------------//

        

    private void handleExplosion()
    {

        int leftColorBorderIndex = -1;
        int rightColorBorderIndex = -1;

        if (!this.bubbleAttr.isFirstBubble && !this.bubbleAttr.isLastBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left", this.bubbleAttr.bubbleColor, transform.GetSiblingIndex());
            rightColorBorderIndex = checkNeighboursOfBubble("right", this.bubbleAttr.bubbleColor, transform.GetSiblingIndex());
        }
        else if (this.bubbleAttr.isFirstBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left", this.bubbleAttr.bubbleColor, transform.GetSiblingIndex());
        }
        else if (this.bubbleAttr.isLastBubble)
        {
            rightColorBorderIndex = checkNeighboursOfBubble("right", this.bubbleAttr.bubbleColor, transform.GetSiblingIndex());
        }

        if (leftColorBorderIndex == -1) { leftColorBorderIndex = transform.GetSiblingIndex(); };
        if (rightColorBorderIndex == -1) { rightColorBorderIndex = transform.GetSiblingIndex(); };

        switch (checkIfExplode(leftColorBorderIndex, rightColorBorderIndex))
        {
            case 1:
                explodeBubbles(leftColorBorderIndex, rightColorBorderIndex);
                break;
            case 2:
                Debug.Log("rutsch");
                break;
            case 3:
                Debug.Log("Rutsch and explode");
                break;
            default:
                break;
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

    private void setExplosionWait(Transform[] rowInfront, int leftIndex, int rightIndex)
    {

        if ((this.bubbles.GetChild(leftIndex + 1).GetComponent<MoveOnSpline>().explosionCounter == this.bubbles.GetChild(leftIndex).GetComponent<MoveOnSpline>().explosionCounter)
            &&
            (this.bubbles.GetChild(rightIndex - 1).GetComponent<MoveOnSpline>().explosionCounter == this.bubbles.GetChild(rightIndex).GetComponent<MoveOnSpline>().explosionCounter))
        {
            for (int i = 0; i < rowInfront.Length; i++)
            {
                rowInfront[i].GetComponent<MoveOnSpline>().explosionCounter++;
            }
        }

        if ((this.bubbles.GetChild(leftIndex + 1).GetComponent<MoveOnSpline>().explosionCounter != this.bubbles.GetChild(leftIndex).GetComponent<MoveOnSpline>().explosionCounter)
          &&
          (this.bubbles.GetChild(rightIndex - 1).GetComponent<MoveOnSpline>().explosionCounter != this.bubbles.GetChild(rightIndex).GetComponent<MoveOnSpline>().explosionCounter))
        {
            for (int i = 0; i < rowInfront.Length; i++)
            {
                rowInfront[i].GetComponent<MoveOnSpline>().explosionCounter--;
            }
        }

    }



    private void explodeBubbles(int leftColorBorderIndex, int rightColorBorderIndex)
    {

        setNewValuesForBubbleExplosion(leftColorBorderIndex, rightColorBorderIndex);
        for (int i = rightColorBorderIndex; i <= leftColorBorderIndex; i++)
        {
            Destroy(this.bubbles.GetChild(i).gameObject);
            this.explosionBubblesCount++;
        }
        setMovedBubbleRow(leftColorBorderIndex, rightColorBorderIndex);

        if (!(rightColorBorderIndex == 0) && !(leftColorBorderIndex == this.bubbles.childCount - 1))
        {
            setExplosionWait(this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>().movedBubbleRow, leftColorBorderIndex, rightColorBorderIndex);
        }

        checkExplosionSlide(leftColorBorderIndex, rightColorBorderIndex);



        //Helper->
        for (int i = leftColorBorderIndex + 1; i < this.bubbles.childCount; i++)
        {
            if (this.bubbles.GetChild(i).GetComponent<MoveOnSpline>().explosionCounter == 0)
            {
                this.bubbles.GetChild(i).GetComponent<MoveOnSpline>().helperWait = true;
            }

        }

    }

    private void checkExplosionSlide(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        if (rightColorBorderIndex != 0 && leftColorBorderIndex != this.bubbles.childCount)
        {

            if (this.bubbles.GetChild(rightColorBorderIndex -1).GetComponent<Bubble>().bubbleColor == this.bubbles.GetChild(leftColorBorderIndex + 1).GetComponent<Bubble>().bubbleColor)
            {
                int searchedBubbleColor = this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>().bubbleColor;
                int leftColorBubbleIndex = checkNeighboursOfBubble("left", searchedBubbleColor, leftColorBorderIndex + 1);
                int rightColorBubbleIndex = checkNeighboursOfBubble("right", searchedBubbleColor, rightColorBorderIndex - 1);

              
                setRollbackAttribute(this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>());
                

                //if (leftColorBubbleIndex == -1) { leftColorBubbleIndex = leftColorBorderIndex + 1; }
                //if (rightColorBubbleIndex == -1) { rightColorBubbleIndex = rightColorBorderIndex - 1; }

                //Debug.Log(leftColorBubbleIndex);
                //Debug.Log(rightColorBubbleIndex);
                //Debug.Log(this.explosionBubblesCount);
                //Debug.Break();
            }

        }
    }

    private void setRollbackAttribute(Bubble rollbackBubble)
    {

        foreach (var infrontBubble in rollbackBubble.movedBubbleRow)
        {
            if (infrontBubble.GetComponent("ShootedBubble"))
            {
                infrontBubble.GetComponent<ShootedBubble>().enabled = false;
            }
            if (infrontBubble.GetComponent<MoveOnSpline>().explosionCounter == rollbackBubble.gameObject.GetComponent<MoveOnSpline>().explosionCounter)
            {
                infrontBubble.GetComponent<Bubble>().rollback = true;
                infrontBubble.GetComponent<Bubble>().rollbackBorderBubble = rollbackBubble.transform;
            }
        }
    }

    private int checkIfExplode(int leftColorBorderIndex, int rightColorBorderIndex)
    {



        if (Mathf.Abs(leftColorBorderIndex - rightColorBorderIndex) >= 2)
        {

            if (isExplosionColorInDifferentRow(leftColorBorderIndex, rightColorBorderIndex))
            {
                return 3;
            }
            else
            {
                return 1;
            }


        }
        else if (Mathf.Abs(leftColorBorderIndex - rightColorBorderIndex) >= 0 && Mathf.Abs(leftColorBorderIndex - rightColorBorderIndex) < 2)
        {
            if (isExplosionColorInDifferentRow(leftColorBorderIndex, rightColorBorderIndex))
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    private bool isExplosionColorInDifferentRow(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        for (int i = rightColorBorderIndex; i < leftColorBorderIndex; i++)
        {
            if (this.bubbles.GetChild(i).GetComponent<MoveOnSpline>().explosionCounter != this.bubbles.GetChild(i + 1).GetComponent<MoveOnSpline>().explosionCounter)
            {
                return true;
            }
        }
        return false;
    }

    private int checkNeighboursOfBubble(string decision, int bubbleColor, int bubbleIndex)
    {

        int leftColorBorderIndex = -1;
        int rightColorBorderIndex = -1;

        if (decision.Equals("left"))
        {
            for (int i = bubbleIndex + 1; i < this.bubbles.childCount; i++)
            {
                if (this.bubbles.GetChild(i).GetComponent<Bubble>().bubbleColor == bubbleColor)
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
            for (int i = bubbleIndex - 1; i >= 0; i--)
            {
                if (this.bubbles.GetChild(i).GetComponent<Bubble>().bubbleColor == bubbleColor)
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
