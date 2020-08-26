using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraaaa : MonoBehaviour
{
    public GameObject player;
    public float Tempo = 0.8f;

    public float sensibilidadeMouse = 200.0f;
    public float anguloMaximo = 30.0f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    public GameObject Earth;
    public GameObject PivotCenter;
    public float SpeedRotationEarth;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, player.transform.position, Tempo);
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * sensibilidadeMouse * Time.deltaTime;
        rotX += mouseY * sensibilidadeMouse * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -anguloMaximo, anguloMaximo);

        Quaternion localRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = localRotation;

        player.transform.rotation = Quaternion.Euler(0, rotY, 0.0f);

        if (Input.GetMouseButton(1))
        {
            Earth.transform.RotateAround(PivotCenter.transform.position, transform.up, Input.GetAxis("Mouse X") * SpeedRotationEarth * Time.deltaTime);
            Earth.transform.RotateAround(PivotCenter.transform.position, transform.right, Input.GetAxis("Mouse Y") * SpeedRotationEarth * Time.deltaTime);
        }

    }
}
