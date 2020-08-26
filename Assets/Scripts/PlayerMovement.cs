using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Earth;
    public float Speed;
    public float SpeedRotation;
    public Vector3 MaxPosition;
    public Vector3 MinPosition;
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * Speed;
        var vertical = Input.GetAxis("Vertical") * Speed;

        vertical *= Time.deltaTime;
        horizontal *= Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);
    }
}
