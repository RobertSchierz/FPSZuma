using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpline : MonoBehaviour {

    public BGCurve curve;
    public BGCcMath mathe;
    public BGCcCursor cursor;
    
    private float min = 0.0f;
    private float max = 1.0f;
    private float steps = 1.0f;
    public float distanceratio;
    public float seconds;

    public Transform bubbles;
    public Transform lastbubble;

    private bool isfirstbubble = false;

    public delegate void lostGame();
    public static event lostGame onLostGame;





    void Start () {


        this.bubbles = GameObject.FindGameObjectWithTag("GameController").GetComponent<Wavespawner>().bubbles;


        this.distanceratio = this.min;

        this.curve = FindObjectOfType<BGCurve>();

        this.mathe = this.curve.GetComponent<BGCcMath>();

        this.cursor = this.mathe.gameObject.AddComponent<BGCcCursor>();

        if (this.bubbles.GetChild(0).gameObject == gameObject)
        {
            this.isfirstbubble = true;
         

        }else
        {
            this.lastbubble = this.bubbles.GetChild(this.bubbles.childCount - 2);
        }
     

    }
	

	void Update () {


        

        this.seconds = GameObject.FindGameObjectWithTag("GameController").GetComponent<Wavespawner>().actualbubblespeed;

        if (this.distanceratio <= this.max)
        {

            
            if (this.isfirstbubble) {
                this.cursor.DistanceRatio = this.distanceratio;
                this.distanceratio += ((this.steps * Time.deltaTime) / this.seconds);
                transform.position = this.mathe.CalcPositionByDistanceRatio(this.distanceratio);
            }else
            {
             
                this.cursor.Distance = this.lastbubble.gameObject.GetComponent<MoveOnSpline>().cursor.Distance - this.lastbubble.gameObject.transform.localScale.x;
                transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);
            }

          

        }
        else
        {
            if (this.isfirstbubble)
            {
                transform.position = Waypoints.points[Waypoints.points.Length-1].position;
                Debug.Log("Verloren");
                onLostGame();
            }
        }

        

    }

}
