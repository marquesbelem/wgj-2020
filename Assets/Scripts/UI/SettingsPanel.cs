using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider cameraSensibilitySlider;
    public Dropdown languageDropdown;

    private void OnEnable() {
        if (masterVolumeSlider) {
            masterVolumeSlider.value = SettingsManager.masterVolume;
            masterVolumeSlider.onValueChanged.AddListener(SetNewMasterVolume);
        }
        if (musicVolumeSlider) {
            musicVolumeSlider.value = SettingsManager.musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(SetNewMusicVolume);
        }
        if (sfxVolumeSlider) {
            sfxVolumeSlider.value = SettingsManager.sfxVolume;
            sfxVolumeSlider.onValueChanged.AddListener(SetNewSFXVolume);
        }
        if (cameraSensibilitySlider) {
            cameraSensibilitySlider.value = SettingsManager.CameraSensibility;
            cameraSensibilitySlider.onValueChanged.AddListener(SetNewCameraSensibility);
        }
        if (languageDropdown) {
            languageDropdown.options = MultiLanguageManager.loadedLanguages.ConvertAll(l => new Dropdown.OptionData(l.languageID, l.flag));
            languageDropdown.value = MultiLanguageManager.SelectedLanguageIndex;
            languageDropdown.onValueChanged.AddListener(SetNewLanguageIndex);
        }
    }
    private void OnDisable() {
        if (masterVolumeSlider) {
            masterVolumeSlider.onValueChanged.RemoveListener(SetNewMasterVolume);
        }
        if (musicVolumeSlider) {
            musicVolumeSlider.onValueChanged.RemoveListener(SetNewMusicVolume);
        }
        if (sfxVolumeSlider) {
            sfxVolumeSlider.onValueChanged.RemoveListener(SetNewSFXVolume);
        }
        if (cameraSensibilitySlider) {
            cameraSensibilitySlider.onValueChanged.RemoveListener(SetNewCameraSensibility);
        }
        if (languageDropdown) {
            languageDropdown.onValueChanged.RemoveListener(SetNewLanguageIndex);
        }
        SettingsManager.SaveValues();
    }

    private void SetNewMasterVolume(float value) {
        SettingsManager.masterVolume = value;
        SettingsManager.ApplyVolumes();
    }
    private void SetNewMusicVolume(float value) {
        SettingsManager.musicVolume = value;
        SettingsManager.ApplyVolumes();
    }
    private void SetNewSFXVolume(float value) {
        SettingsManager.sfxVolume = value;
        SettingsManager.ApplyVolumes();
    }
    private void SetNewCameraSensibility(float value) {
        SettingsManager.CameraSensibility = value;
    }
    private void SetNewLanguageIndex(int value) {
        MultiLanguageManager.SelectedLanguageIndex = value;
    }

}
