using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {

    public static partial class Vector2Utils {

        public static float Distance(Vector2 v0, Vector2 v1) {
            return (v1 - v0).magnitude;
        }
        public static float SqrDistance(Vector2 v0, Vector2 v1) {
            return (v1 - v0).sqrMagnitude;
        }
        public static float ManhatanDistance(Vector2 v0, Vector2 v1) {
            return Mathf.Abs(v1.x - v0.x) + Mathf.Abs(v1.y - v0.y);
        }

        public static Vector2 ClampInside(Vector2 value, Vector2 min, Vector2 max) {
            return Vector2.Max(Vector2.Min(value, max), min);
        }
        public static Vector2 ClampInside(Vector2 value, Rect limits) {
            return ClampInside(value, limits.min, limits.max);
        }

        public static Vector2 ClampOutside(Vector2 value, Vector2 min, Vector2 max) {
            Vector2 result = value;
            Vector2 minDelta = value - min;
            Vector2 maxDelta = value - max;
            float selectedDelta = Mathf.Min(minDelta.x, minDelta.y, maxDelta.x, maxDelta.y);
            if (selectedDelta == minDelta.x) {
                result.x = minDelta.x;
            }
            else if (selectedDelta == maxDelta.x) {
                result.x = maxDelta.x;
            }
            else if (selectedDelta == minDelta.y) {
                result.y = minDelta.y;
            }
            else {
                result.y = maxDelta.y;
            }
            return result;
        }
        public static Vector2 ClampOutside(Vector2 value, Rect limits) {
            return ClampOutside(value, limits.min, limits.max);
        }

        public static Vector2 Round(Vector2 v) {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }
        public static Vector2 Floor(Vector2 v) {
            return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
        }
        public static Vector2 Ceil(Vector2 v) {
            return new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
        }

        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime) {
            current.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime);
            current.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime);
            return current;
        }
        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, Vector2 maxSpeed) {
            current.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed.x);
            current.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed.y);
            return current;
        }

        public static Vector2 Iterpolate(Vector2 min, Vector2 max, Vector2 t) {
            return new Vector2(Mathf.Lerp(min.x, max.x, t.x), Mathf.Lerp(min.y, max.y, t.y));
        }
        public static Vector2 InverseIterpolate(Vector2 min, Vector2 max, Vector2 value) {
            return new Vector2(Mathf.InverseLerp(min.x, max.x, value.x), Mathf.InverseLerp(min.y, max.y, value.y));
        }
        public static Vector2 Remap(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value) {
            return Iterpolate(oMin, oMax, InverseIterpolate(iMin, iMax, value));
        }
        public static Vector2 Remap(float iMin, float iMax, Vector2 oMin, Vector2 oMax, float value) {
            return Vector2.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
        }

        public static Vector2 IterpolateUnclamped(Vector2 min, Vector2 max, Vector2 t) {
            return new Vector2(Mathf.LerpUnclamped(min.x, max.x, t.x), Mathf.LerpUnclamped(min.y, max.y, t.y));
        }

    }

    public static partial class Vector2IntUtils {

        public static Vector2Int Clamp(Vector2Int value, Vector2Int min, Vector2Int max) {
            return Vector2Int.Max(Vector2Int.Min(value, max), min);
        }

        public static List<Vector2Int> Neighbors(bool includeDiagonals = false) {
            if (includeDiagonals) {
                return new List<Vector2Int>() {
                    new Vector2Int( 0, 1),
                    new Vector2Int( 1, 1),
                    new Vector2Int( 1, 0),
                    new Vector2Int( 1,-1),
                    new Vector2Int( 0,-1),
                    new Vector2Int(-1,-1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1)
                };
            }
            else {
                return new List<Vector2Int>() {
                    new Vector2Int( 0, 1),
                    new Vector2Int( 1, 0),
                    new Vector2Int( 0,-1),
                    new Vector2Int(-1, 0)
                };
            }
        }
        public static List<Vector2Int> RasterizeLine(Vector2Int from, Vector2Int to, DiagonalRasterizationType diagonalRasterizationType = DiagonalRasterizationType.GoStraight) {
            return RasterizeLineToGroups(from, to, diagonalRasterizationType).ToSingleList();
        }
        public static List<List<Vector2Int>> RasterizeLineToGroups(Vector2Int from, Vector2Int to, DiagonalRasterizationType diagonalRasterizationType = DiagonalRasterizationType.GoStraight) {
            //Debug.Log("    RasterizeLine(from = " + from + ", to = " + to + ")");
            List<List<Vector2Int>> result = new List<List<Vector2Int>>();
            int yMin = Mathf.Min(from.y, to.y);
            int yMax = Mathf.Max(from.y, to.y);
            int xMin = Mathf.Min(from.x, to.x);
            int xMax = Mathf.Max(from.x, to.x);
            int xSignal = (from.x < to.x) ? 1 : -1;
            int ySignal = (from.y < to.y) ? 1 : -1;
            float yDelta = yMax - yMin;
            float xDelta = xMax - xMin;
            if (from == to) {
                result.Add(new List<Vector2Int>() { from });
            }
            else if (from.x == to.x) {
                for (int i = 0; i <= yDelta; i++) {
                    result.Add(new List<Vector2Int>() { new Vector2Int(from.x, yMin + i) });
                }
            }
            else if (from.y == to.y) {
                for (int i = 0; i <= xDelta; i++) {
                    result.Add(new List<Vector2Int>() { new Vector2Int(xMin + i, from.y) });
                }
            }
            else {
                if (xDelta == yDelta) {
                    for (int i = 0; i <= xDelta; i++) {
                        Vector2Int nextPoint = new Vector2Int(from.x + (i * xSignal), from.y + (i * ySignal));
                        List<Vector2Int> nextList = new List<Vector2Int>();
                        if (result.Count > 0) {
                            Vector2Int last = result.Last().Last();
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    //Debug.Log("        where xDelta==yDelta and diagonalRasterizationType==GoThruAllDirectNeighbors");
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    nextList.Add(new Vector2Int(last.x, nextPoint.y));
                                    break;
                            }
                        }
                        nextList.Add(nextPoint);
                        result.Add(nextList);
                    }
                }
                else if (xDelta > yDelta) {
                    for (int i = 0; i <= xDelta; i++) {
                        int curY = Mathf.RoundToInt(i * yDelta / xDelta);
                        Vector2Int nextPoint = new Vector2Int(from.x + (i * xSignal), from.y + (curY * ySignal));
                        List<Vector2Int> nextList = new List<Vector2Int>();
                        if (result.Count > 0 && nextPoint.x != result.Last().Last().x && nextPoint.y != result.Last().Last().y) {
                            Vector2Int last = result.Last().Last();
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    //Debug.Log("        where xDelta>yDelta and diagonalRasterizationType==GoThruAllDirectNeighbors");
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    nextList.Add(new Vector2Int(last.x, nextPoint.y));
                                    break;
                            }
                        }
                        nextList.Add(nextPoint);
                        result.Add(nextList);
                    }
                }
                else {
                    for (int i = 0; i <= yDelta; i++) {
                        int curX = Mathf.RoundToInt(i * xDelta / yDelta);
                        Vector2Int nextPoint = new Vector2Int(from.x + (curX * xSignal), from.y + (i * ySignal));
                        List<Vector2Int> nextList = new List<Vector2Int>();
                        if (result.Count > 0 && nextPoint.x != result.Last().Last().x && nextPoint.y != result.Last().Last().y) {
                            Vector2Int last = result.Last().Last();
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    //Debug.Log("        where xDelta<yDelta and diagonalRasterizationType==GoThruAllDirectNeighbors");
                                    nextList.Add(new Vector2Int(nextPoint.x, last.y));
                                    nextList.Add(new Vector2Int(last.x, nextPoint.y));
                                    break;
                            }
                        }
                        nextList.Add(nextPoint);
                        result.Add(nextList);
                    }
                }
            }
            //Debug.Log("        return = " + result + ":");
            //result.ForEach(p => Debug.Log("            " + p));
            return result;
        }
        
        public static Vector2Int Min(Vector2Int a, Vector2Int b) {
            return new Vector2Int(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
        }
        public static Vector2Int Max(Vector2Int a, Vector2Int b) {
            return new Vector2Int(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
        }

    }

    public static partial class Vector3Utils {

        public static Vector3 Round(Vector3 v) {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
        public static Vector3 Floor(Vector3 v) {
            return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
        }
        public static Vector3 Ceil(Vector3 v) {
            return new Vector3(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));
        }

        public static float Distance(Vector3 v0, Vector3 v1) {
            return (v1 - v0).magnitude;
        }
        public static float SqrDistance(Vector3 v0, Vector3 v1) {
            return (v1 - v0).sqrMagnitude;
        }
        public static float ManhatanDistance(Vector3 v0, Vector3 v1) {
            return Mathf.Abs(v1.x - v0.x) + Mathf.Abs(v1.y - v0.y) + Mathf.Abs(v1.z - v0.z);
        }

        public static Vector3 ClampInside(Vector3 value, Vector3 min, Vector3 max) {
            return Vector3.Max(Vector3.Min(value, max), min);
        }

        public static Vector3 ClampOutside(Vector3 value, Vector3 min, Vector3 max) {
            Vector3 result = value;
            Vector3 minDelta = value - min;
            Vector3 maxDelta = value - max;
            float selectedDelta = Mathf.Min(minDelta.x, minDelta.y, maxDelta.x, maxDelta.y);
            if (selectedDelta == minDelta.x) {
                result.x = minDelta.x;
            }
            else if (selectedDelta == maxDelta.x) {
                result.x = maxDelta.x;
            }
            else if (selectedDelta == minDelta.y) {
                result.y = minDelta.y;
            }
            else {
                result.y = maxDelta.y;
            }
            return result;
        }
        
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime) {
            current.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime);
            current.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime);
            current.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime);
            return current;
        }
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, Vector3 maxSpeed) {
            current.x = Mathf.SmoothDamp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed.x);
            current.y = Mathf.SmoothDamp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed.y);
            current.z = Mathf.SmoothDamp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed.z);
            return current;
        }

        public static Vector3 Iterpolate(Vector3 min, Vector3 max, Vector3 t) {
            return new Vector3(Mathf.Lerp(min.x, max.x, t.x), Mathf.Lerp(min.y, max.y, t.y), Mathf.Lerp(min.z, max.z, t.z));
        }
        public static Vector3 InverseIterpolate(Vector3 min, Vector3 max, Vector3 value) {
            return new Vector3(Mathf.InverseLerp(min.x, max.x, value.x), Mathf.InverseLerp(min.y, max.y, value.y), Mathf.InverseLerp(min.z, max.z, value.z));
        }
        public static Vector3 Remap(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 value) {
            return Iterpolate(oMin, oMax, InverseIterpolate(iMin, iMax, value));
        }
        public static Vector3 Remap(float iMin, float iMax, Vector3 oMin, Vector3 oMax, float value) {
            return Vector3.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
        }

        public static Vector3 IterpolateUnclamped(Vector3 min, Vector3 max, Vector3 t) {
            return new Vector3(Mathf.LerpUnclamped(min.x, max.x, t.x), Mathf.LerpUnclamped(min.y, max.y, t.y), Mathf.LerpUnclamped(min.z, max.z, t.z));
        }

    }

    public static partial class Vector3IntUtils {

        public static Vector3Int Clamp(Vector3Int value, Vector3Int min, Vector3Int max) {
            return Vector3Int.Max(Vector3Int.Min(value, max), min);
        }

        public static Vector3Int Forward() {
            return new Vector3Int(0, 0, 1);
        }
        public static Vector3Int Back() {
            return new Vector3Int(0, 0, -1);
        }

    }

    public enum DiagonalRasterizationType {
        GoStraight,
        GoThruOneDirectNeighbor,
        GoThruAllDirectNeighbors
    }

}
