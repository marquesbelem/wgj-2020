using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {

    public static class RangeUtils {

        public static Range Lerp(Range r1, Range r2, float t) {
            return new Range(r1.position.LerpedTo(r2.position, t), r1.size.LerpedTo(r2.size, t));
        }
        public static float LerpValue(Range r, float t) {
            return Mathf.Lerp(r.Min, r.Max, t);
        }
        public static float InverseLerpValue(Range r, float value) {
            return Mathf.InverseLerp(r.Min, r.Max, value);
        }
        public static float RemapValue(Range from, Range target, float value) {
            return LerpValue(target, InverseLerpValue(from, value));
        }

        public static float Distance(Range r1, Range r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

    }
    public static class RectUtils {

        public static Rect Lerp(Rect r1, Rect r2, float t) {
            return new Rect(r1.position.LerpedTo(r2.position, t), r1.size.LerpedTo(r2.size, t));
        }
        public static Vector2 InterpolateValue(Rect r, Vector2 t) {
            return Vector2Utils.Iterpolate(r.min, r.max, t);
        }
        public static Vector2 InverseInterpolateValue(Rect r, Vector2 value) {
            return Vector2Utils.InverseIterpolate(r.min, r.max, value);
        }
        public static Vector2 RemapValue(Rect from, Rect target, Vector2 value) {
            return InterpolateValue(target, InverseInterpolateValue(from, value));
        }

        public static float Distance(Rect r1, Rect r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

    }
    public static class Rect3Utils {

        public static Rect3 Lerp(Rect3 r1, Rect3 r2, float t) {
            return new Rect3(r1.position.LerpedTo(r2.position, t), r1.size.LerpedTo(r2.size, t));
        }
        public static Vector3 InterpolateValue(Rect3 r, Vector3 t) {
            return Vector3Utils.Iterpolate(r.Min, r.Max, t);
        }
        public static Vector3 InverseInterpolateValue(Rect3 r, Vector3 value) {
            return Vector3Utils.InverseIterpolate(r.Min, r.Max, value);
        }
        public static Vector3 RemapValue(Rect3 from, Rect3 target, Vector3 value) {
            return InterpolateValue(target, InverseInterpolateValue(from, value));
        }

        public static float Distance(Rect3 r1, Rect3 r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

    }

    public static class RectIntUtils {

        public static RectInt.PositionEnumerator Enumerate(Vector2Int lengths) {
            return new RectInt(0, 0, lengths.x, lengths.y).allPositionsWithin;
        }
        public static RectInt.PositionEnumerator Enumerate(int lengthX, int lengthY) {
            return new RectInt(0, 0, lengthX, lengthY).allPositionsWithin;
        }
        public static RectInt.PositionEnumerator Enumerate(Vector2Int starts, Vector2Int lengths) {
            return new RectInt(starts.x, starts.y, lengths.x, lengths.y).allPositionsWithin;
        }
        public static RectInt.PositionEnumerator Enumerate(int startX, int startY, int lengthX, int lengthY) {
            return new RectInt(startX, startY, lengthX, lengthY).allPositionsWithin;
        }

        public static float Distance(RectInt r1, RectInt r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

    }
    public static class Rect3IntUtils {

        public static Rect3Int Enumerate(Vector3Int lengths) {
            return new Rect3Int(0, 0, 0, lengths.x - 1, lengths.y - 1, lengths.z - 1);
        }
        public static Rect3Int Enumerate(int lengthX, int lengthY, int lengthZ) {
            return new Rect3Int(0, 0, 0, lengthX - 1, lengthY - 1, lengthZ - 1);
        }
        public static Rect3Int Enumerate(Vector3Int starts, Vector3Int lengths) {
            return new Rect3Int(starts.x, starts.y, starts.z, lengths.x - 1, lengths.y - 1, lengths.z - 1);
        }
        public static Rect3Int Enumerate(int startX, int startY, int startZ, int lengthX, int lengthY, int lengthZ) {
            return new Rect3Int(startX, startY, startZ, lengthX - 1, lengthY - 1, lengthZ - 1);
        }

        public static float Distance(Rect3Int r1, Rect3Int r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

    }

}
