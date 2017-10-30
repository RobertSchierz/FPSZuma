﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootedBubble : MonoBehaviour {


    public bool hitted = false;
    public GameObject targetbubble;


    void Start () {
		
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
            }else if (distancetoafterbubble < distancetobeforebubble)
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






        }else if (rowdecision == 2)
        {

            for (int i = 0; i < targetbubbleattr.movedbubblerow.Length; i++)
            {
                targetbubbleattr.movedbubblerow[i].GetComponent<Bubble>().bubblesinserted++;
            }



        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
