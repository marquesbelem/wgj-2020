using UnityEngine;

namespace GalloUtils {
    public static partial class PlaneExtension {

        public static bool ContainsPoint(this Plane plane, Vector3 point) {
            return plane.GetDistanceToPoint(point) == 0f;
        }

        public static Vector3 TransformPoint(this Plane plane, Vector2 point) {
            return Quaternion.FromToRotation(Vector3.forward, plane.normal) * ((Vector3)point).WithZ(plane.distance);
        }

    }

}