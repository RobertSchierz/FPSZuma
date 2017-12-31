using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMaster : MonoBehaviour {


    
    public Transform bubbles;

    public BGCurve curve;

    public Transform[] bubbleprefabs = new Transform[4];

    public Transform explosionPrefab;

    public float bubbleSizeAverage;

    public bool stopAll = false;

    public AudioManager audioManager;

    public Score score;



    void Awake() {
        this.curve = FindObjectOfType<BGCurve>();
        this.bubbleSizeAverage = this.bubbleprefabs[0].transform.localScale.x;
        this.audioManager = AudioManager.instance;
        this.score = Score.instance;
    }

    public IEnumerator explosionEffectHandler(Vector3 position, float seconds)
    {
        float elapsedTime = 0;

        Transform explosionEffect = GameObject.Instantiate(this.explosionPrefab, position, Quaternion.identity);

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(explosionEffect.gameObject);
       
    }

    public void startExplosionCoroutine(Vector3 position)
    {
        StartCoroutine(explosionEffectHandler(position, 2));
    }
}
