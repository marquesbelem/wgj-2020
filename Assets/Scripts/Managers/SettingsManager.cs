using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour {

    public static float masterVolume = 0f;
    public static float musicVolume = 0f;
    public static float sfxVolume = 0f;
    public static float CameraSensibility {
        get => CameraController.sensibility;
        set => CameraController.sensibility = value;
    }
    public static AudioMixer mixer;
    public static string masterVolumeParameterName;
    public static string musicVolumeParameterName;
    public static string sfxVolumeParameterName;

    public AudioMixer audioMixer;
    public string masterVolumeName;
    public string musicVolumeName;
    public string sfxVolumeName;

    private void Start() {
        BecomeStatic();
        LoadValues();
        ApplyVolumes();
    }
    public void BecomeStatic() {
        mixer = audioMixer;
        masterVolumeParameterName = masterVolumeName;
        musicVolumeParameterName = musicVolumeName;
        sfxVolumeParameterName = sfxVolumeName;
    }

    public static void LoadValues() {
        masterVolume = PlayerPrefs.GetFloat(masterVolumeParameterName, masterVolume);
        musicVolume = PlayerPrefs.GetFloat(musicVolumeParameterName, musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(sfxVolumeParameterName, sfxVolume);
        CameraSensibility = PlayerPrefs.GetFloat("CameraSensibility", CameraSensibility);
    }
    public static void SaveValues() {
        PlayerPrefs.SetFloat(masterVolumeParameterName, masterVolume);
        PlayerPrefs.SetFloat(musicVolumeParameterName, musicVolume);
        PlayerPrefs.SetFloat(sfxVolumeParameterName, sfxVolume);
        PlayerPrefs.SetFloat("CameraSensibility", CameraSensibility);
        PlayerPrefs.Save();
    }
    public static void ApplyVolumes() {
        mixer.SetFloat(masterVolumeParameterName, masterVolume);
        mixer.SetFloat(musicVolumeParameterName, musicVolume);
        mixer.SetFloat(sfxVolumeParameterName, sfxVolume);
    }

}
