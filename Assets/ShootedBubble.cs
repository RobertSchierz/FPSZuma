using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootedBubble : MonoBehaviour
{


    public bool hitted = false;
    public GameObject targetbubble;
    public Transform bubbles;
    private Bubble bubbleattr;



    void Start()
    {
        this.bubbleattr = gameObject.GetComponent<Bubble>();
        this.bubbles = this.bubbleattr.bubbles;


    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.contacts[0].otherCollider.gameObject.tag == "Bubble" && !this.hitted)
        {

            this.hitted = true;


            targetbubble = collision.contacts[0].otherCollider.gameObject;
            Bubble bubbleattr = targetbubble.GetComponent<Bubble>();


            float distancetoafterbubble = Vector3.Distance(bubbleattr.afterbubble.transform.position, transform.position);
            float distancetobeforebubble = Vector3.Distance(bubbleattr.beforebubble.transform.position, transform.position);

            if (distancetoafterbubble > distancetobeforebubble)
            {
                Debug.Log("before");
                insertBubbleInRow(bubbleattr, 1);
            }
            else if (distancetoafterbubble < distancetobeforebubble)
            {
                Debug.Log("after");
                insertBubbleInRow(bubbleattr, 2);
            }

        }

    }

    private void insertBubbleInRow(Bubble targetbubbleattr, int rowdecision)
    {
        if (rowdecision == 1)
        {

            for (int i = 0; i < targetbubbleattr.movedbubblerow.Length-1; i++)
            {
                targetbubbleattr.movedbubblerow[i].GetComponent<Bubble>().bubblesinserted++;
            }

        }
        else if (rowdecision == 2)
        {

            for (int i = 0; i < targetbubbleattr.movedbubblerow.Length; i++)
            {
                targetbubbleattr.movedbubblerow[i].GetComponent<Bubble>().bubblesinserted++;
            }
        }
        setHirarchyIndex(targetbubbleattr, rowdecision);
        setStates(targetbubbleattr, rowdecision);
        gameObject.AddComponent<MoveOnSpline>();

    }

    private void setStates(Bubble targetbubbleattr, int rowdecision)
    {



        if (rowdecision == 1)
        {
            this.bubbleattr.beforebubble = targetbubbleattr.beforebubble;
            this.bubbleattr.afterbubble = this.targetbubble.transform;
            targetbubbleattr.beforebubble = transform;
            this.bubbleattr.bubblesinserted = this.bubbleattr.beforebubble.GetComponent<Bubble>().bubblesinserted;
        }
        else if (rowdecision == 2)
        {
            this.bubbleattr.beforebubble = targetbubble.transform;
            this.bubbleattr.afterbubble = targetbubbleattr.afterbubble;
            targetbubbleattr.afterbubble = transform;
            this.bubbleattr.bubblesinserted = targetbubbleattr.bubblesinserted;
        }
    }

    private void setHirarchyIndex(Bubble targetbubbleattr, int rowdecision)
    {
        transform.SetParent(this.bubbles);

        if (rowdecision == 1)
        {
            transform.SetSiblingIndex(this.targetbubble.transform.GetSiblingIndex());
            for (int i = transform.GetSiblingIndex(); i < this.bubbles.childCount; i++)
            {
                this.bubbles.GetChild(i).GetComponent<Bubble>().checkBubblerowInfront();
            }
        }
        else if (rowdecision == 2)
        {
            transform.SetSiblingIndex(this.targetbubble.transform.GetSiblingIndex() + 1);
            for (int i = transform.GetSiblingIndex(); i < this.bubbles.childCount; i++)
            {
                this.bubbles.GetChild(i).GetComponent<Bubble>().checkBubblerowInfront();
            }

        }



    }


}
