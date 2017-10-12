using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour {

    
    public Transform spawnPoint;
    public Transform bubbles;

    public Transform[] bubbleprefabs = new Transform[4];
    private int lastbubbleprefabindex;

    [Range(0,10)]
    public int leveldifficult = 0;


    public BGCurve curve;

    //First Start
    private float countdown = 2f;
    private bool wavespaned = false;



    // Bubble Attributes
    public int bubblecountperwave = 10;
    public float bubblespeed = 10.0f;

    // First Start - Intro
    private int introbubblenumber;
    private float introbubblespeed;
    public float actualbubblespeed;



    void Start()
    {

       
        this.curve = FindObjectOfType<BGCurve>();
        this.spawnPoint.transform.position = this.curve.Points[0].PositionLocal;

        Startpoint.onBuildBubble += handleOnBuildBubble;

        this.introbubblenumber = this.bubblecountperwave / 10;
        this.introbubblespeed = this.bubblespeed / 5.0f;
        this.actualbubblespeed = introbubblespeed;
   
    }

 

    

    void handleOnBuildBubble()
    {
        if (bubbles.childCount < this.bubblecountperwave)
        {
            

            if (bubbles.childCount == this.introbubblenumber)
            {
                this.actualbubblespeed = this.bubblespeed;
            }
            spawnBubble();
            
        }
    }

    void Update()
    {
        if (countdown <= 0f && !this.wavespaned)
        {

            spawnBubble();
            this.wavespaned = true;

        }else if (!this.wavespaned)
        {
            countdown -= Time.deltaTime;
        }
        
            
        

        
    }

    public Transform randomizePrefabs()
    {
        int randomprefabindex = Random.Range(0, 4);

        

        if (this.leveldifficult != 0)
        {
          
        if (this.lastbubbleprefabindex == randomprefabindex)
        {
            if (calculateNewMix()) {
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
                if (Random.Range(0,2) == 0)
                {
                    return true;
                }else
                {
                    return false;
                }
            }else
            {
                return false;
            }
            
        }else
        {
            return true;
        }


    }

    void spawnBubble()
    {
     
       var bubble =  Instantiate(this.randomizePrefabs(), spawnPoint.position, spawnPoint.rotation);
        bubble.transform.parent = this.bubbles.transform;
    }


}
