using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SphericalRegionCircle : MonoBehaviour, ISphericalRegion {

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
        Vector3 hypotheticalPos = dir.normalized * SphereCoordinatesRef.radius;
        Vector3 deltaDir = (SphereCoordinatesRef.transform.position - hypotheticalPos).normalized;
        return Vector3.Angle(transform.up, dir) <= AngularExtension(deltaDir);
    }
    public bool WouldOverlap(ISphericalRegion other, Vector3 hypotheticalUp) {
        float angularBonus = 0f;
        Vector3 hypotheticalPos = hypotheticalUp.normalized * SphereCoordinatesRef.radius;
        Vector3 deltaDir = (SphereCoordinatesRef.transform.position - hypotheticalPos).normalized;
        if (other != null) {
            angularBonus = other.AngularExtension(deltaDir);
            if (other.SphereCoordinatesRef.radius <= 0f) {
                return true;
            }
        }
        else 
        if (SphereCoordinatesRef.radius <= 0f) {
            return true;
        }
        return Vector3.Angle(transform.up, hypotheticalUp) <= AngularExtension(-deltaDir) + angularBonus;
    }
    public float AngularExtension(Vector3 dir) {
        return angularTreshold;
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
                Handles.DrawWireArc(Vector3.zero, transform.right, posNormalized, angularTreshold, posMagnitude);
                Handles.DrawWireArc(Vector3.zero, -transform.right, posNormalized, angularTreshold, posMagnitude);
                Handles.DrawWireArc(Vector3.zero, transform.forward, posNormalized, angularTreshold, posMagnitude);
                Handles.DrawWireArc(Vector3.zero, -transform.forward, posNormalized, angularTreshold, posMagnitude);
            }
            else {
                Gizmos.color = color;
                Gizmos.DrawWireSphere(transform.position, sphereCoordinates.radius);
            }
        }
    }
#endif


}
