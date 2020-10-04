using GalloUtils;
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

    public CameraController cameraController;
    public Transform solarCharacter;
    public Transform nightCharacter;

    public List<GameObject> activeAtNight;
    public List<GameObject> activeAtSolar;

    private DayTime curDayTime = DayTime.Night;

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
        switch (curDayTime) {
            case DayTime.Solar:
                activeAtSolar.ForEach(o => o.SetActive(true));
                activeAtNight.ForEach(o => o.SetActive(false));
                FindObjectsOfType<DayTimeEvents>().ForEach(i => i.onSolar.Invoke());
                RenderSettings.skybox = skyboxDay;
                introNight.Stop();
                loopNight.Stop();
                introSolar.Play();
                CancelInvoke("PlaySoundLoopNight");
                Invoke("PlaySoundLoopSolar", introSolar.clip.length);
                cameraController.playerTransform = solarCharacter;
                break;
            case DayTime.Night:
                activeAtSolar.ForEach(o => o.SetActive(false));
                activeAtNight.ForEach(o => o.SetActive(true));
                FindObjectsOfType<DayTimeEvents>().ForEach(i => i.onNight.Invoke());
                RenderSettings.skybox = skyboxNight;
                introSolar.Stop();
                loopSolar.Stop();
                introNight.Play();
                CancelInvoke("PlaySoundLoopSolar");
                Invoke("PlaySoundLoopNight", introNight.clip.length);
                cameraController.playerTransform = nightCharacter;
                break;
            case DayTime.Twilight:
                FindObjectsOfType<DayTimeEvents>().ForEach(i => i.onTwilight.Invoke());
                RenderSettings.skybox = skyboxTwilight;
                break;
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