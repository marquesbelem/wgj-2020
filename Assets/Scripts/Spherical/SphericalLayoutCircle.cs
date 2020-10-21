using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SphericalLayoutCircle : MonoBehaviour {

    public SphericalCoordinates sphereCoordinatesRef;
    public SphericalCoordinates SphereCoordinatesRef {
        get {
            if (sphereCoordinatesRef == null) {
                sphereCoordinatesRef = GetComponentInParent<SphericalCoordinates>();
            }
            return sphereCoordinatesRef;
        }
    }
    private void OnValidate() {
        _ = SphereCoordinatesRef;
    }

    public float radius;

    public List<SphericalCoordinates> ChildrenCoordinates {
        get {
            List<SphericalCoordinates> result = new List<SphericalCoordinates>();
            foreach (Transform child in transform) {
                SphericalCoordinates coord = child.GetComponent<SphericalCoordinates>();
                if (coord != null) {
                    result.Add(coord);
                }
            }
            return result;
        }
    }

    private void Update() {
        if (!Application.isPlaying) SetChildrenPos();
    }
    public void SetChildrenPos() {
        List<SphericalCoordinates> childrenCoordinates = ChildrenCoordinates;
        for (int i = 0; i < childrenCoordinates.Count; i++) {
            float angle = 360f * i / childrenCoordinates.Count;
            childrenCoordinates[i].radius = SphereCoordinatesRef.radius;
            childrenCoordinates[i].transform.rotation = SphereCoordinatesRef.transform.rotation * Quaternion.Euler(radius, angle, 0f);
            childrenCoordinates[i].ApplyToTransform();
        }
    }

}
