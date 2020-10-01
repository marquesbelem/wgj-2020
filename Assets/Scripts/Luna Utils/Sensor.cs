using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public List<Collider> collidersInside;

    public bool chackTag;
    public string targetTag;

    private void OnTriggerEnter(Collider other) {
        if (!chackTag || other.CompareTag(targetTag)) {
            collidersInside.Add(other);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (collidersInside.Contains(other)) {
            collidersInside.Remove(other);
        }
    }

}
