using GalloUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SphereSpawner : MonoBehaviour {

    public float radius = 1;
    public int quantity = 100;
    public List<GameObject> spawnable;
    public Transform parent;
    
    void Start() {
        for (int i = 0; i < quantity; i++) {
            Vector3 direction = Random.insideUnitSphere.normalized;
            Transform newObj = Instantiate(spawnable.RandomElement()).transform;
            newObj.position = transform.position + direction.normalized * radius;
            newObj.up = direction;
            newObj.parent = parent;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
