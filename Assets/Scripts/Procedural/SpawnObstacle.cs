using System;
using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;
#endif

public class SpawnObstacle : MonoBehaviour {

    public static List<SpawnObstacle> instanceList = new List<SpawnObstacle>();
    private void Awake() {
        instanceList.Add(this);
    }
    private void OnDestroy() {
        instanceList.Remove(this);
    }

    public float angularTreshold = 5f;
    public float RadiansTreshold => Mathf.PI * 2f * angularTreshold / 360f;

    public List<string> identifiers;

#if UNITY_EDITOR
    public void OnDrawGizmosSelected() {
        if (transform.position != Vector3.zero) {
            float cos = Mathf.Cos(RadiansTreshold);
            float sin = Mathf.Sin(RadiansTreshold);
            float posMagnitude = transform.position.magnitude;
            Vector3 posNormalized = transform.position / posMagnitude;
            Handles.color = Color.red;
            Handles.DrawWireDisc(posNormalized * posMagnitude * cos, posNormalized, posMagnitude * sin);
            Handles.DrawWireArc(Vector3.zero, Vector3.right, posNormalized, angularTreshold, posMagnitude);
            Handles.DrawWireArc(Vector3.zero, -Vector3.right, posNormalized, angularTreshold, posMagnitude);
            Handles.DrawWireArc(Vector3.zero, Vector3.forward, posNormalized, angularTreshold, posMagnitude);
            Handles.DrawWireArc(Vector3.zero, -Vector3.forward, posNormalized, angularTreshold, posMagnitude);
        }
    }
#endif

}
