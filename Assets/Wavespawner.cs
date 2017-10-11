using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour {

    public Transform bubblePrefab;
    public Transform spawnPoint;
    public Transform bubbles;

    public BGCurve curve;

    //First Start
    private float countdown = 2f;
    private bool wavespaned = false;



    // Bubble Attributes
    public int bubblecountperwave = 10;
    public float bubblespeed = 10.0f;

    // First Start - Intro
    private int introbubblenumber;
    private float introbubblespeed;
    public float actualbubblespeed;



    void Start()
    {
        this.curve = FindObjectOfType<BGCurve>();
        this.spawnPoint.transform.position = this.curve.Points[0].PositionLocal;

        Startpoint.onBuildBubble += handleOnBuildBubble;

        this.introbubblenumber = this.bubblecountperwave / 10;
        this.introbubblespeed = this.bubblespeed / 8.0f;
        this.actualbubblespeed = introbubblespeed;
   
    }

    void handleOnBuildBubble()
    {
        if (bubbles.childCount < this.bubblecountperwave)
        {
            
            if (bubbles.childCount == this.introbubblenumber)
            {
                this.actualbubblespeed = this.bubblespeed;
            }
            spawnBubble();
        }
    }

    void Update()
    {
        if (countdown <= 0f && !this.wavespaned)
        {
            //StartCoroutine( spawnWave());
            spawnBubble();
            this.wavespaned = true;

        }else if (!this.wavespaned)
        {
            countdown -= Time.deltaTime;
        }
        
            
        

        
    }

    /*IEnumerator spawnWave()
    {
       
        for (int i = 0; i < bubblecountperwave; i++)
        {

            spawnBubble();
            yield return new WaitForSeconds(0.1f);
        }

       
    }*/

    void spawnBubble()
    {
       var bubble =  Instantiate(bubblePrefab, spawnPoint.position, spawnPoint.rotation);
        bubble.transform.parent = this.bubbles.transform;
    }


}
