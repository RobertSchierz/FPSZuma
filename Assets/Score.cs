using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public TextMeshProUGUI scoreText;
    public int score = 0;

    public static Score instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        this.scoreText = GameObject.FindGameObjectWithTag("Scoretext").GetComponent<TextMeshProUGUI>();
    }

     public void updateScore(int addedScore)
    {
        this.score += addedScore;
        this.scoreText.text = this.score.ToString();
    }

}
