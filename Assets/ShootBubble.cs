using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{

    public GameObject camera;
    private Transform shootedBubble;
    public Transform[] bubblePrefabs = new Transform[4];
    public Transform nextBubble;
    public int nextBubbleIndex;
    private GameMaster gameMasterAttributes;
    private int prefabIndex;

    private GameObject level;

    public float bubbleforce = 200f;
    private float timeBetweenShots = 0.5f;
    private float timestamp;
    



    void Start()
    {
        this.gameMasterAttributes = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
        this.camera = gameObject;
        this.level = GameObject.FindGameObjectWithTag("Level");
        Leveltrigger.onOutoflevel += handleBubbleout;
        this.nextBubble = randomizePrefabs();
        Debug.Log("<b>" + getBubbleColor(this.nextBubbleIndex) + "</b>");


    }




    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.nextBubbleIndex = 0;
            this.prefabIndex = 0;
            this.nextBubble = this.bubblePrefabs[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.nextBubbleIndex = 1;
            this.prefabIndex = 1;
            this.nextBubble = this.bubblePrefabs[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.nextBubbleIndex = 2;
            this.prefabIndex = 2;
            this.nextBubble = this.bubblePrefabs[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            this.nextBubbleIndex = 3;
            this.prefabIndex = 3;
            this.nextBubble = this.bubblePrefabs[3];
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= timestamp)
        {
            
            shootBubble();
            this.nextBubble = randomizePrefabs();
            this.timestamp = Time.time + this.timeBetweenShots;
            Debug.Log(getBubbleColor(this.nextBubbleIndex));



        }
    }

    private String getBubbleColor(int color)
    {
        string returnText = "Nächste Farbe: ";
        switch (color)
        {
            case 0:
                returnText += "Rot";
                break;
            case 1:
                returnText += "Blau";
                break;
            case 2:
                returnText += "Grün";
                break;
            case 3:
                returnText += "Gelb";
                break;
            default:
                returnText += "Fehler";
                break;
        }
        return returnText;
    }

    void handleBubbleout()
    {
        //Debug.Log("Bubble hat Level verlassen");
    }

    private void shootBubble()
    {
        this.gameMasterAttributes.audioManager.handleSound("ShootBubble", 1);
        Vector3 shootPos = new Vector3(this.camera.transform.position.x, Waypoints.points[1].transform.position.y, this.camera.transform.position.z);
        this.shootedBubble = Instantiate(this.nextBubble, shootPos, this.camera.transform.rotation);
        this.shootedBubble.gameObject.GetComponent<Rigidbody>().AddForce(this.camera.transform.forward * bubbleforce);
        this.shootedBubble.GetComponent<Bubble>().isShooted = true;
        this.shootedBubble.GetComponent<Bubble>().bubbleColor = this.prefabIndex;

    }



    public Transform randomizePrefabs()
    {
        int randomPrefabIndex = UnityEngine.Random.Range(0, 4);
        this.prefabIndex = randomPrefabIndex;
        this.nextBubbleIndex = randomPrefabIndex;
        return this.bubblePrefabs[randomPrefabIndex];
    }
}
