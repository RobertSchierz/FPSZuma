using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveLoadScript : MonoBehaviour {

    public static SaveLoadScript instance;
    private Hashtable savedHighscores;
    private bool fileEmpty = false;

	void Awake () {


        if (instance == null)
        {
            DontDestroyOnLoad(instance);
            instance = this;
        }else if (instance != this)
        {
            Destroy(instance);
        }

        if (!File.Exists(Application.persistentDataPath + "/savedHighscores.dat"))
        {
            File.Create(Application.persistentDataPath + "/savedHighscores.dat");
            Debug.Log("Safefile created!");
        }else
        {
            Debug.Log("Safefile already exists!");
            this.loadHighscores();
        }

	}

    public Hashtable getHighscores()
    {
        return this.savedHighscores;
    }

    public void setHighscoreToNull()
    {
        loadHighscores();
        this.savedHighscores = null;
    }
	
    private void loadHighscores()
    {
        Hashtable highscores = null;
        FileStream fs = new FileStream(Application.persistentDataPath + "/savedHighscores.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();


            if (fs.Length != 0)
            {
                highscores = (Hashtable)formatter.Deserialize(fs);
                if (highscores == null)
                {
                    Debug.Log("Data empty");
                }
                else
                {
                    this.fileEmpty = false;
                    this.savedHighscores = highscores;
                    Debug.Log("Data not empty");
                }
            }else
            {
                this.fileEmpty = true;
                Debug.Log("FileStream is empty");
            }

           

        }
        catch (SerializationException e)
        {

            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }


    public bool saveHighscore(String name, int highscore, String date)
    {
        this.loadHighscores();

        Hashtable newHashtable = new Hashtable();

        if (!this.fileEmpty)
        {
            foreach (DictionaryEntry entry in this.savedHighscores)
            {
                newHashtable.Add(entry.Key, entry.Value);
            }
        }

        newHashtable.Add(date + " - " + name, highscore.ToString());

        FileStream fs = new FileStream(Application.persistentDataPath + "/savedHighscores.dat", FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, newHashtable);
            return true;
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            return false;
            throw;
        }
        finally
        {
            fs.Close();
            
        }

    }



}

