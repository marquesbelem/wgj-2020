using UnityEngine;

namespace GalloUtils {
    public static partial class CameraExtension {

        public static float OrthographicWidth(this Camera cam) {
            return cam.aspect * cam.orthographicSize * 2f;
        }
        public static float OrthographicHeight(this Camera cam) {
            return cam.orthographicSize * 2f;
        }
        public static Vector2 OrthographicSize(this Camera cam) {
            return new Vector2(cam.OrthographicWidth(), cam.OrthographicHeight());
        }
        public static Rect OrthographicRect(this Camera cam) {
            return new Rect() {
                size = cam.OrthographicSize(),
                center = Vector2.zero
            };
        }

    }

}
