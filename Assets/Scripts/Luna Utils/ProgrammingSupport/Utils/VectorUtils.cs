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
        public static Vector2 InverseIterpolateUnclamped(Vector2 min, Vector2 max, Vector2 value) {
            return new Vector2(FloatUtils.InverseLerpUnclamped(min.x, max.x, value.x), FloatUtils.InverseLerpUnclamped(min.y, max.y, value.y));
        }
        public static Vector2 RemapUnclamped(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value) {
            return IterpolateUnclamped(oMin, oMax, InverseIterpolateUnclamped(iMin, iMax, value));
        }
        public static Vector2 RemapUnclamped(float iMin, float iMax, Vector2 oMin, Vector2 oMax, float value) {
            return Vector2.LerpUnclamped(oMin, oMax, FloatUtils.InverseLerpUnclamped(iMin, iMax, value));
        }

    }

    public static partial class Vector2IntUtils {

        public static Vector2Int Clamp(Vector2Int value, Vector2Int min, Vector2Int max) {
            return Vector2Int.Max(Vector2Int.Min(value, max), min);
        }

        public static bool AreNeighbors(Vector2Int first, Vector2Int second, bool includeDiagonals = false) {
            return first.Neighbors(includeDiagonals).Contains(second);
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

        public static List<Vector2Int> RasterizeLine(Vector2Int from, Vector2Int to, int thickness) {
            List<Vector2Int> result = new List<Vector2Int>();
            RasterizeLine(from, to).ForEach(v => result.AddRangeIfDoesntContain(RasterizeCircle(thickness, v)));
            return result;
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

        public static List<Vector2Int> RasterizeFloatingLine(Vector2 from, Vector2 to, DiagonalRasterizationType diagonalRasterizationType) {
            List<Vector2Int> result = new List<Vector2Int>();
            Vector2Int fromInt = from.RoundedToInt();
            Vector2Int toInt = to.RoundedToInt();
            Vector2Int intMin = Min(fromInt, toInt);
            Vector2Int intMax = Max(fromInt, toInt);
            Vector2Int sign = new Vector2Int((toInt.x - fromInt.x).Sign(), (toInt.y - fromInt.y).Sign());
            Vector2 delta = to - from;
            Vector2Int intSize = intMax - intMin;
            if (fromInt == toInt) {
                result.Add(fromInt);
            }
            else if (fromInt.x == toInt.x) {
                for (int i = 0; i <= intSize.y; i++) {
                    result.Add(new Vector2Int(fromInt.x, intMin.x + i));
                }
            }
            else if (fromInt.y == toInt.y) {
                for (int i = 0; i <= intSize.x; i++) {
                    result.Add(new Vector2Int(intMin.x + i, fromInt.y));
                }
            }
            else {
                List<float> verticalLineIntersectionDists = CollectionUtils.CreateListByIndex(intSize.x - 1, index => GeometryUtils.VerticalLine(from.x + (index + 0.5f) * sign.x, from.y, to.y)).ConvertAll(l => from.DistanceTo(GeometryUtils.LineLineIntersection(l[0], l[1], from, to).Value));
                List<float> horizontalLineIntersectionDists = CollectionUtils.CreateListByIndex(intSize.y - 1, index => GeometryUtils.HorizontalLine(from.y + (index + 0.5f) * sign.y, from.x, to.x)).ConvertAll(l => from.DistanceTo(GeometryUtils.LineLineIntersection(l[0], l[1], from, to).Value));
                Vector2Int i = Vector2Int.zero;
                result.Add(fromInt);
                while (i.x < intSize.x - 1 || i.y < intSize.y - 1) {
                    bool moveOnX = false;
                    bool moveOnY = false;
                    if (i.x == intSize.x - 1) {
                        moveOnY = true;
                    }
                    else if (i.y == intSize.y - 1) {
                        moveOnX = true;
                    }
                    else {
                        float nextVertical = verticalLineIntersectionDists[i.x];
                        float nextHorizontal = horizontalLineIntersectionDists[i.y];
                        if (nextVertical < nextHorizontal) {
                            moveOnX = true;
                        }
                        else if (nextVertical > nextHorizontal) {
                            moveOnY = true;
                        }
                        else {
                            switch (diagonalRasterizationType) {
                                case DiagonalRasterizationType.GoStraight:
                                    moveOnX = true;
                                    moveOnY = true;
                                    break;
                                case DiagonalRasterizationType.GoThruOneDirectNeighbor:
                                    moveOnX = true;
                                    moveOnY = false;
                                    break;
                                case DiagonalRasterizationType.GoThruAllDirectNeighbors:
                                    result.Add(fromInt + new Vector2Int(i.x * sign.x, i.y * sign.y) + Vector2Int.up);
                                    moveOnX = true;
                                    moveOnY = false;
                                    break;
                            }
                        }
                    }
                    if (moveOnX) {
                        i += Vector2Int.right * sign.x;
                    }
                    if (moveOnY) {
                        i += Vector2Int.up * sign.y;
                    }
                    result.Add(fromInt + new Vector2Int(i.x * sign.x, i.y * sign.y));
                }
            }
            return result;
        }

        public static List<Vector2Int> RasterizeCircle(int radius, Vector2Int center) {
            return RasterizeCircle(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircle(int radius) {
            List<Vector2Int> result = new List<Vector2Int>(RasterizeCircumference(radius));
            result.AddRange(GetPointIsland(Vector2Int.zero, result));
            return result;
        }

        public static List<Vector2Int> RasterizeCircumference(int radius, Vector2Int center, float startAngle, float finishAngle) {
            return RasterizeCircumference(radius, startAngle, finishAngle).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumference(int radius, float startAngle, float finishAngle) {
            List<Vector2Int> result = new List<Vector2Int>();

            int startFullOctant = Mathf.CeilToInt(startAngle / 45f);
            int finishFullOctant = Mathf.FloorToInt(finishAngle / 45f) - 1;
            float leftoverStartAngle = startFullOctant * 45f - startAngle;
            float leftoverFinishAngle = finishAngle - finishFullOctant * 45f;

            if (startFullOctant - 1 <= finishFullOctant) {
                if (leftoverStartAngle > 0f) {
                    result.AddRange(RasterizeCircumferencePartialOctant(radius, startFullOctant - 1, startAngle, startFullOctant * 45f));
                }
                result.AddRangeIfDoesntContain(RasterizeCircumferenceOctants(radius, startFullOctant, finishFullOctant));
                if (leftoverFinishAngle > 0f) {
                    result.AddRangeIfDoesntContain(RasterizeCircumferencePartialOctant(radius, finishFullOctant + 1, finishFullOctant * 45f, finishAngle));
                }
            }
            else {
                result.AddRange(RasterizeCircumferencePartialOctant(radius, startFullOctant - 1, startAngle, finishAngle));
            }

            return result;
        }
        public static List<Vector2Int> RasterizeCircumference(int radius, Vector2Int center) {
            return RasterizeCircumference(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumference(int radius) {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> semicircumference = RasterizeSemicircumference(radius);
            result.AddRange(semicircumference);
            semicircumference.SetEachElement(v => v.RotatedBy90Clockwise());
            result.AddRangeIfDoesntContain(semicircumference);
            return result;
        }

        public static List<Vector2Int> RasterizeSemicircumference(int radius, Vector2Int center) {
            return RasterizeSemicircumference(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeSemicircumference(int radius) {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> quadrant = RasterizeCircumferenceQuadrant(radius);
            result.AddRange(quadrant);
            quadrant.SetEachElement(v => v.RotatedBy90Clockwise());
            result.AddRangeIfDoesntContain(quadrant);
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferenceQuadrant(int radius, Vector2Int center) {
            return RasterizeCircumferenceQuadrant(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumferenceQuadrant(int radius) {
            List<Vector2Int> result = new List<Vector2Int>();
            List<Vector2Int> octant = RasterizeCircumferenceOctant(radius);
            result.AddRange(octant);
            octant.SetEachElement(v => v.WithInvertedX().RotatedBy90Clockwise());
            result.AddRangeIfDoesntContain(octant);
            return result;
        }

        public static List<Vector2Int> RasterizeCircumferenceOctants(int radius, int startOctant, int finishOctant) {
            List<Vector2Int> result = RasterizeCircumferenceOctant(radius);
            for (int i = startOctant; i <= finishOctant; i++) {
                result.AddRangeIfDoesntContain(RasterizeCircumferenceOctant(radius, i));
            }
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferenceOctant(int radius, int octantIndex) {
            List<Vector2Int> result = RasterizeCircumferenceOctant(radius);
            if (octantIndex % 2 == 1) {
                result.SetEachElement(v => v.WithInvertedX().RotatedBy90Clockwise());
            }
            result.SetEachElement(v => v.RotatedBy90Clockwise(octantIndex / 2));
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferenceOctant(int radius, Vector2Int center) {
            return RasterizeCircumferenceOctant(radius).ThruFuncElement(v => v + center);
        }
        public static List<Vector2Int> RasterizeCircumferenceOctant(int radius) {
            return RasterizeCircumferencePartialOctant(radius, 0f, 45f);
        }

        public static List<Vector2Int> RasterizeCircumferencePartialOctant(int radius, int octantIndex, float startAngle, float finishAngle) {
            List<Vector2Int> result;
            float octantAngle = octantIndex * 45f;
            if (octantIndex % 2 == 1) {
                result = RasterizeCircumferencePartialOctant(radius, finishAngle - octantAngle, startAngle - octantAngle);
                result.SetEachElement(v => v.WithInvertedX().RotatedBy90Clockwise());
            }
            else {
                result = RasterizeCircumferencePartialOctant(radius, startAngle - octantAngle, finishAngle - octantAngle);
            }
            result.SetEachElement(v => v.RotatedBy90Clockwise(octantIndex / 2));
            return result;
        }
        public static List<Vector2Int> RasterizeCircumferencePartialOctant(int radius, float startAngle, float finishAngle) {
            List<Vector2Int> result = new List<Vector2Int>();
            if (radius == 0) {
                result = new List<Vector2Int>() { Vector2Int.zero };
            }
            else {
                if (radius < 0) {
                    radius = -radius;
                }
                startAngle = Mathf.Clamp(startAngle, 0f, 45f);
                finishAngle = Mathf.Clamp(finishAngle, startAngle, 45f);
                Vector2Int startPoint = Vector2Int.RoundToInt((Vector2.up * radius).RotatedBy(startAngle));
                float curAngle = startAngle;
                int y = startPoint.y;
                int x = startPoint.x;
                while (curAngle <= finishAngle) {
                    result.Add(new Vector2Int(x, y));
                    x++;
                    float distToRadius = Mathf.Abs((new Vector2(x, y)).magnitude - radius);
                    float decDistToRadius = Mathf.Abs((new Vector2(x, y - 1)).magnitude - radius);
                    if (decDistToRadius < distToRadius) {
                        y--;
                    }
                    curAngle = Vector2.Angle(Vector2.up, new Vector2(x, y));
                }
            }
            return result;
        }

        public static List<Vector2Int> GetPointIsland(Vector2Int origin, List<Vector2Int> border) {
            return GetPointIsland(origin, v => border.Contains(v));
        }
        public static List<Vector2Int> GetPointIsland(Vector2Int origin, Predicate<Vector2Int> isBorder) {
            return GraphSolver.ReachableNodes(p => p.Neighbors(), origin, isBorder, true);
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
        public static Vector3 ClampInside(Vector3 value, Rect3 limits) {
            return ClampInside(value, limits.Min, limits.Max);
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
        public static Vector3 ClampOutside(Vector3 value, Rect3 limits) {
            return ClampOutside(value, limits.Min, limits.Max);
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
        public static Vector3 InverseIterpolateUnclamped(Vector3 min, Vector3 max, Vector3 value) {
            return new Vector3(FloatUtils.InverseLerpUnclamped(min.x, max.x, value.x), FloatUtils.InverseLerpUnclamped(min.y, max.y, value.y), FloatUtils.InverseLerpUnclamped(min.z, max.z, value.z));
        }
        public static Vector3 RemapUnclamped(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 value) {
            return IterpolateUnclamped(oMin, oMax, InverseIterpolateUnclamped(iMin, iMax, value));
        }
        public static Vector3 RemapUnclamped(float iMin, float iMax, Vector3 oMin, Vector3 oMax, float value) {
            return Vector3.LerpUnclamped(oMin, oMax, FloatUtils.InverseLerpUnclamped(iMin, iMax, value));
        }

    }

    public static partial class Vector3IntUtils {

        public static Vector3Int Clamp(Vector3Int value, Vector3Int min, Vector3Int max) {
            return Vector3Int.Max(Vector3Int.Min(value, max), min);
        }

        public static bool AreNeighbors(Vector3Int first, Vector3Int second, Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) {
            return first.Neighbors(neighboringRule).Contains(second);
        }
        public static List<Vector3Int> Neighbors(Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) {
            List<Vector3Int> result = new List<Vector3Int>();
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    for (int k = -1; k <= 1; k++) {
                        Vector3Int v = new Vector3Int(i, j, k);
                        int m = v.ManhatanMagnitude();
                        if ((m == 1 && neighboringRule.HasFlag(Neighboring3Type.SharesFace))
                            || (m == 2 && neighboringRule.HasFlag(Neighboring3Type.SharesEdge))
                            || (m == 3 && neighboringRule.HasFlag(Neighboring3Type.SharesVertex))) {
                            result.Add(v);
                        }
                    }
                }
            }
            return result;
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
