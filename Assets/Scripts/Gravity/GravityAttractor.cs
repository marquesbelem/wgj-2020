using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GravityAttractor : MonoBehaviour
{
    public float Gravity = -10; 
    
    public void Attract(Transform body)
    {
        Vector3 target = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.rotation = Quaternion.FromToRotation(bodyUp, target) * body.rotation;
        body.gameObject.GetComponent<Rigidbody>().AddForce(target * Gravity);
    }
}
