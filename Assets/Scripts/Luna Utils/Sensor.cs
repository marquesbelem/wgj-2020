using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour {

    public List<Collider> collidersInside;

    public bool chackTag;
    public string targetTag;

    public UnityEvent onEnter;
    public UnityEvent onExit;

    private void OnTriggerEnter(Collider other) {
        if (!chackTag || other.CompareTag(targetTag)) {
            collidersInside.Add(other);
            onEnter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (collidersInside.Contains(other)) {
            collidersInside.Remove(other);
            onExit.Invoke();
        }
    }

}
