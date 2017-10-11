﻿using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour {

    public Transform bubblePrefab;
    public Transform spawnPoint;

    public BGCurve curve;

    public float timeBetweenBubbles = 0.5f;
    private float countdown = 2f;
    private bool wavespaned = false;

    private int bubblecountperwave = 10; 

    void Start()
    {
        this.curve = FindObjectOfType<BGCurve>();
        this.spawnPoint.transform.position = this.curve.Points[0].PositionLocal;
    }

    void Update()
    {
        if (countdown <= 0f && !this.wavespaned)
        {
            StartCoroutine( spawnWave());
            this.wavespaned = true;

        }else if (!this.wavespaned)
        {
            countdown -= Time.deltaTime;
        }
        
            
        

        
    }

    IEnumerator spawnWave()
    {
       
        for (int i = 0; i < bubblecountperwave; i++)
        {

            spawnBubble();
            yield return new WaitForSeconds(0.1f);
        }

       
    }

    void spawnBubble()
    {
        Instantiate(bubblePrefab, spawnPoint.position, spawnPoint.rotation);
    }


}
