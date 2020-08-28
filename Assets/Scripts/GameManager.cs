using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<GameObject> ObjNight; 
    public List<GameObject> ObjSolar;

    public Material SkyboxNight;
    public Material SkyboxFinal;

    public AudioSource IntroSolar;
    public AudioSource IntroNight;
    public AudioSource LoopSolar;
    public AudioSource LoopNight;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        Invoke("PlaySoundLoopSolar", Mathf.Round(IntroSolar.clip.length));
        EnableObjsNight(false);
    }

    void Update()
    {
        
    }

    public void EnableObjsNight(bool status)
    {
        foreach (GameObject g in ObjNight)
            g.SetActive(status);
    }

    public void EnableObjsSolar(bool status)
    {
        foreach (GameObject g in ObjSolar)
            g.SetActive(status);
    }

    public void ChangeSkybox(int idx)
    {
        if (idx == 0)
        {
            RenderSettings.skybox = SkyboxNight;
            LoopSolar.gameObject.SetActive(false);
            IntroNight.gameObject.SetActive(true);
            Invoke("PlaySoundLoopNight", Mathf.Round(IntroNight.clip.length));
        }
        else if(idx == 1)
            RenderSettings.skybox = SkyboxFinal;
    }

    void PlaySoundLoopSolar()
    {
        IntroSolar.gameObject.SetActive(false);
        LoopSolar.gameObject.SetActive(true);
    }

    void PlaySoundLoopNight()
    {
        IntroNight.gameObject.SetActive(false);
        LoopNight.gameObject.SetActive(true);
    }
}
