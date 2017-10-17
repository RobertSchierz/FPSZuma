using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour
{





    private Transform[] bubbleprefabs;
    private int lastbubbleprefabindex;
    public Transform spawnPoint;

    [Range(0, 10)]
    public int leveldifficult = 0;

    private bool lostgame = false;


    private BGCurve curve;
    private Transform bubbles;

    //First Start
    private float countdown = 2f;
    private bool wavespaned = false;



    // Bubble Attributes
    public int bubblecountperwave = 10;
    [Range(15, 100)]
    public float bubblespeed = 15.0f;

    // First Start - Intro
    private int introbubblenumber;
    private float introbubblespeed;
    public float actualbubblespeed;

    private bool spawnque = false;

    private GameMaster gamemasterattributes;



    void Start()
    {
        this.gamemasterattributes = transform.gameObject.GetComponent<GameMaster>();

        this.bubbleprefabs = this.gamemasterattributes.bubbleprefabs;

        this.curve = this.gamemasterattributes.curve;
        this.spawnPoint.transform.position = this.curve.Points[0].PositionLocal;

        this.bubbles = this.gamemasterattributes.bubbles;

        
        MoveOnSpline.onLostGame += handleOnLostGame;

        this.introbubblenumber = this.bubblecountperwave / 10;
        this.introbubblespeed = this.bubblespeed / 5f;
        this.actualbubblespeed = introbubblespeed;


    }


    void handleOnLostGame()
    {
        this.lostgame = true;
    }


  

    void Update()
    {


        if (countdown <= 0f && !this.wavespaned)
        {

            spawnBubble();
            this.wavespaned = true;
            this.spawnque = true;

        }
        else if (!this.wavespaned)
        {
            countdown -= Time.deltaTime;
        }




        if (this.spawnque && this.bubbles.childCount < this.bubblecountperwave && !this.lostgame)
        {
            if (bubbles.childCount == this.introbubblenumber)
            {
                this.actualbubblespeed = this.bubblespeed;
            }

            
            if (this.bubbles.GetChild(this.bubbles.childCount -1).GetComponent<Bubble>().distance > 0.5f)
            {
                spawnBubble();
            }

        }







    }

    public Transform randomizePrefabs()
    {
        int randomprefabindex = Random.Range(0, 4);



        if (this.leveldifficult != 0)
        {

            if (this.lastbubbleprefabindex == randomprefabindex)
            {
                if (calculateNewMix())
                {
                    randomprefabindex = Random.Range(0, 4);
                }
            }
        }


        this.lastbubbleprefabindex = randomprefabindex;

        return this.bubbleprefabs[randomprefabindex];
    }

    bool calculateNewMix()
    {
        int randomdetermitter = Random.Range(this.leveldifficult, 11);
        if (randomdetermitter > this.leveldifficult)
        {
            if (this.leveldifficult > 5 && this.leveldifficult < 10)
            {
                if (Random.Range(0, 2) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        else
        {
            return true;
        }


    }

    void spawnBubble()
    {

        var bubble = Instantiate(this.randomizePrefabs(), spawnPoint.position, spawnPoint.rotation);
        bubble.transform.parent = this.bubbles.transform;


    }


}
