using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float SensitivityX = 250f; 
    public float SensitivityY = 250f;
    public float AngleLimit = 60f;

    private Transform CameraTarget;
    private float VerticalLookRotation;
    private void Start()
    {
        CameraTarget = Camera.main.transform;
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * SensitivityX);
        //VerticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * SensitivityY;
        //VerticalLookRotation = Mathf.Clamp(VerticalLookRotation, -AngleLimit, AngleLimit);
        //CameraTarget.localEulerAngles = Vector3.left * VerticalLookRotation;

        var horizontal = Input.GetAxis("Horizontal") * Speed;
        var vertical = Input.GetAxis("Vertical") * Speed;

        horizontal *= Time.deltaTime;
        vertical *= Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);
    }
}
