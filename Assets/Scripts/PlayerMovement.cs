using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float SensitivityX = 250f;
    public float SensitivityY = 250f;
    public float AngleLimit = 60f;
    private Animator _Animator;
    public AudioSource Walk;
    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * SensitivityX);

        var horizontal = Input.GetAxis("Horizontal") * Speed;
        var vertical = Input.GetAxis("Vertical") * Speed;

        if (horizontal != 0 || vertical != 0)
        {
            if (!Walk.isPlaying)
                Walk.Play();

            _Animator.SetBool("Walks", true);
        }
        else
        {
            if (Walk.isPlaying)
                Walk.Stop();
            _Animator.SetBool("Walks", false);
        }

        horizontal *= Time.deltaTime;
        vertical *= Time.deltaTime;
        transform.Translate(horizontal, 0, vertical);

    }
}
