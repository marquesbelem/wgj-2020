using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour {

    public static bool allowInput = true;
    public static float sensibility = 1f;
    public static bool invertYAxis = false;

    public string horizontalRotationInputName = "Mouse X";
    public string verticalRotationInputName = "Mouse Y";
    public string actionInputName = ""; //Optional zoom/offset input

    public Vector2 rotationInputSpeed = Vector2.one;
    public float transitionSmoothTime = 0.5f;
    public float inputSmoothTime = 0.1f;
    public UnityEvent onTargetingNewShot;
    public UnityEvent onTargetingNoShot;


    private Vector3 curPivot = Vector3.zero;
    private Quaternion curRotation = Quaternion.identity;
    private float curDistance = 0f;
    private Vector2 curControlledRotation = Vector2.zero;
    private CameraShot nextCameraShot = null;

    private Vector3 lastPivot = Vector3.zero;
    private Quaternion lastRotation = Quaternion.identity;
    private float lastDistance = 0f;
    private Vector2 lastControlledRotation = Vector2.zero;
    private CameraShot lastCameraShot = null;

    private float transitionPoint = 0f;
    private float transitionSpeed = 0f;
    private Vector2 rotationInputSmoothPoint = Vector2.zero;
    private Vector2 rotationInputSmoothSpeed = Vector2.zero;

    public float ActionInput => (string.IsNullOrEmpty(actionInputName) || !allowInput) ? 0f : Input.GetAxis(actionInputName);
    private float TresholdToAcceptNextTarget => transitionSmoothTime / 10f;

    public void Start() {
        Init();
    }
    public void Update() {
        if (transitionPoint > 1f - TresholdToAcceptNextTarget) {
            CameraShot selectedShot = CameraShot.FirstHighestPrioritySelected;
            if (nextCameraShot != selectedShot) {
                lastCameraShot = nextCameraShot;
                lastPivot = curPivot;
                lastRotation = curRotation;
                lastDistance = curDistance;
                lastControlledRotation = curControlledRotation;
                nextCameraShot = selectedShot;
                transitionPoint = 0f;
                curControlledRotation = Vector2.zero;
                rotationInputSmoothPoint = Vector2.zero;
                rotationInputSmoothSpeed = Vector2.zero;
                if (lastCameraShot != null) {
                    lastCameraShot.onEndTargeted.Invoke();
                }
                if (nextCameraShot != null) {
                    onTargetingNewShot.Invoke();
                    nextCameraShot.onBeginTargeted.Invoke();
                }
                else {
                    onTargetingNoShot.Invoke();
                }
            }
        }

        Vector3 nextPivot = curPivot;
        Quaternion nextRotation = curRotation;
        float nextDistance = curDistance;
        if (nextCameraShot != null) {
            if (allowInput) {
                Vector2 rotationInput = new Vector2(Input.GetAxis(verticalRotationInputName) * rotationInputSpeed.y, Input.GetAxis(horizontalRotationInputName) * rotationInputSpeed.x) * nextCameraShot.controlledRotationSpeedMultiplier;
                if (invertYAxis) rotationInput.y *= -1f;
                rotationInputSmoothPoint = Vector2.SmoothDamp(rotationInputSmoothPoint, rotationInput, ref rotationInputSmoothSpeed, inputSmoothTime);
                curControlledRotation += rotationInputSmoothPoint * sensibility;
            }
            nextCameraShot.ClampControlledRotation(ref curControlledRotation);
            nextPivot = nextCameraShot.GetCurrentPivot(ActionInput);
            nextRotation = nextCameraShot.GetCurrentRotation(curControlledRotation);
            nextDistance = nextCameraShot.GetCurrentDistance(ActionInput, curControlledRotation);
        }
        if (lastCameraShot != null) {
            lastPivot = lastCameraShot.GetCurrentPivot(ActionInput);
            lastRotation = lastCameraShot.GetCurrentRotation(lastControlledRotation);
            lastDistance = lastCameraShot.GetCurrentDistance(ActionInput, lastControlledRotation);
        }
        transitionPoint = Mathf.SmoothDamp(transitionPoint, 1f, ref transitionSpeed, transitionSmoothTime);
        curPivot = Vector3.Slerp(lastPivot, nextPivot, transitionPoint);
        curRotation = Quaternion.Lerp(lastRotation, nextRotation, transitionPoint);
        curDistance = Mathf.Lerp(lastDistance, nextDistance, transitionPoint);
        ApplyTransform();
    }
    
    public void Init() {
        curPivot = transform.position;
        curRotation = transform.rotation;
        lastPivot = curPivot;
        lastRotation = curRotation;
        transitionPoint = 1f;
    }
    public void TeleportTo(Vector3 newPivotPoint, Quaternion newRotation, float newDistFromPivot) {
        curPivot = newPivotPoint;
        curRotation = newRotation;
        curDistance = newDistFromPivot;
        ApplyTransform();
    }

    private void ApplyTransform() {
        transform.position = curPivot - (curRotation * Vector3.forward * curDistance);
        transform.rotation = curRotation;
    }

}

