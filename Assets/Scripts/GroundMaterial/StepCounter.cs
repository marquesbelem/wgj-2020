using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepCounter : MonoBehaviour {

    public float stepDistance = 0.8f;
    public UnityEvent onStep;

    private Vector3 lastPos;
    private float stepCounter = 0f;

    private float DistanceTraveledLastFrame => (transform.position - lastPos).magnitude;

    private void Start() {
        lastPos = transform.position;
    }
    void Update() {
        stepCounter += DistanceTraveledLastFrame;
        if (stepCounter >= stepDistance) {
            stepCounter -= stepDistance;
            onStep.Invoke();
        }
        lastPos = transform.position;
    }

}
