﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveltrigger : MonoBehaviour {

    public delegate void outOfLevel();
    public static event outOfLevel onOutoflevel;

    void OnTriggerExit(Collider col)
    {
        //Destroy(col.gameObject);
        onOutoflevel();
    }
}
