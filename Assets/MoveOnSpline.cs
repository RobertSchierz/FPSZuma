using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpline : MonoBehaviour
{
    private BGCurve curve;
    private BGCcMath mathe;
    private BGCcCursor cursor;

    private float min = 0.0f;
    private float max = 1.0f;
    private float steps = 1.0f;
    private float distanceRatio;
    private float seconds;

    private Transform bubbles;
    private Transform beforeBubble;
    private Transform afterBubble;

    private bool isFirstBubble;
    private bool isLastBubble;

    public delegate void lostGame();
    public static event lostGame onLostGame;

    private GameObject gamemaster;
    private GameMaster gameMasterAttributes;

    private Bubble bubbleAttributes;
    private int bubblesInserted;


    void Start()
    {

        this.gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.gamemaster.GetComponent<GameMaster>();

        this.bubbleAttributes = transform.gameObject.GetComponent<Bubble>();

        this.bubbles = this.gameMasterAttributes.bubbles;

        this.distanceRatio = this.min;

        this.curve = this.bubbleAttributes.curve;

        this.mathe = this.bubbleAttributes.mathe;

        this.cursor = this.bubbleAttributes.cursor;

        this.beforeBubble = this.bubbleAttributes.beforeBubble;

        this.afterBubble = this.bubbleAttributes.afterBubble;

        this.isFirstBubble = bubbleAttributes.isFirstBubble;

        this.isLastBubble = bubbleAttributes.isLastBubble;




    }


    void Update()
    {

        this.bubblesInserted = this.bubbleAttributes.bubblesInserted;


        this.seconds = this.gamemaster.GetComponent<Wavespawner>().actualBubblespeed;

        if (this.distanceRatio <= this.max)
        {


            if (this.isFirstBubble)
            {
                /*this.cursor.DistanceRatio = this.distanceratio;
                  this.distanceratio += ((this.steps * Time.deltaTime) / this.seconds);
                  transform.position = this.mathe.CalcPositionByDistanceRatio(this.distanceratio);*/

                
                this.cursor.Distance += ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds);
                transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance + (this.bubblesInserted * this.gameMasterAttributes.bubbleSizeAverage));

            }
            else
            {

                this.cursor.Distance = this.beforeBubble.gameObject.GetComponent<Bubble>().distance - this.gameMasterAttributes.bubbleSizeAverage;
                transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance + (this.bubblesInserted * this.gameMasterAttributes.bubbleSizeAverage));
            }



        }
        else
        {
            if (this.isFirstBubble)
            {
                transform.position = Waypoints.points[Waypoints.points.Length - 1].position;
                //Debug.Log("Verloren");
                onLostGame();
            }
        }



    }

}
