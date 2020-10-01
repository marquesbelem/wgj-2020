﻿using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static bool allowed = true;

    public Rigidbody rigidbodyRef;
    public float movementSpeed = 1f;
    public string walkForwardInputName = "Vertical";
    public string walkRightInputName = "Horizontal";
    public float walkInputTreshold = 0.1f;

    public Animator animatorRef;
    public Transform rotatingTransform;
    public string walkAnimationName = "Walking";
    public float walkAnimationTreshold = 0.1f;

    public AudioSource walkSound;
    public float walkSoundDelay = 0.8f;

    private float walkSoundTimer = 0f;
    private float walkAnimationSqrTreshold;
    private Quaternion movementLocalRot;
    private Vector3 movInputDir;

    private void Start() {
        animatorRef = GetComponent<Animator>();
        walkAnimationSqrTreshold = walkAnimationTreshold * walkAnimationTreshold;
    }

    private void Update() {
        if (allowed) {
            Vector3 cameraForwardProjected = transform.InverseTransformDirection(Camera.main.transform.forward);
            cameraForwardProjected.y = 0f;
            movementLocalRot = Quaternion.LookRotation(transform.TransformDirection(cameraForwardProjected.normalized), transform.up);
            movInputDir = movementLocalRot * (new Vector3(Input.GetAxis(walkRightInputName), 0f, Input.GetAxis(walkForwardInputName))).normalized;
            bool inputPassesTreshold = movInputDir.sqrMagnitude >= walkAnimationSqrTreshold;
            bool velocityPassesTreshold = rigidbodyRef.velocity.sqrMagnitude >= walkAnimationSqrTreshold;
            if (velocityPassesTreshold) {
                if (walkSound != null) {
                    walkSoundTimer += Time.deltaTime;
                    if (walkSoundTimer >= walkSoundDelay) {
                        walkSoundTimer -= walkSoundDelay;
                        walkSound.Play();
                    }
                }
                if (inputPassesTreshold) {
                    rotatingTransform.rotation = Quaternion.LookRotation(movInputDir.normalized, transform.up);
                }
            }
            animatorRef.SetBool(walkAnimationName, velocityPassesTreshold);
            rigidbodyRef.velocity = movInputDir * movementSpeed;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (movementLocalRot * Vector3.right * 10f));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (movementLocalRot * Vector3.up * 10f));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (movementLocalRot * Vector3.forward * 10f));
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + (movInputDir * 10f));
    }

}
