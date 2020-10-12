﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour {

    public Transform playerTransform;
    public bool useInitialParams = false;
    public Vector3 initialPivotPoint = Vector3.zero;
    public Vector2 initialRotation = new Vector2(35f, 0f);
    public float initialDistanceFromPivot = 15f;
    public float initialHeightOffset = 1.5f;
    public string horizontalRotationInputName = "";
    public string verticalRotationInputName = "";
    public Vector2 rotationInputSpeed = Vector2.one;
    public string zoomInputName = "";
    public float inputZoomMultiplier = 0.5f;
    public float lerpSmoothTime = 1f;
    public float verticalRotationRangeMin = 0f;
    public float verticalRotationRangeMax = 70f;
    public float angleZoomRangeMin = 10f;
    public float angleZoomRangeMax = 20f;
    public float heightOffsetRangeMin = 1f;
    public float heightOffsetRangeMax = 2f;
    public LayerMask raycastMask;
    public float offsetFromCollisionPoint = 0.75f;
    public UnityEvent onTargetingShot;
    public UnityEvent onTargetingPlayer;

    private Vector3 targetShotPivotPoint = Vector3.zero;
    private float targetShotDistance = 0f;
    private Quaternion targetShotRotation = Quaternion.identity;
    private Vector3 curPivotPoint = Vector3.zero;
    private float curHeightOffset = 0f;
    private float curDistanceFromPivot = 0f;
    private Quaternion curRotation = Quaternion.identity;
    private Vector2 targetRotationArroundPlayer = Vector2.zero;
    private Vector2 targetRotationArroundShot = Vector2.zero;
    private float playerToShotTransition = 0f;
    private float playerToShotTransitionSpeed = 0f;

    public float ZoomInput => string.IsNullOrEmpty(zoomInputName) ? 0 : Input.GetAxis(zoomInputName);

    public void Start() {
        Init();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void FixedUpdate() {
        float targetPlayerHeightOffset = Mathf.Lerp(heightOffsetRangeMin, heightOffsetRangeMax, 1f - ZoomInput);
        Vector3 targetPlayerPivotPoint = playerTransform.position;
        Quaternion targetPlayerRotation = playerTransform.rotation * Quaternion.Euler(targetRotationArroundPlayer);
        float targetPlayerDistance = angleZoomRangeMin;

        Vector2 rotationInput = new Vector2(Input.GetAxis(verticalRotationInputName) * rotationInputSpeed.y, Input.GetAxis(horizontalRotationInputName) * rotationInputSpeed.x);
        if (CameraShot.AnySelected) {
            playerToShotTransition = Mathf.SmoothDamp(playerToShotTransition, 1f, ref playerToShotTransitionSpeed, lerpSmoothTime);
            CameraShot curShot = CameraShot.FirstHighestPrioritySelected;
            targetRotationArroundShot += rotationInput * SettingsManager.cameraSensibility;
            targetRotationArroundShot.x = Mathf.Clamp(targetRotationArroundShot.x, -curShot.rotationControlLimits.y / 2f, curShot.rotationControlLimits.y / 2f);
            targetRotationArroundShot.y = Mathf.Clamp(targetRotationArroundShot.y, -curShot.rotationControlLimits.x / 2f, curShot.rotationControlLimits.x / 2f);
            targetShotPivotPoint = curShot.PivotPoint;
            targetShotRotation = curShot.Rotation * Quaternion.Euler(targetRotationArroundShot);
            targetShotDistance = curShot.Distance;
            onTargetingShot.Invoke();
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            playerToShotTransition = Mathf.SmoothDamp(playerToShotTransition, 0f, ref playerToShotTransitionSpeed, lerpSmoothTime);
            targetRotationArroundPlayer += rotationInput * SettingsManager.cameraSensibility;
            targetRotationArroundPlayer.x = Mathf.Clamp(targetRotationArroundPlayer.x, verticalRotationRangeMin, verticalRotationRangeMax);
            targetPlayerRotation = playerTransform.rotation * Quaternion.Euler(targetRotationArroundPlayer);
            targetRotationArroundShot = Vector2.zero;
            onTargetingPlayer.Invoke();
        }


        curPivotPoint = Vector3.Lerp(targetPlayerPivotPoint, targetShotPivotPoint, playerToShotTransition);
        curHeightOffset = Mathf.Lerp(targetPlayerHeightOffset, 0f, playerToShotTransition);
        curRotation = Quaternion.Lerp(targetPlayerRotation, targetShotRotation, playerToShotTransition);

        if (!CameraShot.AnySelected) {
            targetPlayerDistance = Mathf.Lerp(1f, inputZoomMultiplier, ZoomInput) * Mathf.Lerp(angleZoomRangeMin, angleZoomRangeMax, Mathf.InverseLerp(verticalRotationRangeMin, verticalRotationRangeMax, targetPlayerRotation.x));
            Vector3 origin = targetPlayerPivotPoint + Vector3.up * targetPlayerHeightOffset;
            if (Physics.Raycast(origin, targetPlayerRotation * -Vector3.forward, out RaycastHit hit, targetPlayerDistance, raycastMask, QueryTriggerInteraction.Ignore)) {
                targetPlayerDistance = (hit.point - origin).magnitude - offsetFromCollisionPoint;
            }
        }
        curDistanceFromPivot = Mathf.Lerp(targetPlayerDistance, targetShotDistance, playerToShotTransition);

        ApplyTransform();
    }
        
    public void Init() {
        if (useInitialParams) {
            TeleportTo(initialPivotPoint, Quaternion.Euler(initialRotation), initialDistanceFromPivot, initialHeightOffset);
        }
        else {
            TeleportToPlayer();
        }
    }
    public void TeleportToPlayer() {
        float newDistFromPivot = Mathf.Lerp(angleZoomRangeMin, angleZoomRangeMax, Mathf.InverseLerp(verticalRotationRangeMin, verticalRotationRangeMax, curRotation.x));
        Vector3 origin = playerTransform.position + Vector3.up * heightOffsetRangeMin;
        if (Physics.Raycast(origin, -playerTransform.forward, out RaycastHit hit, newDistFromPivot, raycastMask)) {
            newDistFromPivot = (hit.point - origin).magnitude - offsetFromCollisionPoint;
        }
        TeleportTo(playerTransform.position, playerTransform.rotation, newDistFromPivot, heightOffsetRangeMin);
    }
    public void TeleportTo(Vector3 newPivotPoint, Quaternion newRotation, float newDistFromPivot, float newHeightOffset) {
        curPivotPoint = newPivotPoint;
        curRotation = newRotation;
        curDistanceFromPivot = newDistFromPivot;
        curHeightOffset = newHeightOffset;
        ApplyTransform();
    }

    private void ApplyTransform() {
        transform.position = curPivotPoint + (curRotation * Vector3.up * curHeightOffset) - (curRotation * Vector3.forward * curDistanceFromPivot);
        transform.rotation = curRotation;
    }

}

