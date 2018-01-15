using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Bubble : MonoBehaviour
{
    [Header("Controller Attributes")]
    public GameObject Gamemaster;
    public GameMaster gameMasterAttributes;
    public Wavespawner waveSpawner;
    public GameObject cam;

    [Space]
    [Header("Own Attributes")]
    public GameObject bubble;
    public int speciality;
    public bool isShooted = false;
    public int bubbleColor;
    public Rigidbody rigidBodyAttr;
    private int currentWaypoint;


    [Space]
    [Header("Math Attributes")]
    public BGCurve curve;
    public BGCcMath mathe;
    public BGCcCursor cursor;



    [Space]
    [Header("Chain Attributes")]
    public Transform bubbles;
    public Transform beforeBubble;
    public Transform afterBubble;
    public bool isFirstBubble = false;
    public bool isLastBubble = false;
    public Transform[] movedBubbleRow;
    public bool interpolate = false;
    public bool rollback = false;
    //public Transform rollbackBorderBubble;
    public float rotationSpeed;
    public bool isRollbackBorderBubble = false;



    void Start()
    {
        this.cam = GameObject.FindGameObjectWithTag("MainCamera");
        this.Gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.Gamemaster.GetComponent<GameMaster>();
        this.waveSpawner = this.Gamemaster.GetComponent<Wavespawner>();

        this.bubble = transform.gameObject;
        this.bubbles = this.gameMasterAttributes.bubbles;
        this.curve = this.gameMasterAttributes.curve;
        this.mathe = this.curve.GetComponent<BGCcMath>();
        this.cursor = this.mathe.gameObject.AddComponent<BGCcCursor>();
        this.rigidBodyAttr = transform.GetComponent<Rigidbody>();

        this.rotationSpeed = 150;

        this.currentWaypoint = this.cursor.CalculateSectionIndex();

        this.movedBubbleRow = new Transform[this.bubbles.childCount];

        if (!this.isShooted)
        {
            this.checkBubbleState();
            if (!this.isFirstBubble)
            {
                this.bubble.GetComponent<MoveOnSpline>().distanceCalc = this.bubble.GetComponent<Bubble>().beforeBubble.GetComponent<MoveOnSpline>().distanceCalc - this.gameMasterAttributes.bubbleSizeAverage;
            }

        }

    }


    public void checkBubblerowInfront()
    {

        this.movedBubbleRow = new Transform[this.bubble.transform.GetSiblingIndex() + 1];
        for (int i = 0; i <= this.bubble.transform.GetSiblingIndex(); i++)
        {
            this.movedBubbleRow[i] = this.bubbles.GetChild(i);
        }


    }


    public void checkBubbleState()
    {
        if (this.bubbles.GetChild(0).gameObject == gameObject)
        {
            this.isFirstBubble = true;
            this.movedBubbleRow[0] = transform;

        }
        else
        {
            this.beforeBubble = this.bubbles.GetChild(this.bubbles.childCount - 2);
            for (int i = 0; i < this.bubbles.childCount; i++)
            {
                this.movedBubbleRow[i] = this.bubbles.GetChild(i);
            }
        }

        if (this.bubbles.childCount == this.waveSpawner.bubbleCountPerWave)
        {
            this.isLastBubble = true;
        }


        if (!this.isFirstBubble)
        {
            this.bubbles.GetChild(this.bubbles.childCount - 2).GetComponent<Bubble>().afterBubble = transform;
        }


    }

    void Update()
    {
       

        if (!this.isShooted && !Wavespawner.lostgame)
        {
            Vector3 distanceToCam = this.cam.transform.position - transform.position;



            if (this.bubble.GetComponent<MoveOnSpline>().explosionCounter == 0)
            {
                
                transform.Rotate(distanceToCam * (Time.deltaTime) * this.rotationSpeed, Space.World);
            }else
            {
                if (this.rollback)
                {
                    transform.Rotate(-distanceToCam * (Time.deltaTime) * this.rotationSpeed, Space.World);
                }
            }


            // transform.RotateAround(Vector3.zero, distanceToCam, 200 * Time.deltaTime);
            //transform.position = this.mathe.CalcPositionByDistance(this.cursor.Distance);

        }







        /*
                if (this.cursor.CalculateSectionIndex() != this.currentWaypoint)
                {


                }
                this.currentWaypoint = this.cursor.CalculateSectionIndex();

                Vector3 targetdir = Waypoints.points[this.cursor.CalculateSectionIndex()].position - transform.position;
                float step = 2 * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, step, 20.0F);
                */

        // Quaternion lookToWaypoint = Quaternion.LookRotation(newDir);
        // transform.rotation = lookToWaypoint;






        //temp.eulerAngles = (1,1,0);

        // this.angleSpin += Time.deltaTime * 100;
        //  Quaternion temp
        //  temp *= Quaternion.Euler(Vector3.up * 20);


        // Quaternion calcQuaternion = transform.rotation;
        //calcQuaternion *= Quaternion.Euler(-distanceToCam * 10) * transform.rotation;



        //transform.rotation = calcQuaternion;




        //transform.rotation = lookToWaypoint;













    }

}
