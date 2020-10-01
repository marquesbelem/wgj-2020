using UnityEngine;
using UnityEngine.UI;

namespace GalloUtils {
    public class Frame3D : MonoBehaviour {

        public Rect3 rect = new Rect3(-1, -1, -1, 2, 2, 2);

        public Vector3 RectCenter {
            get {
                return transform.position + (transform.rotation * rect.Center);
            }
        }

        public bool IsInside(Vector3 point) {
            return rect.Contains(transform.InverseTransformPoint(point));
        }
        public Vector3 ClampInside(Vector3 point) {
            return transform.TransformPoint(Vector3Utils.ClampInside(transform.InverseTransformPoint(point), rect));
        }
        public Vector3 ClampOutside(Vector3 point) {
            return transform.TransformPoint(Vector3Utils.ClampOutside(transform.InverseTransformPoint(point), rect));
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            GizmosUtils.DrawCube(RectCenter, rect.size.ScaledBy(transform.localScale), transform.rotation);
        }

    }

}
