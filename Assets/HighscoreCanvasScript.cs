using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HighscoreCanvasScript : MonoBehaviour {

    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI name;
    

    // Use this for initialization
    void Start () {
        scoretext.text = Score.instance.score.ToString();
	}
	

    public void saveScore()
    {
        System.DateTime theTime = System.DateTime.Now;
        string date = theTime.Day + "-" + theTime.Month + "-" + theTime.Year;
        if (SaveLoadScript.instance.saveHighscore(name.text, Score.instance.score, date))
        {
            Debug.Log("Score gespeichert!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            Debug.Log("Score nicht gespeichert");
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
