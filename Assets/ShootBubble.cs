using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{

    public GameObject camera;
    private Transform shootedbubble;
    public Transform[] bubbleprefabs = new Transform[4];
    private GameMaster gamemasterattributes;
    private int prefabindex;

    private GameObject level;

    public float bubbleforce = 200.0f;


    void Start()
    {
        this.camera = gameObject;
        this.level = GameObject.FindGameObjectWithTag("Level");
        Leveltrigger.onOutoflevel += handleBubbleout;

    }


    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            shootBubble();
        }


    }

    void handleBubbleout()
    {

        //Debug.Log("Bubble hat Level verlassen");

    }

    private void shootBubble()
    {
        Vector3 shootpos = new Vector3(this.camera.transform.position.x, Waypoints.points[1].transform.position.y, this.camera.transform.position.z);
        this.shootedbubble = Instantiate(randomizePrefabs(), shootpos, this.camera.transform.rotation);
        this.shootedbubble.gameObject.GetComponent<Rigidbody>().AddForce(this.camera.transform.forward * bubbleforce);
        this.shootedbubble.GetComponent<Bubble>().isshooted = true;
        this.shootedbubble.GetComponent<Bubble>().bubblecolor = this.prefabindex;

    }



    public Transform randomizePrefabs()
    {
        int randomprefabindex = UnityEngine.Random.Range(0, 4);
        this.prefabindex = randomprefabindex;
        return this.bubbleprefabs[randomprefabindex];
    }
}
