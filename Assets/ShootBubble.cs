using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{

    public GameObject camera;
    private Transform shootedBubble;
    public Transform[] bubblePrefabs = new Transform[4];
    private GameMaster gameMasterAttributes;
    private int prefabIndex;

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
        Vector3 shootPos = new Vector3(this.camera.transform.position.x, Waypoints.points[1].transform.position.y, this.camera.transform.position.z);
        this.shootedBubble = Instantiate(randomizePrefabs(), shootPos, this.camera.transform.rotation);
        this.shootedBubble.gameObject.GetComponent<Rigidbody>().AddForce(this.camera.transform.forward * bubbleforce);
        this.shootedBubble.GetComponent<Bubble>().isShooted = true;
        this.shootedBubble.GetComponent<Bubble>().bubbleColor = this.prefabIndex;

    }



    public Transform randomizePrefabs()
    {
        int randomPrefabIndex = UnityEngine.Random.Range(0, 4);
        this.prefabIndex = randomPrefabIndex;
        return this.bubblePrefabs[randomPrefabIndex];
    }
}
