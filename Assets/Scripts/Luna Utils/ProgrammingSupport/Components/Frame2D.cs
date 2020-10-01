using UnityEngine;
using UnityEngine.UI;

namespace GalloUtils {
    public class Frame2D : MonoBehaviour {

        public Rect rect = new Rect(-1,-1,2,2);

        public Vector3 RectCenter {
            get {
                return transform.position + (transform.rotation * rect.center);
            }
        }

        public bool IsInside(Vector3 point) {
            return rect.Contains(transform.InverseTransformPoint(point), true);
        }
        public Vector3 ClampInside(Vector3 point) {
            return transform.TransformPoint(Vector2Utils.ClampInside(transform.InverseTransformPoint(point), rect));
        }
        public Vector3 ClampOutside(Vector3 point) {
            return transform.TransformPoint(Vector2Utils.ClampOutside(transform.InverseTransformPoint(point), rect));
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            GizmosUtils.DrawRect(RectCenter, rect.size.ScaledBy(transform.localScale), transform.rotation);
        }

    }

}
