using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Earth;
    public float Speed;
    public float SpeedRotation;
   
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var position = new Vector3(-(float)Math.Round(vertical), 0, (float)Math.Round(horizontal));

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
