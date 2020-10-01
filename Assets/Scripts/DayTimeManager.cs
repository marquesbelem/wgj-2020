using GalloUtils;
using SD;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeManager : MonoBehaviour {

    public static DayTimeManager instance = null;
    private void Awake() {
        instance = this;
    }
    
    public Material skyboxDay;
    public Material skyboxNight;
    public Material skyboxTwilight;

    public AudioSource introSolar;
    public AudioSource introNight;
    public AudioSource loopSolar;
    public AudioSource loopNight;
    
    public string solarOnlyType = "SolarOnly";
    public string nightOnlyType = "NightOnly";
    public string twilightOnlyType = "TwilightOnly";

    public DayTime curDayTime = DayTime.Solar;

    public CameraController cameraController;
    public Transform solarCharacter;
    public Transform nightCharacter;

    void Start() {
        UpdateDayTime();
    }

    public void ToggleDayTime() {
        if (curDayTime == DayTime.Solar) {
            curDayTime = DayTime.Night;
        }
        else if (curDayTime == DayTime.Night) {
            curDayTime = DayTime.Solar;
        }
        UpdateDayTime();
    }
    public void SetTwilight() {
        curDayTime = DayTime.Twilight;
        UpdateDayTime();
    }

    private void UpdateDayTime() {
        UpdateGlobalInstances();
        UpdateSkybox();
        UpdateAudios();
        UpdateCameraTarget();
    }
    private void UpdateGlobalInstances() {
        foreach (GlobalInstance g in GlobalInstance.GetInstanceListOfType(nightOnlyType)) {
            g.gameObject.SetActive(curDayTime == DayTime.Night);
        }
        foreach (GlobalInstance g in GlobalInstance.GetInstanceListOfType(solarOnlyType)) {
            g.gameObject.SetActive(curDayTime == DayTime.Solar);
        }
        foreach (GlobalInstance g in GlobalInstance.GetInstanceListOfType(twilightOnlyType)) {
            g.gameObject.SetActive(curDayTime == DayTime.Twilight);
        }
    }
    private void UpdateSkybox() {
        switch (curDayTime) {
            case DayTime.Solar:
                RenderSettings.skybox = skyboxDay;
                break;
            case DayTime.Night:
                RenderSettings.skybox = skyboxNight;
                break;
            case DayTime.Twilight:
                RenderSettings.skybox = skyboxTwilight;
                break;
            default:
                break;
        }
    }
    private void UpdateAudios() {
        switch (curDayTime) {
            case DayTime.Solar:
                loopNight.Stop();
                introSolar.Play();
                Invoke("PlaySoundLoopSolar", introSolar.clip.length);
                break;
            case DayTime.Night:
                loopSolar.Stop();
                introNight.Play();
                Invoke("PlaySoundLoopNight", introNight.clip.length);
                break;
            case DayTime.Twilight:
                break;
            default:
                break;
        }
    }
    private void UpdateCameraTarget() {
        if (curDayTime == DayTime.Night) {
            cameraController.playerTransform = nightCharacter;
        }
        else if (curDayTime == DayTime.Solar) {
            cameraController.playerTransform = solarCharacter;
        }
    }

    private void PlaySoundLoopSolar() {
        introSolar.Stop();
        loopSolar.Play();
    }
    private void PlaySoundLoopNight() {
        introNight.Stop();
        loopNight.Play();
    }

}

public enum DayTime { Solar, Night, Twilight };