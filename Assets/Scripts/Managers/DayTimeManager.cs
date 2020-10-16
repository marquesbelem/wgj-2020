using System.Collections;
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
                RenderSettings.skybox = skyboxDay;
                introNight.Stop();
                loopNight.Stop();
                introSolar.Play();
                StopAllCoroutines();
                StartCoroutine(PlaySoundLoopSolar());
                break;
            case DayTime.Night:
                activeAtSolar.ForEach(o => o.SetActive(false));
                activeAtNight.ForEach(o => o.SetActive(true));
                RenderSettings.skybox = skyboxNight;
                introSolar.Stop();
                loopSolar.Stop();
                introNight.Play();
                StopAllCoroutines();
                StartCoroutine(PlaySoundLoopNight());
                break;
            case DayTime.Twilight:
                RenderSettings.skybox = skyboxTwilight;
                break;
        }
    }

    private IEnumerator PlaySoundLoopSolar() {
        yield return new WaitForSeconds(introSolar.clip.length);
        introSolar.Stop();
        loopSolar.Play();
    }
    private IEnumerator PlaySoundLoopNight() {
        yield return new WaitForSeconds(introNight.clip.length);
        introNight.Stop();
        loopNight.Play();
    }

}

public enum DayTime { Solar, Night, Twilight };