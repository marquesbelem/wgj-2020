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
        masterVolumeSlider.value = SettingsManager.masterVolume;
        masterVolumeSlider.onValueChanged.AddListener(SetNewMasterVolume);
        
        musicVolumeSlider.value = SettingsManager.musicVolume;
        musicVolumeSlider.onValueChanged.AddListener(SetNewMusicVolume);
        
        sfxVolumeSlider.value = SettingsManager.sfxVolume;
        sfxVolumeSlider.onValueChanged.AddListener(SetNewSFXVolume);

        cameraSensibilitySlider.value = SettingsManager.cameraSensibility;
        cameraSensibilitySlider.onValueChanged.AddListener(SetNewCameraSensibility);

        languageDropdown.options = MultiLanguageManager.loadedLanguages.ConvertAll(l => new Dropdown.OptionData(l.languageID));
        languageDropdown.value = MultiLanguageManager.SelectedLanguageIndex;
        languageDropdown.onValueChanged.AddListener(SetNewLanguageIndex);
    }
    private void OnDisable() {
        masterVolumeSlider.onValueChanged.RemoveListener(SetNewMasterVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(SetNewMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(SetNewSFXVolume);
        cameraSensibilitySlider.onValueChanged.RemoveListener(SetNewCameraSensibility);
        languageDropdown.onValueChanged.RemoveListener(SetNewLanguageIndex);
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
        SettingsManager.cameraSensibility = value;
    }
    private void SetNewLanguageIndex(int value) {
        MultiLanguageManager.SelectedLanguageIndex = value;
    }

}
