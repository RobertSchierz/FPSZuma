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
    private float distanceratio;
    private float seconds;

    private Transform bubbles;
    private Transform beforebubble;
    private Transform afterbubble;

    private bool isfirstbubble;
    private bool islastbubble;

    public delegate void lostGame();
    public static event lostGame onLostGame;

    private GameObject gamemaster;
    private GameMaster gamemasterattributes;

    private Bubble bubbleattributes;


    void Start()
    {

        this.gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gamemasterattributes = this.gamemaster.GetComponent<GameMaster>();

        this.bubbleattributes = transform.gameObject.GetComponent<Bubble>();

        this.bubbles = this.gamemasterattributes.bubbles;

        this.distanceratio = this.min;

        this.curve = this.bubbleattributes.curve;

        this.mathe = this.bubbleattributes.mathe;

        this.cursor = this.bubbleattributes.cursor;

        this.beforebubble = this.bubbleattributes.beforebubble;

        this.afterbubble = this.bubbleattributes.afterbubble;

        this.isfirstbubble = bubbleattributes.isfirstbubble;

        this.islastbubble = bubbleattributes.islastbubble;




    }


    void Update()
    {




        this.seconds = this.gamemaster.GetComponent<Wavespawner>().actualbubblespeed;

        if (this.distanceratio <= this.max)
        {


            if (this.isfirstbubble)
            {
                this.cursor.DistanceRatio = this.distanceratio;
                this.distanceratio += ((this.steps * Time.deltaTime) / this.seconds);
                transform.position = this.mathe.CalcPositionByDistanceRatio(this.distanceratio);
            }
            else
            {

                this.cursor.Distance = this.beforebubble.gameObject.GetComponent<MoveOnSpline>().cursor.Distance - this.beforebubble.gameObject.transform.localScale.x;
                transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
            }



        }
        else
        {
            if (this.isfirstbubble)
            {
                transform.position = Waypoints.points[Waypoints.points.Length - 1].position;
                //Debug.Log("Verloren");
                onLostGame();
            }
        }



    }

}
