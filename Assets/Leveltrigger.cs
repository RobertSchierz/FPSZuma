using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveltrigger : MonoBehaviour {

    public GameObject Gamemaster;
    public GameMaster gameMasterAttributes;



    void Start()
    {
        this.Gamemaster = GameObject.FindGameObjectWithTag("GameController");
        this.gameMasterAttributes = this.Gamemaster.GetComponent<GameMaster>();
    }

    void OnTriggerExit(Collider col)
    {
        this.gameMasterAttributes.startExplosionCoroutine(col.transform.position, 2);
        this.gameMasterAttributes.audioManager.handleSound("FinalBallExplosion", 1);
        Destroy(col.gameObject);
   
    }
}
