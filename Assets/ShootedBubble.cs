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
            Bubble targetbubbleattr = targetbubble.GetComponent<Bubble>();

            if (!targetbubbleattr.isfirstbubble && !targetbubbleattr.islastbubble)
            {
                float distancetoafterbubble = Vector3.Distance(targetbubbleattr.afterbubble.transform.position, transform.position);
                float distancetobeforebubble = Vector3.Distance(targetbubbleattr.beforebubble.transform.position, transform.position);

                if (distancetoafterbubble > distancetobeforebubble)
                {
                    Debug.Log("before");
                    insertBubbleInRow(targetbubbleattr, 1);
                }
                else if (distancetoafterbubble < distancetobeforebubble)
                {
                    Debug.Log("after");
                    insertBubbleInRow(targetbubbleattr, 2);
                }
            } else if (targetbubbleattr.isfirstbubble && !targetbubbleattr.islastbubble)
            {
                Debug.Log("firstbubble hit");


                Vector3 temppossbefore = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance + targetbubbleattr.transform.localScale.x);
                Vector3 temppossafter = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance - targetbubbleattr.transform.localScale.x);


                float ditsanceafter = Vector3.Distance(temppossafter, transform.position);
                float distancebefore = Vector3.Distance(temppossbefore, transform.position);

                if (ditsanceafter > distancebefore)
                {
                    Debug.Log("before");
                    targetbubbleattr.isfirstbubble = false;
                    this.bubbleattr.isfirstbubble = true;
                    
                    insertBubbleInRow(targetbubbleattr, 1);

                }
                else if (ditsanceafter < distancebefore)
                {
                    Debug.Log("after");
                    insertBubbleInRow(targetbubbleattr, 2);
                }



            }
            else if (targetbubbleattr.islastbubble && !targetbubbleattr.isfirstbubble)
            {
                Debug.Log("lastbubble hit");

                Vector3 temppossbefore = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance + targetbubbleattr.transform.localScale.x);
                Vector3 temppossafter = targetbubbleattr.mathe.CalcPositionByDistance(targetbubbleattr.distance - targetbubbleattr.transform.localScale.x);


                float ditsanceafter = Vector3.Distance(temppossafter, transform.position);
                float distancebefore = Vector3.Distance(temppossbefore, transform.position);

                if (ditsanceafter > distancebefore)
                {
                    Debug.Log("before");
                

                    insertBubbleInRow(targetbubbleattr, 1);

                }
                else if (ditsanceafter < distancebefore)
                {
                    Debug.Log("after");

                    targetbubbleattr.islastbubble  = false;
                    this.bubbleattr.islastbubble = true;
                    insertBubbleInRow(targetbubbleattr, 2);

                }

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
            if (this.bubbleattr.isfirstbubble)
            {
                this.bubbleattr.afterbubble = this.targetbubble.transform;
                targetbubbleattr.beforebubble = transform;
                this.bubbleattr.bubblesinserted = this.bubbleattr.afterbubble.GetComponent<Bubble>().bubblesinserted + 1;
                this.bubbleattr.cursor.Distance = targetbubbleattr.distance;


            }
            else if (!this.bubbleattr.isfirstbubble)
            {
                this.bubbleattr.beforebubble = targetbubbleattr.beforebubble;
                this.bubbleattr.afterbubble = this.targetbubble.transform;
                targetbubbleattr.beforebubble = transform;
                this.bubbleattr.bubblesinserted = this.bubbleattr.beforebubble.GetComponent<Bubble>().bubblesinserted;
            }

     
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
