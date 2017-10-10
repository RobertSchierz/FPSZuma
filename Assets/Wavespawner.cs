using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour {

    public Transform bubblePrefab;
    public Transform spawnPoint;

    public float timeBetweenBubbles = 5f;
    private float countdown = 2f;

    private int waveIndex = 0; 

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(spawnWave());
            countdown = timeBetweenBubbles;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator spawnWave()
    {
        waveIndex++;
        for (int i = 0; i < waveIndex; i++)
        {

            spawnBubble();
            yield return new WaitForSeconds(0.5f);
        }

       
    }

    void spawnBubble()
    {
        Instantiate(bubblePrefab, spawnPoint.position, spawnPoint.rotation);
    }


}
