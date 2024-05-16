using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class ChangeSettings : MonoBehaviour
{
    public Toggle FullScreenToggle, VSyncToggle;
    public List<ResItem> resolutions = new List<ResItem>();
    public List<AdvancedItem> advancedLevels = new List<AdvancedItem>();
    private List<string> levels = new List<string>{"Low","Medium","High","Ultra"};
    private int SelectedResolution;
    public TMP_Text resolutionLabel, masterLabel, musicLabel, sfxLabel, ambientLabel, qualityLabel, bloomLabel, aliasingLabel, processingLabel, brightnessLabel;
    public AudioMixer mixer;
    public Slider masterSlider, musicSlider, sfxSlider, ambientSlider, brightnessSlider;
    public Volume defaultVolume, UIVolume;
    private Bloom bloom;
    private ColorAdjustments colorAdjustments;
    private List<TMP_Text> advancedLabels;
    private float[] values;

    // Start is called before the first frame update
    void Start()
    {
        FullScreenToggle.isOn = Screen.fullScreen;
        VSyncToggle.isOn = (QualitySettings.vSyncCount != 0);

        for(int i = 0; i < resolutions.Count; i++) {
            if(Screen.width == resolutions[i].wide && Screen.height == resolutions[i].tall) {
                SelectedResolution = i;
                UpdateResLabel();
            }
        }

        float volume = 0f;
        mixer.GetFloat("MasterVol", out volume);
        masterSlider.value = Mathf.RoundToInt(volume * 2) + 100;
        mixer.GetFloat("MusicVol", out volume);
        musicSlider.value = Mathf.RoundToInt(volume * 2) + 100;
        mixer.GetFloat("SFXVol", out volume);
        sfxSlider.value = Mathf.RoundToInt(volume * 2) + 100;
        mixer.GetFloat("AmbientVol", out volume);
        ambientSlider.value = Mathf.RoundToInt(volume * 2) + 100;

        masterLabel.text = masterSlider.value.ToString();
        musicLabel.text = musicSlider.value.ToString();
        sfxLabel.text = sfxSlider.value.ToString();
        ambientLabel.text = ambientSlider.value.ToString();

        advancedLabels = new List<TMP_Text>{qualityLabel, bloomLabel, aliasingLabel, processingLabel};

        // read and set brightness from playerprefs
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness",50);
        SetBrightness();
        
        advancedLevels[0].level = QualitySettings.GetQualityLevel();
        qualityLabel.text = levels[advancedLevels[0].level];

        values = new float[] {1.0f, 4.0f, 6.0f, 10.0f};
        float selectedBloom = PlayerPrefs.GetFloat("Bloom",6.0f);
        defaultVolume.profile.TryGet(out bloom);
        {
            bloom.intensity.value = selectedBloom;
        }
        advancedLevels[1].level = Array.IndexOf(values,selectedBloom);
        bloomLabel.text = levels[advancedLevels[1].level];

        values = new float[] {0.0f, 2.0f, 4.0f, 8.0f};
        advancedLevels[2].level = Array.IndexOf(values,(float)QualitySettings.antiAliasing);
        aliasingLabel.text = levels[advancedLevels[2].level];

        values = new float[] {0.2f, 0.5f, 0.8f, 1.0f};
        defaultVolume.weight = PlayerPrefs.GetFloat("Weight",0.8f);
        advancedLevels[3].level = Array.IndexOf(values,defaultVolume.weight);
        processingLabel.text = levels[advancedLevels[3].level];
    }

    public void ResLeft() 
    {
        SelectedResolution--;
        if (SelectedResolution < 0)
        {
            SelectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight() 
    {
        SelectedResolution++;
        if (SelectedResolution > resolutions.Count - 1)
        {
            SelectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel() 
    {
        resolutionLabel.text = resolutions[SelectedResolution].wide.ToString() + " x " + resolutions[SelectedResolution].tall.ToString();
    }

    public void LevelLeft(TMP_Text label) 
    {
        AdvancedItem SelectedVar = advancedLevels[advancedLabels.IndexOf(label)];
        SelectedVar.level--;
        if (SelectedVar.level < 0)
        {
            SelectedVar.level = 0;
        }
        label.text = levels[SelectedVar.level];
    }

    public void LevelRight(TMP_Text label) 
    {
        AdvancedItem SelectedVar = advancedLevels[advancedLabels.IndexOf(label)];
        SelectedVar.level++;
        if (SelectedVar.level > levels.Count - 1)
        {
            SelectedVar.level = levels.Count - 1;
        }
        label.text = levels[SelectedVar.level];
    }

    public void ApplyGraphicsChanges()
    {
        if (VSyncToggle.isOn) 
        {
            QualitySettings.vSyncCount = 1;
        } else 
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[SelectedResolution].wide, resolutions[SelectedResolution].tall, FullScreenToggle.isOn);
    }

    public void ApplyAdvancedChanges()
    {
        values = new float[] {1.0f, 4.0f, 6.0f, 10.0f};
        defaultVolume.profile.TryGet(out bloom);
        {
            bloom.intensity.value = values[levels.IndexOf(bloomLabel.text)];
            PlayerPrefs.SetFloat("Bloom", bloom.intensity.value);
        }

        values = new float[] {0.2f, 0.5f, 0.8f, 1.0f};
        defaultVolume.weight = values[levels.IndexOf(processingLabel.text)];
        PlayerPrefs.SetFloat("Weight", defaultVolume.weight);

        QualitySettings.SetQualityLevel(levels.IndexOf(qualityLabel.text));

        values = new float[] {0.0f, 2.0f, 4.0f, 8.0f};
        QualitySettings.antiAliasing = (int)values[levels.IndexOf(aliasingLabel.text)];
    }

    public void SetMasterVolume()
    {
        masterLabel.text = masterSlider.value.ToString();
        mixer.SetFloat("MasterVol", (float)0.5*(masterSlider.value-100));
        PlayerPrefs.SetFloat("MasterVol", (float)0.5*(masterSlider.value-100));
    }

    public void SetMusicVolume()
    {
        musicLabel.text = musicSlider.value.ToString();
        mixer.SetFloat("MusicVol", (float)0.5*(musicSlider.value-100));
        PlayerPrefs.SetFloat("MusicVol", (float)0.5*(musicSlider.value-100));
    }

    public void SetSFXVolume()
    {
        sfxLabel.text = sfxSlider.value.ToString();
        mixer.SetFloat("SFXVol", (float)0.5*(sfxSlider.value-100));
        PlayerPrefs.SetFloat("SFXVol", (float)0.5*(sfxSlider.value-100));
    }

    public void SetAmbientVolume()
    {
        ambientLabel.text = ambientSlider.value.ToString();
        mixer.SetFloat("AmbientVol", (float)0.5*(ambientSlider.value-100));
        PlayerPrefs.SetFloat("AmbientVol", (float)0.5*(ambientSlider.value-100));
    }

    public void SetBrightness()
    {
        brightnessLabel.text = brightnessSlider.value.ToString();
        UIVolume.profile.TryGet(out colorAdjustments);
        {
            colorAdjustments.postExposure.value = 0.1f+(float)((brightnessSlider.value-50)/25);
        }
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
    }
}

[System.Serializable]
public class ResItem
{
    public int wide, tall;
}

[System.Serializable]
public class AdvancedItem
{
    public int level;
}