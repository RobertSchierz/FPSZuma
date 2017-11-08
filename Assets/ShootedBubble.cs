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
            handleInsertBubble(targetBubbleAttr);


        }

    }

    private void handleInsertBubble(Bubble targetbubbleattr)
    {
        Vector3 bubblePosBefore = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance + this.gameMaster.bubbleSizeAverage);
        Vector3 bubblePosAfter = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance - this.gameMaster.bubbleSizeAverage);

        //Debug.DrawRay(bubbleposbefore, Vector3.up, Color.green, 2);
        //Debug.DrawRay(bubbleposafter, Vector3.up, Color.blue, 2);

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

        insertedBubbleHandler(targetbubbleattr);
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


    private void insertedBubbleHandler(Bubble targetbubbleattr)
    {
        if (this.insertBefore)
        {
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedBubbleRow.Length - 1);
            setHirarchyIndex(targetbubbleattr, this.targetBubble.transform.GetSiblingIndex());
            setNewValuesForBubbles(targetbubbleattr);
            gameObject.AddComponent<MoveOnSpline>();
        }
        else if (this.insertAfter)
        {
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedBubbleRow.Length);
            setHirarchyIndex(targetbubbleattr, this.targetBubble.transform.GetSiblingIndex() + 1);
            setNewValuesForBubbles(targetbubbleattr);
            gameObject.AddComponent<MoveOnSpline>();
        }
    }


    private void setNewValuesForBubbles(Bubble targetBubbleAttr)
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
                this.bubbleAttr.cursor.Distance = targetBubbleAttr.distance;
            }
            else
            {
                targetBubbleAttr.beforeBubble.GetComponent<Bubble>().afterBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubbleAttr.beforeBubble;
                this.bubbleAttr.afterBubble = this.targetBubble.transform;
                targetBubbleAttr.beforeBubble = transform;
                this.bubbleAttr.bubblesInserted = this.bubbleAttr.beforeBubble.GetComponent<Bubble>().bubblesInserted;
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
            }
            else
            {
                targetBubbleAttr.afterBubble.GetComponent<Bubble>().beforeBubble = transform;
                this.bubbleAttr.beforeBubble = targetBubble.transform;
                this.bubbleAttr.afterBubble = targetBubbleAttr.afterBubble;
                targetBubbleAttr.afterBubble = transform;
                this.bubbleAttr.bubblesInserted = targetBubbleAttr.bubblesInserted;
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
        }
    }


}
