using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SphereCoordinates : MonoBehaviour {

    public float radius = 1f;

    public void ApplyToTransform() {
        transform.position = transform.up * radius;
    }
    public void GetFromTransform() {
        radius = transform.position.magnitude;
    }

    private void OnEnable() {
        GetFromTransform();
    }
    private void Update() {
        if (!Application.isPlaying) {
            ApplyToTransform();
        }
    }
    private void OnValidate() {
        ApplyToTransform();
    }

}
