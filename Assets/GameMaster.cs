using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {


    
    public Transform bubbles;

    public BGCurve curve;

    public Transform[] bubbleprefabs = new Transform[4];

    public float bubbleSizeAverage;

    public bool stopAll = false;

    public AudioManager audioManager;



    void Awake() {
        this.curve = FindObjectOfType<BGCurve>();
        this.bubbleSizeAverage = this.bubbleprefabs[0].transform.localScale.x;
        this.audioManager = AudioManager.instance;
    }
	

}
