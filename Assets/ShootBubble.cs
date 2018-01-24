using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBubble : MonoBehaviour
{

    public GameObject camera;
    private Transform shootedBubble;
    public Transform[] bubblePrefabs = new Transform[4];

    public Transform[] previewPrefabs = new Transform[4];
    public GameObject previewBubbles;
    public GameObject previewBubbles2;

    public Transform[] nextBubble = new Transform[2];
    public int[] nextBubbleIndex = new int[2];
    private GameMaster gameMasterAttributes;


    private GameObject level;

    public float bubbleforce = 50f;
    private float timeBetweenShots = 1.0f;
    private float timestamp;
    private bool isSwitching = false;

  



    void Start()
    {
        this.timeBetweenShots = 0.7f;
        this.gameMasterAttributes = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
        this.camera = gameObject;
        this.level = GameObject.FindGameObjectWithTag("Level");
        this.previewBubbles = GameObject.Find("PreviewBubbles");
        this.previewBubbles2 = GameObject.Find("PreviewBubbles2");
        this.nextBubble[0] = randomizePrefabs(0);
        this.nextBubble[1] = randomizePrefabs(1);
       // Debug.Log("<b>" + getBubbleColor(this.nextBubbleIndex[0]) + "</b>");



    }




    void Update()
    {

        if (!Wavespawner.lostgame)
        {



            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.nextBubbleIndex[0] = 0;

                this.nextBubble[0] = this.bubblePrefabs[0];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.nextBubbleIndex[0] = 1;

                this.nextBubble[0] = this.bubblePrefabs[1];
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.nextBubbleIndex[0] = 2;

                this.nextBubble[0] = this.bubblePrefabs[2];
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                this.nextBubbleIndex[0] = 3;

                this.nextBubble[0] = this.bubblePrefabs[3];
            }

            if (Input.GetButtonDown("Fire1") && Time.time >= timestamp && !this.isSwitching && !PauseMenu.gameIsPaused)
            {


                shootBubble();
                //this.nextBubble = randomizePrefabs();
                this.timestamp = Time.time + this.timeBetweenShots;
               // Debug.Log(getBubbleColor(this.nextBubbleIndex[0]));
            }

            if (Input.GetButtonDown("Fire2") && !this.isSwitching)
            {
                this.gameMasterAttributes.audioManager.handleSound("Bubbleswitch", 1);
                switchBubbles();

            }

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


    private void shootBubble()
    {

        this.gameMasterAttributes.audioManager.handleSound("ShootBubble", 1);
        Vector3 shootPos = new Vector3(this.camera.transform.position.x, Waypoints.points[1].transform.position.y, this.camera.transform.position.z);
        this.shootedBubble = Instantiate(this.nextBubble[0], shootPos, this.camera.transform.rotation);
        this.shootedBubble.gameObject.GetComponent<Rigidbody>().AddForce(this.camera.transform.forward * bubbleforce);
        this.shootedBubble.GetComponent<Bubble>().isShooted = true;
        this.shootedBubble.GetComponent<Bubble>().bubbleColor = this.nextBubbleIndex[0];
        generateNewBubbles();

    }


    private void switchBubbles()
    {
        Transform temp = this.nextBubble[0];
        int tempint = this.nextBubbleIndex[0];
        this.nextBubble[0] = this.nextBubble[1];
        this.nextBubble[1] = temp;
        this.nextBubbleIndex[0] = this.nextBubbleIndex[1];
        this.nextBubbleIndex[1] = tempint;

        StartCoroutine(MoveOverSeconds(this.previewBubbles.transform.GetChild(0).gameObject, this.previewBubbles2.transform.position, 0.2f, 2));
        StartCoroutine(MoveOverSeconds(this.previewBubbles2.transform.GetChild(0).gameObject, this.previewBubbles.transform.position, 0.2f, 3));

    }


    private void generateNewBubbles()
    {
        this.nextBubble[0] = this.nextBubble[1];
        this.nextBubbleIndex[0] = this.nextBubbleIndex[1];
        //this.previewBubbles.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(this.previewBubbles.transform.GetChild(0).gameObject);



        StartCoroutine(MoveOverSeconds(this.previewBubbles2.transform.GetChild(0).gameObject, this.previewBubbles.transform.position, 0.2f, 1));
       
    }



    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds, int option)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            this.isSwitching = true;
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.isSwitching = false;
        objectToMove.transform.position = end;


        switch (option)
        {
            case 1:
                
                this.nextBubble[1] = randomizePrefabs(1);
                this.previewBubbles2.transform.GetChild(0).parent = this.previewBubbles.transform;
                break;

            case 2:
                this.previewBubbles2.transform.GetChild(0).parent = this.previewBubbles.transform;
                break;

            case 3:
                this.previewBubbles.transform.GetChild(0).parent = this.previewBubbles2.transform;
                break;
            default:
                break;
        }


    }



    public Transform randomizePrefabs(int position)
    {
        int randomPrefabIndex = UnityEngine.Random.Range(0, 4);
        this.nextBubbleIndex[position] = randomPrefabIndex;
        setPreviewBubbles(position, randomPrefabIndex);
        return this.bubblePrefabs[randomPrefabIndex];
    }

    private void setPreviewBubbles(int position, int randomPrefabIndex)
    {
        switch (position)
        {
            case 0:
                Transform previewBubble = Instantiate(this.previewPrefabs[randomPrefabIndex], this.previewBubbles.transform.position, Quaternion.identity);
                previewBubble.parent = this.previewBubbles.transform;

                break;
            case 1:
                Transform previewBubble2 = Instantiate(this.previewPrefabs[randomPrefabIndex], this.previewBubbles2.transform.position, Quaternion.identity);
                previewBubble2.parent = this.previewBubbles2.transform;

                break;
            default:
                break;
        }
    }
}
