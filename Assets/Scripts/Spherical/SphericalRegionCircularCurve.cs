using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SphericalRegionCircularCurve : MonoBehaviour, ISphericalRegion {

    public AnimationCurve angularExtensionCurve;

    public SphericalCoordinates sphereCoordinatesRef;
    public SphericalCoordinates SphereCoordinatesRef {
        get {
            if (sphereCoordinatesRef == null) {
                sphereCoordinatesRef = GetComponentInParent<SphericalCoordinates>();
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
        Vector3 dir = (SphereCoordinatesRef.transform.position - hypotheticalPos).normalized;
        if (other != null) {
            angularBonus = other.AngularExtension(dir);
            if (other.SphereCoordinatesRef.radius <= 0f) {
                return true;
            }
        }
        else
        if (SphereCoordinatesRef.radius <= 0f) {
            return true;
        }
        return Vector3.Angle(transform.up, hypotheticalUp) <= AngularExtension(-dir) + angularBonus;
    }
    public float AngularExtension(Vector3 dir) {
        Vector3 localDir = SphereCoordinatesRef.transform.InverseTransformDirection(dir);
        localDir.y = 0f;
        if (localDir == Vector3.zero) {
            return angularExtensionCurve.Evaluate(0f);
        }
        localDir.Normalize();
        float angle = Vector3.SignedAngle(Vector3.forward, localDir, Vector3.up);
        if (angle < 0f) {
            angle += 360f;
        }
        return angularExtensionCurve.Evaluate(angle);
    }
    private void OnValidate() {
        _ = SphereCoordinatesRef;
    }

#if UNITY_EDITOR
    public void DrawGizmos(Color color) {
        SphericalCoordinates sphereCoordinates = SphereCoordinatesRef;
        if (sphereCoordinates != null) {
            if (sphereCoordinates.radius > 0f) {
                int vertexCount = angularExtensionCurve.length * 4;
                List<float> angularExtensions = new List<float>();
                for (int i = 0; i < vertexCount; i++) {
                    angularExtensions.Add(angularExtensionCurve.Evaluate((float)i / vertexCount));
                }
                /*
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
                */
            }
            else {
                Gizmos.color = color;
                Gizmos.DrawWireSphere(transform.position, sphereCoordinates.radius);
            }
        }
    }
#endif

}
