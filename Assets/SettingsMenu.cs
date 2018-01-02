using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    private bool playedPreviewSound = false;

	public void setMusicVolume(float volume)
    {
        Sound themeSound = Array.Find(AudioManager.instance.sounds, sound => sound.name == "Theme");
        themeSound.source.volume = volume;
    }

    public void setSoundEffects(float volume)
    {
        foreach (var sound in AudioManager.instance.sounds)
        {
            if (sound.name != "Theme")
            {
                sound.source.volume = volume;
            }
        }
        if (!this.playedPreviewSound)
        {
            AudioManager.instance.handleSound("BubblesTouch", 1);
            StartCoroutine(previewSoundWaiter());
        }
       
    }

    public IEnumerator previewSoundWaiter()
    {
        float elapsedTime = 0;
      
        while (elapsedTime < 0.5)
        {
            this.playedPreviewSound = true;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.playedPreviewSound = false;
    }

    public void setQuality(int quilityIndex)
    {
        QualitySettings.SetQualityLevel(quilityIndex);
    }


}
