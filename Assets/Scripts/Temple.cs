using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour
{
    public enum TempleType { Solar, Night };
    public TempleType TypeTemple;

    public int TotalCountResources;
    public int CurrentResources;
    public List<GameObject> Crystal;
    void Start()
    {

    }

    void Update()
    {

    }

    public void UptadeCountResources()
    {
        CurrentResources = transform.childCount;
        Crystal[CurrentResources - 1].SetActive(true);

        if (CurrentResources == TotalCountResources)
        {
            if (TypeTemple == TempleType.Solar)
            {
                foreach (GameObject c in Crystal)
                    c.tag = "SolarCrystal";
            }
            if (TypeTemple == TempleType.Night)
            {
                foreach (GameObject c in Crystal)
                    c.tag = "NightCrystal";
            }
        }
    }
}
