using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootedBubble : MonoBehaviour
{


    public bool hitted = false;
    public GameObject targetbubble;
    public Transform bubbles;
    private Bubble bubbleattr;
    private GameMaster gamemaster;

    private bool insertbefore = false;
    private bool insertafter = false;



    void Start()
    {
        this.bubbleattr = gameObject.GetComponent<Bubble>();
        this.bubbles = this.bubbleattr.bubbles;
        this.gamemaster = this.bubbleattr.gamemasterattributes;


    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts[0].otherCollider.gameObject.tag == "Bubble" && !this.hitted)
        {

            this.hitted = true;
            targetbubble = collision.contacts[0].otherCollider.gameObject;
            Bubble targetbubbleattr = targetbubble.GetComponent<Bubble>();
            handleInsertBubble(targetbubbleattr);


        }

    }

    private void handleInsertBubble(Bubble targetbubbleattr)
    {
        Vector3 bubbleposbefore = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance + this.gamemaster.bubblesizeaverage);
        Vector3 bubbleposafter = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance - this.gamemaster.bubblesizeaverage);

        //Debug.DrawRay(bubbleposbefore, Vector3.up, Color.green, 2);
        //Debug.DrawRay(bubbleposafter, Vector3.up, Color.blue, 2);

        float distancebubbleposbefore = Vector3.Distance(bubbleposbefore, transform.position);
        float distancebubbleposafter = Vector3.Distance(bubbleposafter, transform.position);

        if (distancebubbleposafter > distancebubbleposbefore)
        {
            Debug.Log("before");
            this.insertbefore = true;

        }
        else if (distancebubbleposafter < distancebubbleposbefore)
        {
            Debug.Log("after");
            this.insertafter = true;

        }

        insertedBubbleHandler(targetbubbleattr);
        checkNeighboursOfBubble(targetbubbleattr);

    }

    private void checkNeighboursOfBubble(Bubble targetbubbleattr)
    {
        
    }

    private void insertedBubbleHandler(Bubble targetbubbleattr)
    {
        if (this.insertbefore)
        {
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedbubblerow.Length - 1);
            setHirarchyIndex(targetbubbleattr, this.targetbubble.transform.GetSiblingIndex());
            setNewValuesForBubbles(targetbubbleattr);
            gameObject.AddComponent<MoveOnSpline>();
        }
        else if (this.insertafter)
        {
            addInsertedBubbleAttrToRow(targetbubbleattr, targetbubbleattr.movedbubblerow.Length);
            setHirarchyIndex(targetbubbleattr, this.targetbubble.transform.GetSiblingIndex() + 1);
            setNewValuesForBubbles(targetbubbleattr);
            gameObject.AddComponent<MoveOnSpline>();
        }
    }


    private void setNewValuesForBubbles(Bubble targetbubbleattr)
    {
        if (this.insertbefore)
        {
            if (targetbubbleattr.isfirstbubble)
            {
                targetbubbleattr.isfirstbubble = false;
                this.bubbleattr.isfirstbubble = true;
                targetbubbleattr.beforebubble = transform;
                this.bubbleattr.afterbubble = targetbubbleattr.transform;
                this.bubbleattr.bubblesinserted = targetbubbleattr.bubblesinserted + 1;
                this.bubbleattr.cursor.Distance = targetbubbleattr.distance;
            }
            else
            {
                targetbubbleattr.beforebubble.GetComponent<Bubble>().afterbubble = transform;
                this.bubbleattr.beforebubble = targetbubbleattr.beforebubble;
                this.bubbleattr.afterbubble = this.targetbubble.transform;
                targetbubbleattr.beforebubble = transform;
                this.bubbleattr.bubblesinserted = this.bubbleattr.beforebubble.GetComponent<Bubble>().bubblesinserted;
            }

        }
        else if (this.insertafter)
        {
            if (targetbubbleattr.islastbubble)
            {
                targetbubbleattr.islastbubble = false;
                this.bubbleattr.islastbubble = true;
                targetbubbleattr.afterbubble = transform;
                this.bubbleattr.beforebubble = targetbubble.transform;
                this.bubbleattr.bubblesinserted = targetbubbleattr.bubblesinserted;
            }
            else
            {
                targetbubbleattr.afterbubble.GetComponent<Bubble>().beforebubble = transform;
                this.bubbleattr.beforebubble = targetbubble.transform;
                this.bubbleattr.afterbubble = targetbubbleattr.afterbubble;
                targetbubbleattr.afterbubble = transform;
                this.bubbleattr.bubblesinserted = targetbubbleattr.bubblesinserted;
            }
        }
    }

    private void setHirarchyIndex(Bubble targetbubbleattr, int newhirarchyindex)
    {
        transform.SetParent(this.bubbles);

        transform.SetSiblingIndex(newhirarchyindex);
        for (int i = transform.GetSiblingIndex(); i < this.bubbles.childCount; i++)
        {
            this.bubbles.GetChild(i).GetComponent<Bubble>().checkBubblerowInfront();
        }
    }

    private void addInsertedBubbleAttrToRow(Bubble targetbubbleattr, int rowinfrontlength)
    {
        for (int i = 0; i < rowinfrontlength; i++)
        {
            targetbubbleattr.movedbubblerow[i].GetComponent<Bubble>().bubblesinserted++;
        }
    }


}
