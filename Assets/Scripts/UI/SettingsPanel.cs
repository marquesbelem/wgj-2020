using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider cameraSensibilitySlider;

    private void OnEnable() {
        masterVolumeSlider.value = SettingsManager.masterVolume;
        musicVolumeSlider.value = SettingsManager.musicVolume;
        sfxVolumeSlider.value = SettingsManager.sfxVolume;
    }
    private void OnDisable() {
        SettingsManager.SaveValues();
    }

    public void SetNewMasterVolume(float value) {
        SettingsManager.masterVolume = value;
        SettingsManager.ApplyVolumes();
    }
    public void SetNewMusicVolume(float value) {
        SettingsManager.musicVolume = value;
        SettingsManager.ApplyVolumes();
    }
    public void SetNewSFXVolume(float value) {
        SettingsManager.sfxVolume = value;
        SettingsManager.ApplyVolumes();
    }
    public void SetNewCameraSensibility(float value) {
        SettingsManager.cameraSensibility = value;
    }

}
