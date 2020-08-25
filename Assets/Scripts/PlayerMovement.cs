using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Earth;
    public float Speed;
    public float SpeedRotation;
    private GameObject _Camera;
    private void Start()
    {
        _Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var position = new Vector3(vertical, 0, -horizontal);

        transform.RotateAround(Earth.transform.position, position, Speed * Time.deltaTime);
        _Camera.transform.RotateAround(Earth.transform.position, position, Speed * Time.deltaTime);

        var rotationEarth = position * SpeedRotation * Time.deltaTime;
        Earth.transform.Rotate(rotationEarth);

    }
}
