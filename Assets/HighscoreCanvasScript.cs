using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreCanvasScript : MonoBehaviour {

    public TextMeshProUGUI scoretext;

    // Use this for initialization
    void Start () {
        scoretext.text = Score.instance.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
