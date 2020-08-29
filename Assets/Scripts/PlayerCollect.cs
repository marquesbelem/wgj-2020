using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollect : MonoBehaviour
{
    public enum Player { Solar, Night };
    public Player PlayerType;

    private int _TotalCountResourcesSolar;
    private int _TotalCountResourcesNight;

    private int _CurrentCountResourcesSolar = 0;
    private int _CurrentCountResourcesNight = 0;
    void OnTriggerEnter(Collider col)
    {
        if (PlayerType == Player.Solar)
        {
            if (col.gameObject.CompareTag("SolarCrystal"))
            {
                StartCoroutine(ActionSolar());
                Destroy(col.gameObject);
            }
        }

        if (PlayerType == Player.Night)
        {
            if (col.gameObject.CompareTag("NightCrystal"))
            {
                StartCoroutine(ActionNight());
                Destroy(col.gameObject);
            }
        }
    }

    IEnumerator ActionSolar()
    {
        var temple = GameObject.FindGameObjectWithTag("TempleSolar").GetComponent<Temple>();
        _TotalCountResourcesSolar = temple.TotalCountResources;

        _CurrentCountResourcesSolar++;

        if(_CurrentCountResourcesSolar>= _TotalCountResourcesSolar)
        {
            GameManager.Instance.ChangeSkybox(0);
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.EnableObjsNight(true);
            GameManager.Instance.EnableObjsSolar(false);
        }
    }
    IEnumerator ActionNight()
    {
        var temple = GameObject.FindGameObjectWithTag("TempleNight").GetComponent<Temple>();
        _TotalCountResourcesNight = temple.TotalCountResources;

        _CurrentCountResourcesNight++;

        if (_CurrentCountResourcesNight >= _TotalCountResourcesNight)
        {
            //Encontro das players
            GameManager.Instance.ChangeSkybox(1);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("GameEncontro");
        }
    }
}
