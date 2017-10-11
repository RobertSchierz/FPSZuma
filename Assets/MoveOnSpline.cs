using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSpline : MonoBehaviour {

    public BGCurve curve;
    private BGCcMath mathe;
    public BGCcCursor cursor;
  
   
    
    private float min = 0.0f;
    private float max = 1.0f;
    private float steps = 1.0f;
    private float distanceratio;
    public float seconds = 10.0f;



    void Start () {

        this.distanceratio = this.min;

        this.curve = FindObjectOfType<BGCurve>();

        this.mathe = this.curve.GetComponent<BGCcMath>();

        this.cursor = this.mathe.gameObject.AddComponent<BGCcCursor>();
       

    }
	

	void Update () {

     

        if (this.distanceratio <= this.max)
        {
            this.cursor.DistanceRatio = this.distanceratio;
            this.distanceratio += ((this.steps * Time.deltaTime) / this.seconds);
            transform.position = this.mathe.CalcPositionByDistanceRatio(this.distanceratio);


        }else
        {
            Destroy(gameObject);
        }

        

    }

}
