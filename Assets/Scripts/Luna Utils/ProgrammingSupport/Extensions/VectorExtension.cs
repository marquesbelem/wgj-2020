using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {

    public static partial class Vector2Extension {

        public static Vector2 Rounded(this Vector2 v, RoundMethod method = RoundMethod.Round) {
            switch (method) {
                case RoundMethod.Round:
                    return Vector2Utils.Round(v);
                case RoundMethod.Floor:
                    return Vector2Utils.Floor(v);
                case RoundMethod.Ceil:
                    return Vector2Utils.Ceil(v);
                default:
                    return v;
            }
        }
        public static Vector2Int RoundedToInt(this Vector2 v, RoundMethod method = RoundMethod.Round) {
            switch (method) {
                case RoundMethod.Round:
                    return Vector2Int.RoundToInt(v);
                case RoundMethod.Floor:
                    return Vector2Int.FloorToInt(v);
                case RoundMethod.Ceil:
                    return Vector2Int.CeilToInt(v);
                default:
                    return default;
            }
        }

        public static Vector2 Clamped(this Vector2 v, Rect limits) {
            return Vector2Utils.ClampInside(v, limits);
        }

        public static Vector2 Abs(this Vector2 v) {
            return v.WithParts(p => Mathf.Abs(p));
        }
        public static Vector2 RotatedBy(this Vector2 v, float angle) {
            return Quaternion.AngleAxis(angle, Vector3.forward) * v;
        }
        public static Vector2 RotatedBy(this Vector2 v, float angle, Vector2 point) {
            return (v - point).RotatedBy(angle) + point;
        }
        public static Vector2 ScaledBy(this Vector2 v0, Vector2 v1) {
            return new Vector2(v0.x * v1.x, v0.y * v1.y);
        }
        public static Vector2 DividedBy(this Vector2 v0, Vector2 v1) {
            return new Vector2(v0.x / v1.x, v0.y / v1.y);
        }

        public static float DistanceTo(this Vector2 v0, Vector2 v1) {
            return Vector2Utils.Distance(v0, v1);
        }
        public static float SqrDistanceTo(this Vector2 v0, Vector2 v1) {
            return Vector2Utils.SqrDistance(v0, v1);
        }
        public static float ManhatanDistanceTo(this Vector2 v0, Vector2 v1) {
            return Vector2Utils.ManhatanDistance(v0, v1);
        }
        public static float ManhatanMagnitude(this Vector2 v) {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y);
        }

        public static Vector2 WithX(this Vector2 v, float x) {
            return new Vector2(x, v.y);
        }
        public static Vector2 WithY(this Vector2 v, float y) {
            return new Vector2(v.x, y);
        }
        public static Vector2 WithX(this Vector2 v, Func<float, float> operation) {
            return new Vector2(operation.Invoke(v.x), v.y);
        }
        public static Vector2 WithY(this Vector2 v, Func<float, float> operation) {
            return new Vector2(v.x, operation.Invoke(v.y));
        }

        public static Vector2 WithInvertedX(this Vector2 v) {
            return v.WithX(x => -1f * x);
        }
        public static Vector2 WithInvertedY(this Vector2 v) {
            return v.WithY(y => -1f * y);
        }
        public static Vector2 ProjectedToX(this Vector2 v) {
            return v.WithY(0f);
        }
        public static Vector2 ProjectedToY(this Vector2 v) {
            return v.WithX(0f);
        }

        public static Vector2 WithParts(this Vector2 v, Func<float, float> operation) {
            return new Vector2(operation.Invoke(v.x), operation.Invoke(v.y));
        }
        public static bool AllParts(this Vector2 v, Predicate<float> match) {
            return match.Invoke(v.x) && match.Invoke(v.y);
        }
        public static bool AnyPart(this Vector2 v, Predicate<float> match) {
            return match.Invoke(v.x) || match.Invoke(v.y);
        }

        public static Vector2 LerpedTo(this Vector2 v1, Vector2 v2, float t) {
            return Vector2.Lerp(v1, v2, t);
        }
        public static Vector2 MidpointTo(this Vector2 v1, Vector2 v2) {
            return GeometryUtils.Midpoint(v1, v2);
        }

        public static Vector2 WithMagnitude(this Vector2 v, float newMagnitude) {
            return v.normalized * newMagnitude;
        }

        public static Vector2 MultipliedBy(this Vector2 c, float[,] m) {
            return new Vector2(m[0,0] * c.x + m[0,1] * c.y, m[1, 0] * c.x + m[1, 1] * c.y);
        }

    }

    public static partial class Vector2IntExtension {

        public static Vector2Int RotatedBy90Clockwise(this Vector2Int v) {
            return new Vector2Int(v.y, -v.x);
        }
        public static Vector2Int RotatedBy180Clockwise(this Vector2Int v) {
            return Vector2Int.zero - v;
        }
        public static Vector2Int RotatedBy270Clockwise(this Vector2Int v) {
            return new Vector2Int(-v.y, v.x);
        }

        public static Vector2Int RotatedBy90Clockwise(this Vector2Int v, int times) {
            switch (times % 4) {
                case 0:
                    return v;
                case 1:
                    return v.RotatedBy90Clockwise();
                case 2:
                    return v.RotatedBy180Clockwise();
                case 3:
                    return v.RotatedBy270Clockwise();
                default:
                    return Vector2Int.zero;
            }
        }
        public static Vector2Int RotatedBy90Counterclockwise(this Vector2Int v, int times) {
            switch (times % 4) {
                case 0:
                    return v;
                case 1:
                    return v.RotatedBy270Clockwise();
                case 2:
                    return v.RotatedBy180Clockwise();
                case 3:
                    return v.RotatedBy90Clockwise();
                default:
                    return Vector2Int.zero;
            }
        }

        public static Vector2Int Abs(this Vector2Int v) {
            return new Vector2Int(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }
        public static Vector2Int ScaledBy(this Vector2Int v0, Vector2Int v1) {
            return new Vector2Int(v0.x * v1.x, v0.y * v1.y);
        }
        public static Vector2Int DividedBy(this Vector2Int v0, Vector2Int v1) {
            return new Vector2Int(v0.x / v1.x, v0.y / v1.y);
        }

        public static Vector2Int Clamped(this Vector2Int v, RectInt limits) {
            return Vector2IntUtils.Clamp(v, limits.min, limits.max);
        }

        public static float DistanceTo(this Vector2Int v0, Vector2Int v1) {
            return (v1 - v0).magnitude;
        }
        public static float SqrDistanceTo(this Vector2Int v0, Vector2Int v1) {
            return (v1 - v0).sqrMagnitude;
        }
        public static int ManhatanDistanceTo(this Vector2Int v0, Vector2Int v1) {
            return Mathf.Abs(v1.x - v0.x) + Mathf.Abs(v1.y - v0.y);
        }
        public static int ManhatanMagnitude(this Vector2Int v) {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y);
        }

        public static Vector2Int WithX(this Vector2Int v, int x) {
            return new Vector2Int(x, v.y);
        }
        public static Vector2Int WithY(this Vector2Int v, int y) {
            return new Vector2Int(v.x, y);
        }
        public static Vector2Int WithX(this Vector2Int v, Func<int, int> operation) {
            return new Vector2Int(operation.Invoke(v.x), v.y);
        }
        public static Vector2Int WithY(this Vector2Int v, Func<int, int> operation) {
            return new Vector2Int(v.x, operation.Invoke(v.y));
        }

        public static Vector2Int WithInvertedX(this Vector2Int v) {
            return v.WithX(x => -1 * x);
        }
        public static Vector2Int WithInvertedY(this Vector2Int v) {
            return v.WithY(y => -1 * y);
        }
        public static Vector2Int ProjectedToX(this Vector2Int v) {
            return v.WithY(0);
        }
        public static Vector2Int ProjectedToY(this Vector2Int v) {
            return v.WithX(0);
        }

        public static bool IsNeighborsWith(this Vector2Int self, Vector2Int other, bool includeDiagonals = false) {
            return Vector2IntUtils.AreNeighbors(self, other, includeDiagonals);
        }
        public static List<Vector2Int> Neighbors(this Vector2Int v, bool includeDiagonals = false) {
            return Vector2IntUtils.Neighbors(includeDiagonals).ThruFuncElement(n => n + v);
        }

        public static Vector2Int WithParts(this Vector2Int v, Func<int, int> operation) {
            return new Vector2Int(operation.Invoke(v.x), operation.Invoke(v.y));
        }
        public static bool AllParts(this Vector2Int v, Predicate<int> match) {
            return match.Invoke(v.x) && match.Invoke(v.y);
        }
        public static bool AnyPart(this Vector2Int v, Predicate<int> match) {
            return match.Invoke(v.x) || match.Invoke(v.y);
        }

        public static Vector3 ToVec3(this Vector2Int self) {
            return new Vector3(self.x, self.y, 0f);
        }

    }

    public static partial class Vector3Extension {

        public static Vector3 Rounded(this Vector3 v, RoundMethod method = RoundMethod.Round) {
            switch (method) {
                case RoundMethod.Round:
                    return Vector3Utils.Round(v);
                case RoundMethod.Floor:
                    return Vector3Utils.Floor(v);
                case RoundMethod.Ceil:
                    return Vector3Utils.Ceil(v);
                default:
                    return v;
            }
        }
        public static Vector3Int RoundedToInt(this Vector3 v, RoundMethod method = RoundMethod.Round) {
            switch (method) {
                case RoundMethod.Round:
                    return Vector3Int.RoundToInt(v);
                case RoundMethod.Floor:
                    return Vector3Int.FloorToInt(v);
                case RoundMethod.Ceil:
                    return Vector3Int.CeilToInt(v);
                default:
                    return default;
            }
        }

        public static Vector3 Rounded(this Vector3 v) {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
        public static Vector3Int RoundedToInt(this Vector3 v) {
            return Vector3Int.RoundToInt(v); ;
        }

        public static Vector3 Abs(this Vector3 v) {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static Vector3 ScaledBy(this Vector3 v0, Vector3 v1) {
            return new Vector3(v0.x * v1.x, v0.y * v1.y, v0.z * v1.z);
        }
        public static Vector3 DividedBy(this Vector3 v0, Vector3 v1) {
            return new Vector3(v0.x / v1.x, v0.y / v1.y, v0.z / v1.z);
        }

        public static float DistanceTo(this Vector3 v0, Vector3 v1) {
            return Vector3Utils.Distance(v0, v1);
        }
        public static float SqrDistanceTo(this Vector3 v0, Vector3 v1) {
            return Vector3Utils.SqrDistance(v0, v1);
        }
        public static float ManhatanDistanceTo(this Vector3 v0, Vector3 v1) {
            return Vector3Utils.ManhatanDistance(v0, v1);
        }
        public static float ManhatanMagnitude(this Vector3 v) {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
        }

        public static Vector3 WithX(this Vector3 v, float x) {
            return new Vector3(x, v.y, v.z);
        }
        public static Vector3 WithY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }
        public static Vector3 WithZ(this Vector3 v, float z) {
            return new Vector3(v.x, v.y, z);
        }
        public static Vector3 WithX(this Vector3 v, Func<float, float> operation) {
            return new Vector3(operation.Invoke(v.x), v.y, v.z);
        }
        public static Vector3 WithY(this Vector3 v, Func<float, float> operation) {
            return new Vector3(v.x, operation.Invoke(v.y), v.z);
        }
        public static Vector3 WithZ(this Vector3 v, Func<float, float> operation) {
            return new Vector3(v.x, v.y, operation.Invoke(v.z));
        }

        public static Vector3 WithInvertedX(this Vector3 v) {
            return v.WithX(x => -1f * v.x);
        }
        public static Vector3 WithInvertedY(this Vector3 v) {
            return v.WithY(y => -1f * v.y);
        }
        public static Vector3 WithInvertedZ(this Vector3 v) {
            return v.WithZ(z => -1f * v.z);
        }

        public static Vector3 ProjectedToX(this Vector3 v) {
            return new Vector3(v.x, 0f, 0f);
        }
        public static Vector3 ProjectedToY(this Vector3 v) {
            return new Vector3(0f, v.y, 0f);
        }
        public static Vector3 ProjectedToZ(this Vector3 v) {
            return new Vector3(0f, 0f, v.z);
        }
        public static Vector3 ProjectedToXY(this Vector3 v) {
            return v.WithZ(0f);
        }
        public static Vector3 ProjectedToXZ(this Vector3 v) {
            return v.WithY(0f);
        }
        public static Vector3 ProjectedToYZ(this Vector3 v) {
            return v.WithX(0f);
        }

        public static Vector3 WithParts(this Vector3 v, Func<float, float> operation) {
            return new Vector3(operation.Invoke(v.x), operation.Invoke(v.y), operation.Invoke(v.z));
        }
        public static bool AllParts(this Vector3 v, Predicate<float> match) {
            return match.Invoke(v.x) && match.Invoke(v.y) && match.Invoke(v.z);
        }
        public static bool AnyPart(this Vector3 v, Predicate<float> match) {
            return match.Invoke(v.x) || match.Invoke(v.y) || match.Invoke(v.z);
        }

        public static Vector3 LerpedTo(this Vector3 v1, Vector3 v2, float t) {
            return Vector3.Lerp(v1, v2, t);
        }
        public static Vector3 MidpointTo(this Vector3 v1, Vector3 v2) {
            return (v1 + v2) / 2f;
        }

        public static Vector3 WithMagnitude(this Vector3 v, float newMagnitude) {
            return v.normalized * newMagnitude;
        }

    }

    public static partial class Vector3IntExtension {
        
        public static Vector3Int Abs(this Vector3Int v) {
            return new Vector3Int(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static Vector3Int ScaledBy(this Vector3Int v0, Vector3Int v1) {
            return new Vector3Int(v0.x * v1.x, v0.y * v1.y, v0.z * v1.z);
        }
        public static Vector3Int DividedBy(this Vector3Int v0, Vector3Int v1) {
            return new Vector3Int(v0.x / v1.x, v0.y / v1.y, v0.z / v1.z);
        }

        public static Vector3Int Clamped(this Vector3Int v, Rect3Int limits) {
            return Vector3IntUtils.Clamp(v, limits.Min, limits.Max);
        }

        public static int ManhatanDistanceTo(this Vector3Int v0, Vector3Int v1) {
            return Mathf.Abs(v1.x - v0.x) + Mathf.Abs(v1.y - v0.y) + Mathf.Abs(v1.z - v0.z);
        }
        public static float DistanceTo(this Vector3Int v0, Vector3Int v1) {
            return (v1 - v0).magnitude;
        }
        public static int SqrDistanceTo(this Vector3Int v0, Vector3Int v1) {
            return (v1 - v0).sqrMagnitude;
        }
        public static int ManhatanMagnitude(this Vector3Int v) {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);
        }

        public static Vector3Int WithX(this Vector3Int v, int x) {
            return new Vector3Int(x, v.y, v.z);
        }
        public static Vector3Int WithY(this Vector3Int v, int y) {
            return new Vector3Int(v.x, y, v.z);
        }
        public static Vector3Int WithZ(this Vector3Int v, int z) {
            return new Vector3Int(v.x, v.y, z);
        }
        public static Vector3Int WithX(this Vector3Int v, Func<int, int> operation) {
            return new Vector3Int(operation.Invoke(v.x), v.y, v.z);
        }
        public static Vector3Int WithY(this Vector3Int v, Func<int, int> operation) {
            return new Vector3Int(v.x, operation.Invoke(v.y), v.z);
        }
        public static Vector3Int WithZ(this Vector3Int v, Func<int, int> operation) {
            return new Vector3Int(v.x, v.y, operation.Invoke(v.z));
        }

        public static Vector3Int WithInvertedX(this Vector3Int v) {
            return v.WithX(x => -1 * v.x);
        }
        public static Vector3Int WithInvertedY(this Vector3Int v) {
            return v.WithY(y => -1 * v.y);
        }
        public static Vector3Int WithInvertedZ(this Vector3Int v) {
            return v.WithZ(z => -1 * v.z);
        }

        public static Vector3Int ProjectedToX(this Vector3Int v) {
            return new Vector3Int(v.x, 0, 0);
        }
        public static Vector3Int ProjectedToY(this Vector3Int v) {
            return new Vector3Int(0, v.y, 0);
        }
        public static Vector3Int ProjectedToZ(this Vector3Int v) {
            return new Vector3Int(0, 0, v.z);
        }
        public static Vector3Int ProjectedToXY(this Vector3Int v) {
            return v.WithZ(0);
        }
        public static Vector3Int ProjectedToXZ(this Vector3Int v) {
            return v.WithY(0);
        }
        public static Vector3Int ProjectedToYZ(this Vector3Int v) {
            return v.WithX(0);
        }

        public static bool IsNeighborsWith(this Vector3Int self, Vector3Int other, Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) {
            return Vector3IntUtils.AreNeighbors(self, other, neighboringRule);
        }
        public static List<Vector3Int> Neighbors(this Vector3Int v, Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) {
            return Vector3IntUtils.Neighbors(neighboringRule).ThruFuncElement(n => n + v);
        }

        public static Vector3Int WithParts(this Vector3Int v, Func<int, int> operation) {
            return new Vector3Int(operation.Invoke(v.x), operation.Invoke(v.y), operation.Invoke(v.z));
        }
        public static bool AllParts(this Vector3Int v, Predicate<int> match) {
            return match.Invoke(v.x) && match.Invoke(v.y) && match.Invoke(v.z);
        }
        public static bool AnyPart(this Vector3Int v, Predicate<int> match) {
            return match.Invoke(v.x) || match.Invoke(v.y) || match.Invoke(v.z);
        }

    }

    public static partial class Vector4Extension {

        public static Vector4 Abs(this Vector4 v) {
            return new Vector4(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
        }
        public static Vector4 ScaledBy(this Vector4 v0, Vector4 v1) {
            return new Vector4(v0.x * v1.x, v0.y * v1.y, v0.z * v1.z, v0.w * v1.w);
        }
        public static Vector4 DividedBy(this Vector4 v0, Vector4 v1) {
            return new Vector4(v0.x / v1.x, v0.y / v1.y, v0.z / v1.z, v0.w / v1.w);
        }

        public static float ManhatanDistanceTo(this Vector4 v0, Vector4 v1) {
            return Mathf.Abs(v1.x - v0.x) + Mathf.Abs(v1.y - v0.y) + Mathf.Abs(v1.z - v0.z) + Mathf.Abs(v1.w - v0.w);
        }
        public static float DistanceTo(this Vector4 v0, Vector4 v1) {
            return (v1 - v0).magnitude;
        }
        public static float SqrDistanceTo(this Vector4 v0, Vector4 v1) {
            return (v1 - v0).sqrMagnitude;
        }
        public static float ManhatanMagnitude(this Vector4 v) {
            return Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z) + Mathf.Abs(v.w);
        }

        public static Vector4 WithX(this Vector4 v, float x) {
            return new Vector4(x, v.y, v.z, v.w);
        }
        public static Vector4 WithY(this Vector4 v, float y) {
            return new Vector4(v.x, y, v.z, v.w);
        }
        public static Vector4 WithZ(this Vector4 v, float z) {
            return new Vector4(v.x, v.y, z, v.w);
        }
        public static Vector4 WithW(this Vector4 v, float w) {
            return new Vector4(v.x, v.y, v.z, w);
        }
        public static Vector4 WithX(this Vector4 v, Func<float, float> operation) {
            return new Vector4(operation.Invoke(v.x), v.y, v.z, v.w);
        }
        public static Vector4 WithY(this Vector4 v, Func<float, float> operation) {
            return new Vector4(v.x, operation.Invoke(v.y), v.z, v.w);
        }
        public static Vector4 WithZ(this Vector4 v, Func<float, float> operation) {
            return new Vector4(v.x, v.y, operation.Invoke(v.z), v.w);
        }
        public static Vector4 WithW(this Vector4 v, Func<float, float> operation) {
            return new Vector4(v.x, v.y, v.z, operation.Invoke(v.w));
        }

        public static Vector4 WithInvertedX(this Vector4 v) {
            return v.WithX(x => -1f * v.x);
        }
        public static Vector4 WithInvertedY(this Vector4 v) {
            return v.WithY(y => -1f * v.y);
        }
        public static Vector4 WithInvertedZ(this Vector4 v) {
            return v.WithZ(z => -1f * v.z);
        }
        public static Vector4 WithInvertedW(this Vector4 v) {
            return v.WithW(z => -1f * v.w);
        }

        public static Vector4 ProjectedToX(this Vector4 v) {
            return new Vector4(v.x, 0f, 0f, 0f);
        }
        public static Vector4 ProjectedToY(this Vector4 v) {
            return new Vector4(0f, v.y, 0f, 0f);
        }
        public static Vector4 ProjectedToZ(this Vector4 v) {
            return new Vector4(0f, 0f, v.z, 0f);
        }
        public static Vector4 ProjectedToW(this Vector4 v) {
            return new Vector4(0f, 0f, 0f, v.w);
        }

        public static Vector4 WithParts(this Vector4 v, Func<float, float> operation) {
            return new Vector4(operation.Invoke(v.x), operation.Invoke(v.y), operation.Invoke(v.z), operation.Invoke(v.w));
        }
        public static bool AllParts(this Vector4 v, Predicate<float> match) {
            return match.Invoke(v.x) && match.Invoke(v.y) && match.Invoke(v.z) && match.Invoke(v.w);
        }
        public static bool AnyPart(this Vector4 v, Predicate<float> match) {
            return match.Invoke(v.x) || match.Invoke(v.y) || match.Invoke(v.z) || match.Invoke(v.w);
        }

    }

    public static partial class Vector2ListExtension {

        public static float PathLength(this List<Vector2> path) {
            return path.SumPairs(Vector2.Distance);
        }
        public static List<float> PathSegmentLengths(this List<Vector2> path) {
            return path.MergedInPairs(Vector2.Distance);
        }
        public static Vector2 LerpPath(this List<Vector2> path, float t) {
            if (path.FindAccumulatedElementPair(t, Vector2.Distance, out float localT, out Vector2 p1, out Vector2 p2)) {
                return p1.LerpedTo(p2, localT);
            }
            if (t < 0f) {
                return path.First();
            }
            else {
                return path.Last();
            }
        }

    }

    public static partial class Vector3ListExtension {

        public static float PathLength(this List<Vector3> path) {
            return path.SumPairs(Vector3.Distance);
        }
        public static List<float> PathSegmentLengths(this List<Vector3> path) {
            return path.MergedInPairs(Vector3.Distance);
        }
        public static Vector3 LerpPath(this List<Vector3> path, float t) {
            if (path.FindAccumulatedElementPair(t, Vector3.Distance, out float localT, out Vector3 p1, out Vector3 p2)) {
                return p1.LerpedTo(p2, localT);
            }
            if (t < 0f) {
                return path.First();
            }
            else {
                return path.Last();
            }
        }

    }

    public enum Neighboring2Type {
        SharesEdge = 1,
        SharesVertex = 2,
        SharesBoth = 3
    }

    public enum Neighboring3Type {
        SharesFace = 1,
        SharesEdge = 2,
        SharesVertex = 4,
        All = 7
    }

}
