using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SphereRegion : MonoBehaviour {

    public float angularTreshold = 5f;
    public float RadiansTreshold => Mathf.Deg2Rad * angularTreshold;

    public SphereCoordinates sphereCoordinatesRef;
    public SphereCoordinates SphereCoordinatesRef {
        get {
            if (sphereCoordinatesRef == null) {
                sphereCoordinatesRef = GetComponentInParent<SphereCoordinates>();
            }
            return sphereCoordinatesRef;
        }
    }

    public bool Contains(Vector3 dir) {
        if (SphereCoordinatesRef.radius <= 0f) {
            return true;
        }
        return Vector3.Angle(transform.up, dir) <= angularTreshold;
    }
    public bool WouldOverlap(SphereRegion other, Vector3 hypotheticalUp) {
        float angularBonus = 0f;
        if (other == null) {
            angularBonus = other.angularTreshold;
        }
        else if (other.SphereCoordinatesRef.radius <= 0f) {
            return true;
        }
        if (SphereCoordinatesRef.radius <= 0f) {
            return true;
        }
        return Vector3.Angle(transform.up, hypotheticalUp) <= angularTreshold + angularBonus;
    }

    private void OnValidate() {
        _ = SphereCoordinatesRef;
    }


#if UNITY_EDITOR
    public void DrawGizmos(Color color) {
        SphereCoordinates sphereCoordinates = SphereCoordinatesRef;
        if (sphereCoordinates != null) {
            if (sphereCoordinates.radius > 0f) {
                float cos = Mathf.Cos(RadiansTreshold);
                float sin = Mathf.Sin(RadiansTreshold);
                float posMagnitude = transform.position.magnitude;
                Vector3 posNormalized = transform.position / posMagnitude;
                Handles.color = color;
                Handles.DrawWireDisc(posNormalized * posMagnitude * cos, posNormalized, posMagnitude * sin);
                Handles.DrawWireArc(Vector3.zero, Vector3.right, posNormalized, angularTreshold, posMagnitude);
                Handles.DrawWireArc(Vector3.zero, -Vector3.right, posNormalized, angularTreshold, posMagnitude);
                Handles.DrawWireArc(Vector3.zero, Vector3.forward, posNormalized, angularTreshold, posMagnitude);
                Handles.DrawWireArc(Vector3.zero, -Vector3.forward, posNormalized, angularTreshold, posMagnitude);
            }
            else {
                Gizmos.color = color;
                Gizmos.DrawWireSphere(transform.position, sphereCoordinates.radius);
            }
        }
    }
#endif


}
