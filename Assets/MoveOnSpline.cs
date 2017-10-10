using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpline : MonoBehaviour {

    public BGCurve curve;
    public BGCcMath mathe;
    public BGCcCursor cursor;
    public BGCcCursorObjectTranslate movedObject;
   

    //public float speed = 10f;
    //private Transform target;
    //private int wavepointIndex = 0;

    
    private float min = 0.0f;
    private float max = 1.0f;
    private float steps = 1.0f;
    private float distanceratio;
    public float seconds = 10.0f;


    // Use this for initialization
    void Start () {

       // target = Waypoints.points[0];

        this.mathe = this.curve.GetComponent<BGCcMath>();
        this.cursor = this.curve.GetComponent<BGCcCursor>();
        this.movedObject = this.mathe.GetComponent<BGCcCursorObjectTranslate>();


        this.movedObject.ObjectToManipulate = gameObject.transform;


        //var point = this.curve.Points[0];

       // transform.position = point.PositionWorld;

       


        this.distanceratio = this.min;




    }
	
	// Update is called once per frame
	void Update () {

     

        if (this.distanceratio <= this.max)
        {
            this.cursor.DistanceRatio = this.distanceratio;
            this.distanceratio += ((this.steps * Time.deltaTime) / this.seconds);
            print(Time.time);
        }

        


        /*

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        
   
        }*/


    }

    /*
    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }
    */

}
