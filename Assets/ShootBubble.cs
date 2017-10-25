using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour {

    public GameObject camera;
    private Transform shootedbubble;
    public Transform[] bubbleprefabs = new Transform[4];
    private GameMaster gamemasterattributes;

    private GameObject level;

    public float bubbleforce = 200.0f;


    void Start () {
        this.camera = gameObject;
        this.level = GameObject.FindGameObjectWithTag("Level");
        Leveltrigger.onOutoflevel += handleBubbleoutr;
    }
	
	
	void Update () {

        if (Input.GetButtonDown("Fire1"))
        {
            shootBubble();
        }


    }

    void handleBubbleoutr()
    {

        Debug.Log("Bubble hat Level verlassen");

    }

    private void shootBubble()
    {
       this.shootedbubble =  Instantiate(randomizePrefabs(), this.camera.transform.position, this.camera.transform.rotation);
        this.shootedbubble.gameObject.GetComponent<Rigidbody>().AddForce(this.camera.transform.forward * bubbleforce);
 
    }



    public Transform randomizePrefabs()
    {
        int randomprefabindex = UnityEngine.Random.Range(0, 4);

        return this.bubbleprefabs[randomprefabindex];
    }
}
