using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleTrigger : MonoBehaviour {

    public GameObject Gamemaster;
    public GameMaster gameMasterAttributes;

    void Start()
    {
        this.Gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.Gamemaster.GetComponent<GameMaster>();
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.GetComponent<Bubble>().isShooted)
        {
            if (!col.gameObject.GetComponent<ShootedBubble>().hitted)
            {
               this.gameMasterAttributes.startExplosionCoroutine(col.transform.position, 2);
                this.gameMasterAttributes.audioManager.handleSound("FinalBallExplosion", 1);
                Destroy(col.gameObject);
            }
           
        }
    }
}
