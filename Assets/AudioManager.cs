using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    // Use this for initialization
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

        foreach (Sound audio in sounds)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;
            audio.source.volume = audio.volume;
            audio.source.playOnAwake = false;
            audio.source.loop = audio.loop;
            audio.source.pitch = audio.pitch;
        }
    }

    void Start()
    {
        handleSound("Theme",1);
    }

    public void handleSound(string nameOfSound, int option)
    {
        Sound playableSound = Array.Find(sounds, sound => sound.name == nameOfSound);
        if (playableSound == null)
        {
            Debug.Log("Sound: " + nameOfSound + " existiert nicht");
            return;
        }

        switch (option)
        {
            case 1:
                playableSound.source.Play();
                break;
            case 2:
                    StartCoroutine(WaitAndDecreaseVolume(playableSound));
                break;
            case 3:
                playableSound.source.pitch = 1.1f;
                playableSound.pitch = 1.1f;
                playableSound.source.Play();
                playableSound.source.pitch = 1.0f;
                playableSound.pitch = 1.0f;
                break;
            default:
                break;
        }
        
    }


    IEnumerator WaitAndDecreaseVolume(Sound playableSound)
    {
        
        while (playableSound.source.volume > 0)
        {
            yield return null;
            playableSound.source.volume -= 0.0001f;
        }
        playableSound.source.Stop();
        yield break;
    }

  

}
