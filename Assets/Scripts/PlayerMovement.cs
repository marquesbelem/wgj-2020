using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Earth;
    public float Speed;
    public float SpeedRotation;
    private GameObject _Camera;
    public GameObject Armature;
    private void Start()
    {
        _Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var position = new Vector3(-(float)Math.Round(vertical), 0, (float)Math.Round(horizontal));

        // transform.RotateAround(Earth.transform.position, position, Speed * Time.deltaTime);
        // _Camera.transform.RotateAround(Earth.transform.position, position, Speed * Time.deltaTime);

        var rotationEarth = position * SpeedRotation * Time.deltaTime;
        Earth.transform.Rotate(rotationEarth);


        if (Math.Round(horizontal) == -1)
            transform.rotation = Quaternion.Euler(0, -90, 0);
        if (Math.Round(horizontal) == 1)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        if (Math.Round(vertical) == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        if (Math.Round(vertical) == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
