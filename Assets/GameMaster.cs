﻿using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {


    
    public Transform bubbles;

    public BGCurve curve;

    public Transform[] bubbleprefabs = new Transform[4];

    public float bubblesizeaverage;


    void Start() {
        this.curve = FindObjectOfType<BGCurve>();
        this.bubblesizeaverage = this.bubbleprefabs[0].transform.localScale.x;
    }
	

}
