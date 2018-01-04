using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveLoadScript : MonoBehaviour {

    public static SaveLoadScript instance;
    public GameObject bubbles;

	void Awake () {

        if (instance == null)
        {
            DontDestroyOnLoad(instance);
            instance = this;
        }else if (instance != this)
        {
            Destroy(instance);
        }

	}
	
	public void saveBubbles()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedBubbles.dat");

        for (int i = 0; i < this.bubbles.transform.childCount; i++)
        {

        }
    }


}

[Serializable]
class SavedBubble
{
    public float distance;
    public int prefabindex;
}
