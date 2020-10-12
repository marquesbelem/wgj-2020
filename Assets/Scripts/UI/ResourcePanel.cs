using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour {

    public static ResourcePanel instance;
    private void Awake() {
        instance = this;
    }

    public static readonly int resourceCount = 3;

    public GameObject panelRoot;
    public Text[] textRenderers;

    private void Update() {
        PlayerInteractor collector = PlayerInteractor.FirstEnabled;
        DeliverSpot deliverSpot = DeliverSpot.FirstEnabled;
        if (collector != null && deliverSpot != null) {
            for (int i = 0; i < resourceCount; i++) {
                textRenderers[i].text = collector.CountResourcesOfType(i) + deliverSpot.CollectedQuantityOfType(i) + "/" + deliverSpot.WishedQuantityOfType(i);
            }
            panelRoot.SetActive(true);
        }
        else {
            panelRoot.SetActive(false);
        }
    }

}
