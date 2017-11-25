using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootedBubble : MonoBehaviour
{


    public bool hitted = false;
    public GameObject targetBubble;
    public Transform bubbles;
    private Bubble bubbleAttr;
    private GameMaster gameMaster;
    private MoveOnSpline moveOnSplineAttr;

    private bool insertBefore = false;
    private bool insertAfter = false;



    void Start()
    {
        this.bubbleAttr = gameObject.GetComponent<Bubble>();
        this.bubbles = this.bubbleAttr.bubbles;
        this.gameMaster = this.bubbleAttr.gameMasterAttributes;


    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts[0].otherCollider.gameObject.tag == "Bubble" && !this.hitted)
        {

            this.hitted = true;
            targetBubble = collision.contacts[0].otherCollider.gameObject;
            Bubble targetBubbleAttr = targetBubble.GetComponent<Bubble>();
            MoveOnSpline targetMoveOnSplineAttr = targetBubble.GetComponent<MoveOnSpline>();
            handleInsertBubble(targetBubbleAttr, targetMoveOnSplineAttr);
            //this.gameMaster.stopAll = true;


        }

    }

    private void handleInsertBubble(Bubble targetbubbleattr, MoveOnSpline targetMoveOnSplineAttr)
    {
        Vector3 bubblePosBefore = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.cursor.Distance + this.gameMaster.bubbleSizeAverage);
        Vector3 bubblePosAfter = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.cursor.Distance - this.gameMaster.bubbleSizeAverage);

        float distanceBubblePosBefore = Vector3.Distance(bubblePosBefore, transform.position);
        float distanceBubblePosAfter = Vector3.Distance(bubblePosAfter, transform.position);

        if (distanceBubblePosAfter > distanceBubblePosBefore)
        {
            Debug.Log("before");
            this.insertBefore = true;

        }
        else if (distanceBubblePosAfter < distanceBubblePosBefore)
        {
            Debug.Log("after");
            this.insertAfter = true;

        }

        insertedBubbleHandler(targetbubbleattr, targetMoveOnSplineAttr);
        handleExplosion();


    }

    private void handleExplosion()
    {
        int leftColorBorderIndex = -1;
        int rightColorBorderIndex = -1;

        if (!this.bubbleAttr.isFirstBubble && !this.bubbleAttr.isLastBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left");
            rightColorBorderIndex = checkNeighboursOfBubble("right");
        }else if (this.bubbleAttr.isFirstBubble)
        {
            leftColorBorderIndex = checkNeighboursOfBubble("left");
        }
        else if (this.bubbleAttr.isLastBubble)
        {
            rightColorBorderIndex = checkNeighboursOfBubble("right");
        }

        if (checkIfExplode(leftColorBorderIndex, rightColorBorderIndex))
        {
            Debug.Log("Explode");
            explodeBubbles(leftColorBorderIndex, rightColorBorderIndex);
        }

    }

    private void explodeBubbles(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        for (int i = rightColorBorderIndex; i <= leftColorBorderIndex; i++)
        {
           // Destroy(this.bubbles.GetChild(i).gameObject);
        }
    }

    private bool checkIfExplode(int leftColorBorderIndex, int rightColorBorderIndex)
    {
        if (leftColorBorderIndex == -1) { leftColorBorderIndex = transform.GetSiblingIndex(); };
        if (rightColorBorderIndex == -1) { rightColorBorderIndex = transform.GetSiblingIndex(); };

        if (Mathf.Abs(leftColorBorderIndex - rightColorBorderIndex) >= 2)
        {
            return true;
        }else
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


    private void insertedBubbleHandler(Bubble targetbubbleattr, MoveOnSpline targetMoveOnSplineAttr)
    {
        if (this.insertBefore)
        {
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedBubbleRow.Length - 1);
            setHirarchyIndex(targetbubbleattr, this.targetBubble.transform.GetSiblingIndex());
            gameObject.AddComponent<MoveOnSpline>();
            this.moveOnSplineAttr = gameObject.GetComponent<MoveOnSpline>();
            setNewValuesForBubbles(targetbubbleattr, targetMoveOnSplineAttr);
            
        }
        else if (this.insertAfter)
        {
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedBubbleRow.Length);
            setHirarchyIndex(targetbubbleattr, this.targetBubble.transform.GetSiblingIndex() + 1);
            gameObject.AddComponent<MoveOnSpline>();
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
                this.bubbleAttr.bubblesInserted = targetBubbleAttr.bubblesInserted + 1;
                
                this.moveOnSplineAttr.distanceCalc = targetMoveOnSplineAttr.distanceCalc;
            }
            else
            {
                targetBubbleAttr.beforeBubble.GetComponent<Bubble>().afterBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubbleAttr.beforeBubble;
                this.bubbleAttr.afterBubble = this.targetBubble.transform;
                targetBubbleAttr.beforeBubble = transform;
                this.bubbleAttr.bubblesInserted = this.bubbleAttr.afterBubble.GetComponent<Bubble>().bubblesInserted + 1;
                this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc;
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
                this.bubbleAttr.bubblesInserted = targetBubbleAttr.bubblesInserted;
                this.moveOnSplineAttr.distanceCalc = targetMoveOnSplineAttr.distanceCalc - this.gameMaster.bubbleSizeAverage;
                
            }
            else
            {
                targetBubbleAttr.afterBubble.GetComponent<Bubble>().beforeBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubble.transform;
                this.bubbleAttr.afterBubble = targetBubbleAttr.afterBubble;
                targetBubbleAttr.afterBubble = transform;
              
                this.bubbleAttr.bubblesInserted = this.bubbleAttr.afterBubble.GetComponent<Bubble>().bubblesInserted + 1;
                
                this.moveOnSplineAttr.distanceCalc = this.bubbleAttr.afterBubble.GetComponent<MoveOnSpline>().distanceCalc;
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
            targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().bubblesInserted++;
            targetBubbleAttr.movedBubbleRow[i].GetComponent<Bubble>().interpolate = true;
            
        }
    }


}
