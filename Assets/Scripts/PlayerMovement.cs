using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject Earth;
    public float Speed;
    void Start()
    {

    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var position = new Vector3(vertical, 0, -horizontal);

        transform.RotateAround(Earth.transform.position, position, Speed * Time.deltaTime);
        Earth.transform.Rotate(position);
    }
}
