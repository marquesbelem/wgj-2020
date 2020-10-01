using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GravityAttractor : MonoBehaviour {
    public float gravity = -10; 
    
    public void Attract(GravityBody body) {
        Transform bodyTransform = body.transform;
        Vector3 direction = (bodyTransform.position - transform.position).normalized;
        bodyTransform.rotation = Quaternion.FromToRotation(bodyTransform.up, direction) * bodyTransform.rotation;
        body.rigidbodyRef.AddForce(direction * gravity, ForceMode.Force);
    }

    public void FixedUpdate() {
        GravityBody.instanceList.ForEach(Attract);
    }

}
