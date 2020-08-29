using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour
{
    public enum TempleType { Solar, Night };
    public TempleType TypeTemple;

    public int TotalCountResources;
    public int CurrentResources;
    public List<GameObject> Crystal;
   
    public void UptadeCountResources()
    {
        if (CurrentResources >TotalCountResources)
            return;

        CurrentResources++;
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
