using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionProvider : MonoBehaviour {

    public GameObject targetBubble;
    public Transform bubbles;
    private Bubble bubbleAttr;
    private GameObject gameMaster;
    private GameMaster gameMasterAttr;
    private MoveOnSpline moveOnSplineAttr;
    private Wavespawner waveSpawner;

   
    private int explosionBubblesCount = 0;
    private Transform bubbleTransform;


    
    

    public ExplosionProvider(Bubble bubble)
    {
        this.bubbleAttr = bubble;
        this.gameMaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttr = this.gameMaster.GetComponent<GameMaster>();
        this.bubbles = this.bubbleAttr.bubbles;
        this.waveSpawner = this.bubbleAttr.waveSpawner;
        this.bubbleTransform = this.bubbleAttr.transform;
        
    }



    public void handleExplosion(int soundDecision)
    {

        int leftColorBorderIndex = -1;
        int rightColorBorderIndex = -1;

        if (!this.bubbleAttr.isFirstBubble && !this.bubbleAttr.isLastBubble)
        {
            leftColorBorderIndex =  checkNeighboursOfBubble("left", this.bubbleAttr.bubbleColor, this.bubbleTransform.GetSiblingIndex());
            rightColorBorderIndex = checkNeighboursOfBubble("right", this.bubbleAttr.bubbleColor, this.bubbleTransform.GetSiblingIndex());
        }
        else if (this.bubbleAttr.isFirstBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left", this.bubbleAttr.bubbleColor, this.bubbleTransform.GetSiblingIndex());
        }
        else if (this.bubbleAttr.isLastBubble)
        {
            rightColorBorderIndex = checkNeighboursOfBubble("right", this.bubbleAttr.bubbleColor, this.bubbleTransform.GetSiblingIndex());
        }

        if (leftColorBorderIndex == -1) { leftColorBorderIndex = this.bubbleTransform.GetSiblingIndex(); };
        if (rightColorBorderIndex == -1) { rightColorBorderIndex = this.bubbleTransform.GetSiblingIndex(); };

        switch (checkIfExplode(leftColorBorderIndex, rightColorBorderIndex))
        {
            case 1:

                if(soundDecision == 1){
                    this.gameMasterAttr.audioManager.handleSound("ExplodeBubble", 1);
                    this.gameMasterAttr.audioManager.handleSound("BubblesDestroyed", 1);
                }else if (soundDecision == 2)
                {
                    this.gameMasterAttr.audioManager.handleSound("ExplodeBubble", 3);
                    this.gameMasterAttr.audioManager.handleSound("BubblesDestroyed2", 1);
                }
                

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

            if (this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>().bubbleColor == this.bubbles.GetChild(leftColorBorderIndex + 1).GetComponent<Bubble>().bubbleColor)
            {
                int searchedBubbleColor = this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>().bubbleColor;
                int leftColorBubbleIndex = checkNeighboursOfBubble("left", searchedBubbleColor, leftColorBorderIndex + 1);
                int rightColorBubbleIndex = checkNeighboursOfBubble("right", searchedBubbleColor, rightColorBorderIndex - 1);


                setRollbackAttribute(this.bubbles.GetChild(rightColorBorderIndex - 1).GetComponent<Bubble>());

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
