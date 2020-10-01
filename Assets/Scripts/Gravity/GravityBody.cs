using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour {

    public static List<GravityBody> instanceList = new List<GravityBody>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }

    public Rigidbody rigidbodyRef;
    private void Start() {
        if (rigidbodyRef == null) {
            rigidbodyRef = GetComponent<Rigidbody>();
        }
    }
    
}
