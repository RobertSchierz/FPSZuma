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

    private Wavespawner wavespawner;

    private Bubble bubbleAttributes;

    private float animationEnd;
    public float animationStart = 0f;
    public delegate void animationUpdate();
    public static event animationUpdate OnAnimationUpdate;

    public bool waitAfterExplosion = false;

    public float distanceCalc;





    void Start()
    {

        this.gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.gamemaster.GetComponent<GameMaster>();
        this.wavespawner = this.gamemaster.GetComponent<Wavespawner>();

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

        this.animationEnd = this.gameMasterAttributes.bubbleSizeAverage;




    }


    void Update()
    {
        this.seconds = this.gamemaster.GetComponent<Wavespawner>().actualBubblespeed; 

        if (this.distanceRatio <= this.max)
        {
            if (!this.gameMasterAttributes.stopAll)
            {
                if (this.bubbleAttributes.interpolate)
                {
                    insertAnimation();
                }
                else
                {
                    moveOnSpline();
                }
            }  
        }

               /*
                   if (this.isFirstBubble)
                   {
                       transform.position = Waypoints.points[Waypoints.points.Length - 1].position;
                       //Debug.Log("Verloren");
                       onLostGame();
                   }
               }

           */

        }

    private void moveOnSpline()
    {
        this.distanceCalc += (this.mathe.GetDistance() * Time.deltaTime) / this.seconds;

        this.cursor.Distance = this.distanceCalc ;
        transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);

    
    }

    public void insertAnimation()
    {
        if(this.animationStart < this.animationEnd)
        {
            this.animationStart += ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds) * 2;
            this.distanceCalc += ((this.mathe.GetDistance() * Time.deltaTime) / this.seconds) * 3;
            this.cursor.Distance = this.distanceCalc;
            transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
        }
        else
        {
            moveOnSpline();
            setAnimationValuesBack();
        }
  
    }

    private void setAnimationValuesBack()
    {
      
        this.bubbleAttributes.interpolate = false;
        this.animationStart = 0.0f;
        OnAnimationUpdate();


    }

   


   

}
