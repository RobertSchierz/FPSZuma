using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public class SettingsMenu : MonoBehaviour {

    private bool playedPreviewSound = false;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionsDropdown;

    void Start()
    {
        this.resolutions = Screen.resolutions;
        this.resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentIndex = 0;

        for (int i = 0; i < this.resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }

            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentIndex;
        resolutionsDropdown.RefreshShownValue();
    }

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

    public void setFullscrean(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


}
