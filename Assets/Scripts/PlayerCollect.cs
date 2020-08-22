using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public enum Player { Solar, Night };
    public Player PlayerType;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (PlayerType == Player.Solar)
        {
            if (col.gameObject.CompareTag("SolarCrystal"))
            {
                Debug.Log("Cristal Solar");
                Destroy(col.gameObject);
            }
        }

        if (PlayerType == Player.Night)
        {
            if (col.gameObject.CompareTag("NightCrystal"))
            {
                Debug.Log("Noturno");
                Destroy(col.gameObject);
            }
        }
    }

}
