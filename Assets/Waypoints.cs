using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {


    public static Transform[] points;

    void Awake()
    {
        points = new Transform[transform.childCount-1];

        for (int i = 1; i < transform.childCount; i++)
        {
            points[i-1] = transform.GetChild(i);
        }
    }
}
