using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class ChangeSettings : MonoBehaviour
{
    public Toggle FullScreenToggle, VSyncToggle;
    public List<ResItem> resolutions = new List<ResItem>();
    private int SelectedResolution;
    public TMP_Text resolutionLabel, masterLabel, musicLabel, sfxLabel, ambientLabel;
    public AudioMixer mixer;
    public Slider masterSlider, musicSlider, sfxSlider, ambientSlider;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

[System.Serializable]
public class ResItem
{
    public int wide, tall;
}