using BansheeGz.BGSpline.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bubble : MonoBehaviour {

    
    private GameObject bubble;
    public int speciality;

    public BGCcCursor cursor;
    public float distance;
    public float distanceratio;

    void Start()
    {
        this.bubble = transform.gameObject;
    }

    void update()
    {
       
    }

}
