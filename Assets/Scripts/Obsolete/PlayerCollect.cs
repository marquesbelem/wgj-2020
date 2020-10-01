using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollect : MonoBehaviour {
    public DayTime PlayerType;

    private int _TotalCountResourcesSolar;
    private int _TotalCountResourcesNight;

    private int _CurrentCountResourcesSolar = 0;
    private int _CurrentCountResourcesNight = 0;
    void OnTriggerEnter(Collider col) {
        if (PlayerType == DayTime.Solar) {
            if (col.gameObject.CompareTag("SolarCrystal")) {
                StartCoroutine(ActionSolar());
                Destroy(col.gameObject);
            }
        }

        if (PlayerType == DayTime.Night) {
            if (col.gameObject.CompareTag("NightCrystal")) {
                StartCoroutine(ActionNight());
                Destroy(col.gameObject);
            }
        }
    }

    IEnumerator ActionSolar() {
        var temple = GameObject.FindGameObjectWithTag("TempleSolar").GetComponent<DeliverSpot>();
        //_TotalCountResourcesSolar = temple.totalCountResources;

        _CurrentCountResourcesSolar++;

        if (_CurrentCountResourcesSolar >= _TotalCountResourcesSolar) {
            //DayTimeManager.instance.ChangeSkybox(0);
            yield return new WaitForSeconds(0.5f);
            //DayTimeManager.instance.EnableObjsNight(true);
            //DayTimeManager.instance.EnableObjsSolar(false);
        }
    }
    IEnumerator ActionNight() {
        var temple = GameObject.FindGameObjectWithTag("TempleNight").GetComponent<DeliverSpot>();
        //_TotalCountResourcesNight = temple.totalCountResources;

        _CurrentCountResourcesNight++;

        if (_CurrentCountResourcesNight >= _TotalCountResourcesNight) {
            //Encontro das players
            //DayTimeManager.instance.ChangeSkybox(1);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("GameEncontro");
        }
    }
}
