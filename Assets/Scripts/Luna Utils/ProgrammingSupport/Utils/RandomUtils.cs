using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class RandomUtils {

        public static Vector2 RandomVector2() {
            return new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        public static Vector2 RandomVector2(Rect range) {
            return new Vector2(Random.Range(range.xMin, range.xMax), Random.Range(range.yMin, range.yMax));
        }
        public static Vector2 RandomVector2(Vector2 min, Vector2 max) {
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
        public static Vector2 RandomPointInTriangle(Vector2 origin, Vector2 u, Vector2 v) {
            Vector2 p = RandomVector2(new Rect(0f, 0f, 1f, 1f));
            if (p.x + p.y > 1f) {
                p = Vector2.one - p;
            }
            return origin + (p.x * u) + (p.y * v);
        }
        public static Vector2 RandomPointInCircle(float radius, Vector2 origin = default) {
            Vector2 p = RandomVector2();
            return origin + (new Vector2(radius, 0f) * Mathf.Sqrt(p.x)).RotatedBy(p.y * 2 * Mathf.PI);
        }

        public static Vector3 RandomVector3(Vector3 halfSize) {
            return RandomVector3(-halfSize, halfSize);
        }
        public static Vector3 RandomVector3(Vector3 min, Vector3 max) {
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }
        public static Vector3 RandomPointInTriangle(Vector3 origin, Vector3 u, Vector3 v) {
            Vector2 p = RandomVector2(new Rect(0f, 0f, 1f, 1f));
            if (p.x + p.y > 1f) {
                p = Vector2.one - p;
            }
            return origin + (p.x * u) + (p.y * v);
        }

    }

}