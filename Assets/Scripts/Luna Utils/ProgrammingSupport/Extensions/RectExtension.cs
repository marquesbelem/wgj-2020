using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GalloUtils {

    public static partial class RectExtension {

        public static Rect WithWidth(this Rect rect, float value, bool preserveAspect = false) {
            float aspect = rect.Aspect();
            rect.width = value;
            if (preserveAspect) {
                rect.height = rect.width / aspect;
            }
            return rect;
        }
        public static Rect WithWidth(this Rect rect, Func<float, float> operation, bool preserveAspect = false) {
            return rect.WithWidth(operation.Invoke(rect.width), preserveAspect);
        }
        public static Rect WithHeight(this Rect rect, float value, bool preserveAspect = false) {
            float aspect = rect.Aspect();
            rect.height = value;
            if (preserveAspect) {
                rect.width = rect.height * aspect;
            }
            return rect;
        }
        public static Rect WithHeight(this Rect rect, Func<float, float> operation, bool preserveAspect = false) {
            return rect.WithHeight(operation.Invoke(rect.height), preserveAspect);
        }
        public static Rect WithSize(this Rect rect, Vector2 value) {
            rect.size = value;
            return rect;
        }
        public static Rect WithSize(this Rect rect, Func<Vector2, Vector2> operation) {
            rect.size = operation.Invoke(rect.size);
            return rect;
        }

        public static Rect WithPosX(this Rect rect, float value) {
            rect.position = rect.position.WithX(value);
            return rect;
        }
        public static Rect WithPosX(this Rect rect, Func<float, float> operation) {
            rect.position = rect.position.WithX(operation.Invoke(rect.position.x));
            return rect;
        }
        public static Rect WithPosY(this Rect rect, float value) {
            rect.position = rect.position.WithY(value);
            return rect;
        }
        public static Rect WithPosY(this Rect rect, Func<float, float> operation) {
            rect.position = rect.position.WithY(operation.Invoke(rect.position.y));
            return rect;
        }
        public static Rect WithPos(this Rect rect, Vector2 value) {
            rect.position = value;
            return rect;
        }
        public static Rect WithPos(this Rect rect, Func<Vector2, Vector2> operation) {
            rect.position = operation.Invoke(rect.position);
            return rect;
        }

        public static Rect WithMaxX(this Rect rect, float value) {
            rect.max = rect.max.WithX(value);
            return rect;
        }
        public static Rect WithMaxX(this Rect rect, Func<float, float> operation) {
            rect.max = rect.max.WithX(operation.Invoke(rect.max.x));
            return rect;
        }
        public static Rect WithMaxY(this Rect rect, float value) {
            rect.max = rect.max.WithY(value);
            return rect;
        }
        public static Rect WithMaxY(this Rect rect, Func<float, float> operation) {
            rect.max = rect.max.WithY(operation.Invoke(rect.max.y));
            return rect;
        }
        public static Rect WithMax(this Rect rect, Vector2 value) {
            rect.max = value;
            return rect;
        }
        public static Rect WithMax(this Rect rect, Func<Vector2, Vector2> operation) {
            rect.max = operation.Invoke(rect.max);
            return rect;
        }

        public static Rect WithMinX(this Rect rect, float value) {
            rect.min = rect.min.WithX(value);
            return rect;
        }
        public static Rect WithMinX(this Rect rect, Func<float, float> operation) {
            rect.min = rect.min.WithX(operation.Invoke(rect.min.x));
            return rect;
        }
        public static Rect WithMinY(this Rect rect, float value) {
            rect.min = rect.min.WithY(value);
            return rect;
        }
        public static Rect WithMinY(this Rect rect, Func<float, float> operation) {
            rect.min = rect.min.WithY(operation.Invoke(rect.min.y));
            return rect;
        }
        public static Rect WithMin(this Rect rect, Vector2 value) {
            rect.min = value;
            return rect;
        }
        public static Rect WithMin(this Rect rect, Func<Vector2, Vector2> operation) {
            rect.min = operation.Invoke(rect.min);
            return rect;
        }


        public static Rect WithInvertedX(this Rect rect) => rect.WithPosX(p => -p).WithWidth(p => -p);
        public static Rect WithInvertedY(this Rect rect) => rect.WithPosY(p => -p).WithHeight(p => -p);
        public static Rect Inverted(this Rect rect) => rect.WithInvertedX().WithInvertedY();

        public static Rect Inset(this Rect rect, float thickness) => rect.WithMin(v => v += thickness.ToVector2()).WithMax(v => v -= thickness.ToVector2());
        public static Rect Outset(this Rect rect, float thickness) => rect.WithMin(v => v -= thickness.ToVector2()).WithMax(v => v += thickness.ToVector2());

        public static float Aspect(this Rect rect) => rect.size.x / rect.size.y;
        public static Rect FitTo(this Rect r1, Rect r2, RectFittingMode fittingMode) {
            Rect result = new Rect(r1);
            switch (fittingMode) {
                case RectFittingMode.Fit:
                    if (r1.Aspect() < r2.Aspect()) {
                        result = result.WithHeight(r2.height, true);
                    }
                    else {
                        result = result.WithWidth(r2.width, true);
                    }
                    break;
                case RectFittingMode.Envelope:
                    if (r1.Aspect() < r2.Aspect()) {
                        result = result.WithWidth(r2.width, true);
                    }
                    else {
                        result = result.WithHeight(r2.height, true);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public static float DistanceTo(this Rect r, Rect other) => RectUtils.Distance(r, other);

        public static Range XRange(this Rect r) => new Range(r.position.x, r.size.x);
        public static Range YRange(this Rect r) => new Range(r.position.y, r.size.y);

        public static Vector2 RandomPointInside(this Rect r) => new Vector2(r.XRange().RandomValue(), r.YRange().RandomValue());

    }
    public static partial class RectIntExtension {

        public static RectInt Clone(this RectInt rect) => new RectInt(rect.position, rect.size);

        public static RectInt WithWidth(this RectInt rect, int value, bool preserveAspect = false) {
            int aspect = rect.Aspect();
            rect.width = value;
            if (preserveAspect) {
                rect.height = rect.width / aspect;
            }
            return rect;
        }
        public static RectInt WithWidth(this RectInt rect, Func<int, int> operation, bool preserveAspect = false) {
            return rect.WithWidth(operation.Invoke(rect.width), preserveAspect);
        }
        public static RectInt WithHeight(this RectInt rect, int value, bool preserveAspect = false) {
            int aspect = rect.Aspect();
            rect.height = value;
            if (preserveAspect) {
                rect.width = rect.height * aspect;
            }
            return rect;
        }
        public static RectInt WithHeight(this RectInt rect, Func<int, int> operation, bool preserveAspect = false) {
            return rect.WithHeight(operation.Invoke(rect.height), preserveAspect);
        }
        public static RectInt WithSize(this RectInt rect, Vector2Int value) {
            rect.size = value;
            return rect;
        }
        public static RectInt WithSize(this RectInt rect, Func<Vector2Int, Vector2Int> operation) {
            rect.size = operation.Invoke(rect.size);
            return rect;
        }

        public static RectInt WithPosX(this RectInt rect, int value) {
            rect.position = rect.position.WithX(value);
            return rect;
        }
        public static RectInt WithPosX(this RectInt rect, Func<int, int> operation) {
            rect.position = rect.position.WithX(operation.Invoke);
            return rect;
        }
        public static RectInt WithPosY(this RectInt rect, int value) {
            rect.position = rect.position.WithY(value);
            return rect;
        }
        public static RectInt WithPosY(this RectInt rect, Func<int, int> operation) {
            rect.position = rect.position.WithY(operation.Invoke);
            return rect;
        }
        public static RectInt WithPos(this RectInt rect, Vector2Int value) {
            rect.position = value;
            return rect;
        }
        public static RectInt WithPos(this RectInt rect, Func<Vector2Int, Vector2Int> operation) {
            rect.position = operation.Invoke(rect.position);
            return rect;
        }

        public static RectInt WithMaxX(this RectInt rect, int value) {
            rect.max = rect.max.WithX(value);
            return rect;
        }
        public static RectInt WithMaxX(this RectInt rect, Func<int, int> operation) {
            rect.max = rect.max.WithX(operation.Invoke);
            return rect;
        }
        public static RectInt WithMaxY(this RectInt rect, int value) {
            rect.max = rect.max.WithY(value);
            return rect;
        }
        public static RectInt WithMaxY(this RectInt rect, Func<int, int> operation) {
            rect.max = rect.max.WithY(operation.Invoke);
            return rect;
        }
        public static RectInt WithMax(this RectInt rect, Vector2Int value) {
            rect.max = value;
            return rect;
        }
        public static RectInt WithMax(this RectInt rect, Func<Vector2Int, Vector2Int> operation) {
            rect.max = operation.Invoke(rect.max);
            return rect;
        }

        public static RectInt WithMinX(this RectInt rect, int value) {
            rect.min = rect.min.WithX(value);
            return rect;
        }
        public static RectInt WithMinX(this RectInt rect, Func<int, int> operation) {
            rect.min = rect.min.WithX(operation.Invoke);
            return rect;
        }
        public static RectInt WithMinY(this RectInt rect, int value) {
            rect.min = rect.min.WithY(value);
            return rect;
        }
        public static RectInt WithMinY(this RectInt rect, Func<int, int> operation) {
            rect.min = rect.min.WithY(operation.Invoke);
            return rect;
        }
        public static RectInt WithMin(this RectInt rect, Vector2Int value) {
            rect.min = value;
            return rect;
        }
        public static RectInt WithMin(this RectInt rect, Func<Vector2Int, Vector2Int> operation) {
            rect.min = operation.Invoke(rect.min);
            return rect;
        }


        public static RectInt WithInvertedX(this RectInt rect) => rect.WithPosX(p => -p).WithWidth(p => -p);
        public static RectInt WithInvertedY(this RectInt rect) => rect.WithPosY(p => -p).WithHeight(p => -p);
        public static RectInt Inverted(this RectInt rect) => rect.WithInvertedX().WithInvertedY();

        public static RectInt Inset(this RectInt rect, int thickness) => rect.WithMin(v => v += thickness.ToVector2()).WithMax(v => v -= thickness.ToVector2());
        public static RectInt Outset(this RectInt rect, int thickness) => rect.WithMin(v => v -= thickness.ToVector2()).WithMax(v => v += thickness.ToVector2());

        public static int Aspect(this RectInt rect) => rect.size.x / rect.size.y;
        public static RectInt FitTo(this RectInt r1, RectInt r2, RectFittingMode fittingMode) {
            RectInt result = r1.Clone();
            switch (fittingMode) {
                case RectFittingMode.Fit:
                    if (r1.Aspect() < r2.Aspect()) {
                        result = result.WithHeight(r2.height, true);
                    }
                    else {
                        result = result.WithWidth(r2.width, true);
                    }
                    break;
                case RectFittingMode.Envelope:
                    if (r1.Aspect() < r2.Aspect()) {
                        result = result.WithWidth(r2.width, true);
                    }
                    else {
                        result = result.WithHeight(r2.height, true);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public static float DistanceTo(this RectInt r, RectInt other) => RectIntUtils.Distance(r, other);

        public static RangeInt XRange(this RectInt r) => new RangeInt(r.position.x, r.size.x);
        public static RangeInt YRange(this RectInt r) => new RangeInt(r.position.y, r.size.y);

        public static Vector2Int RandomPointInside(this RectInt r) => new Vector2Int(r.XRange().RandomValue(), r.YRange().RandomValue());

        public static Rect ToFloat(this RectInt r) => new Rect(r.position, r.size);

        public static List<Vector2Int> PointList(this RectInt rect) {
            List<Vector2Int> result = new List<Vector2Int>();
            foreach(Vector2Int p in rect.allPositionsWithin) {
                result.Add(p);
            }
            return result;
        }
        public static List<Vector2Int> BoundPoints(this RectInt rect) {
            List<Vector2Int> result = new List<Vector2Int>();
            foreach (Vector2Int p in rect.allPositionsWithin) {
                if (p.x == rect.xMin || p.x == rect.xMax || p.y == rect.yMin || p.y == rect.yMax) {
                    result.Add(p);
                }
            }
            return result;
        }

    }
    public static partial class Rect3Extension {

        public static Rect3 WithWidth(this Rect3 rect, float value, bool preserveAspect = false) {
            Tuple<float, float, float> aspect = rect.Aspect();
            rect.Width = value;
            if (preserveAspect) {
                rect.Height = rect.Width / aspect.Item1;
                rect.Depth = rect.Width / aspect.Item2;
            }
            return rect;
        }
        public static Rect3 WithWidth(this Rect3 rect, Func<float, float> operation, bool preserveAspect = false) {
            return rect.WithWidth(operation.Invoke(rect.Width), preserveAspect);
        }
        public static Rect3 WithHeight(this Rect3 rect, float value, bool preserveAspect = false) {
            Tuple<float, float, float> aspect = rect.Aspect();
            rect.Height = value;
            if (preserveAspect) {
                rect.Width = rect.Height * aspect.Item1;
                rect.Depth = rect.Width / aspect.Item2;
            }
            return rect;
        }
        public static Rect3 WithHeight(this Rect3 rect, Func<float, float> operation, bool preserveAspect = false) {
            return rect.WithHeight(operation.Invoke(rect.Height), preserveAspect);
        }
        public static Rect3 WithDepth(this Rect3 rect, float value, bool preserveAspect = false) {
            Tuple<float, float, float> aspect = rect.Aspect();
            rect.Depth = value;
            if (preserveAspect) {
                rect.Width = rect.Depth * aspect.Item2;
                rect.Height = rect.Width / aspect.Item1;
            }
            return rect;
        }
        public static Rect3 WithDepth(this Rect3 rect, Func<float, float> operation, bool preserveAspect = false) {
            return rect.WithHeight(operation.Invoke(rect.Depth), preserveAspect);
        }
        public static Rect3 WithSize(this Rect3 rect, Vector3 value) {
            rect.size = value;
            return rect;
        }
        public static Rect3 WithSize(this Rect3 rect, Func<Vector3, Vector3> operation) {
            rect.size = operation.Invoke(rect.size);
            return rect;
        }

        public static Rect3 WithPosX(this Rect3 rect, float value) {
            rect.position = rect.position.WithX(value);
            return rect;
        }
        public static Rect3 WithPosX(this Rect3 rect, Func<float, float> operation) {
            rect.position = rect.position.WithX(operation.Invoke(rect.position.x));
            return rect;
        }
        public static Rect3 WithPosY(this Rect3 rect, float value) {
            rect.position = rect.position.WithY(value);
            return rect;
        }
        public static Rect3 WithPosY(this Rect3 rect, Func<float, float> operation) {
            rect.position = rect.position.WithY(operation.Invoke(rect.position.y));
            return rect;
        }
        public static Rect3 WithPosZ(this Rect3 rect, float value) {
            rect.position = rect.position.WithZ(value);
            return rect;
        }
        public static Rect3 WithPosZ(this Rect3 rect, Func<float, float> operation) {
            rect.position = rect.position.WithZ(operation.Invoke(rect.position.z));
            return rect;
        }
        public static Rect3 WithPos(this Rect3 rect, Vector3 value) {
            rect.position = value;
            return rect;
        }
        public static Rect3 WithPos(this Rect3 rect, Func<Vector3, Vector3> operation) {
            rect.position = operation.Invoke(rect.position);
            return rect;
        }

        public static Rect3 WithMaxX(this Rect3 rect, float value) {
            rect.Max = rect.Max.WithX(value);
            return rect;
        }
        public static Rect3 WithMaxX(this Rect3 rect, Func<float, float> operation) {
            rect.Max = rect.Max.WithX(operation.Invoke(rect.Max.x));
            return rect;
        }
        public static Rect3 WithMaxY(this Rect3 rect, float value) {
            rect.Max = rect.Max.WithY(value);
            return rect;
        }
        public static Rect3 WithMaxY(this Rect3 rect, Func<float, float> operation) {
            rect.Max = rect.Max.WithY(operation.Invoke(rect.Max.y));
            return rect;
        }
        public static Rect3 WithMaxZ(this Rect3 rect, float value) {
            rect.Max = rect.Max.WithZ(value);
            return rect;
        }
        public static Rect3 WithMaxZ(this Rect3 rect, Func<float, float> operation) {
            rect.Max = rect.Max.WithZ(operation.Invoke(rect.Max.z));
            return rect;
        }
        public static Rect3 WithMax(this Rect3 rect, Vector3 value) {
            rect.Max = value;
            return rect;
        }
        public static Rect3 WithMax(this Rect3 rect, Func<Vector3, Vector3> operation) {
            rect.Max = operation.Invoke(rect.Max);
            return rect;
        }

        public static Rect3 WithMinX(this Rect3 rect, float value) {
            rect.Min = rect.Min.WithX(value);
            return rect;
        }
        public static Rect3 WithMinX(this Rect3 rect, Func<float, float> operation) {
            rect.Min = rect.Min.WithX(operation.Invoke(rect.Min.x));
            return rect;
        }
        public static Rect3 WithMinY(this Rect3 rect, float value) {
            rect.Min = rect.Min.WithY(value);
            return rect;
        }
        public static Rect3 WithMinY(this Rect3 rect, Func<float, float> operation) {
            rect.Min = rect.Min.WithY(operation.Invoke(rect.Min.y));
            return rect;
        }
        public static Rect3 WithMinZ(this Rect3 rect, float value) {
            rect.Min = rect.Min.WithZ(value);
            return rect;
        }
        public static Rect3 WithMinZ(this Rect3 rect, Func<float, float> operation) {
            rect.Min = rect.Min.WithZ(operation.Invoke(rect.Min.z));
            return rect;
        }
        public static Rect3 WithMin(this Rect3 rect, Vector3 value) {
            rect.Min = value;
            return rect;
        }
        public static Rect3 WithMin(this Rect3 rect, Func<Vector3, Vector3> operation) {
            rect.Min = operation.Invoke(rect.Min);
            return rect;
        }


        public static Rect3 WithInvertedX(this Rect3 rect) {
            return rect.WithPosX(p => -p).WithWidth(p => -p);
        }
        public static Rect3 WithInvertedY(this Rect3 rect) {
            return rect.WithPosY(p => -p).WithHeight(p => -p);
        }
        public static Rect3 WithInvertedZ(this Rect3 rect) {
            return rect.WithPosZ(p => -p).WithDepth(p => -p);
        }
        public static Rect3 Inverted(this Rect3 rect) {
            return rect.WithInvertedX().WithInvertedY().WithInvertedZ();
        }

        public static Rect3 Inset(this Rect3 rect, float thickness) {
            return rect.WithMin(v => v += thickness.ToVector3()).WithMax(v => v -= thickness.ToVector3());
        }
        public static Rect3 Outset(this Rect3 rect, float thickness) {
            return rect.WithMin(v => v -= thickness.ToVector3()).WithMax(v => v += thickness.ToVector3());
        }

        public static Tuple<float, float, float> Aspect(this Rect3 rect) {
            return new Tuple<float, float, float>(rect.size.x / rect.size.y, rect.size.x / rect.size.z, rect.size.y / rect.size.z);
        }
        public static Rect3 FitTo(this Rect3 r1, Rect3 r2, RectFittingMode fittingMode) {
            Rect3 result = new Rect3(r1);
            Tuple<float, float, float> aspect1 = r1.Aspect();
            Tuple<float, float, float> aspect2 = r2.Aspect();
            switch (fittingMode) {
                case RectFittingMode.Fit:
                    if (aspect1.Item1 > aspect2.Item1 && aspect1.Item2 > aspect2.Item2) {
                        result = result.WithWidth(r2.Width, true);
                    }
                    else if (aspect1.Item1 < aspect2.Item1 && aspect1.Item3 > aspect2.Item3) {
                        result = result.WithHeight(r2.Height, true);
                    }
                    else {
                        result = result.WithDepth(r2.Depth, true);
                    }
                    break;
                case RectFittingMode.Envelope:
                    if (aspect1.Item1 < aspect2.Item1 && aspect1.Item2 < aspect2.Item2) {
                        result = result.WithWidth(r2.Width, true);
                    }
                    else if (aspect1.Item1 > aspect2.Item1 && aspect1.Item3 < aspect2.Item3) {
                        result = result.WithHeight(r2.Height, true);
                    }
                    else {
                        result = result.WithDepth(r2.Depth, true);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public static float DistanceTo(this Rect3 r, Rect3 other) {
            return Rect3Utils.Distance(r, other);
        }

    }
    public static partial class Rect3IntExtension {

        public static Rect3Int Clone(this Rect3Int rect) {
            return new Rect3Int(rect.position, rect.size);
        }

        public static Rect3Int WithWidth(this Rect3Int rect, int value, bool preserveAspect = false) {
            Tuple<int, int, int> aspect = rect.Aspect();
            rect.Width = value;
            if (preserveAspect) {
                rect.Height = rect.Width / aspect.Item1;
                rect.Depth = rect.Width / aspect.Item2;
            }
            return rect;
        }
        public static Rect3Int WithWidth(this Rect3Int rect, Func<int, int> operation, bool preserveAspect = false) {
            return rect.WithWidth(operation.Invoke(rect.Width), preserveAspect);
        }
        public static Rect3Int WithHeight(this Rect3Int rect, int value, bool preserveAspect = false) {
            Tuple<int, int, int> aspect = rect.Aspect();
            rect.Height = value;
            if (preserveAspect) {
                rect.Width = rect.Height * aspect.Item1;
                rect.Depth = rect.Width / aspect.Item2;
            }
            return rect;
        }
        public static Rect3Int WithHeight(this Rect3Int rect, Func<int, int> operation, bool preserveAspect = false) {
            return rect.WithHeight(operation.Invoke(rect.Height), preserveAspect);
        }
        public static Rect3Int WithDepth(this Rect3Int rect, int value, bool preserveAspect = false) {
            Tuple<int, int, int> aspect = rect.Aspect();
            rect.Depth = value;
            if (preserveAspect) {
                rect.Width = rect.Depth * aspect.Item2;
                rect.Height = rect.Width / aspect.Item1;
            }
            return rect;
        }
        public static Rect3Int WithDepth(this Rect3Int rect, Func<int, int> operation, bool preserveAspect = false) {
            return rect.WithHeight(operation.Invoke(rect.Depth), preserveAspect);
        }
        public static Rect3Int WithSize(this Rect3Int rect, Vector3Int value) {
            rect.size = value;
            return rect;
        }
        public static Rect3Int WithSize(this Rect3Int rect, Func<Vector3Int, Vector3Int> operation) {
            rect.size = operation.Invoke(rect.size);
            return rect;
        }

        public static Rect3Int WithPosX(this Rect3Int rect, int value) {
            rect.position = rect.position.WithX(value);
            return rect;
        }
        public static Rect3Int WithPosX(this Rect3Int rect, Func<int, int> operation) {
            rect.position = rect.position.WithX(operation.Invoke(rect.position.x));
            return rect;
        }
        public static Rect3Int WithPosY(this Rect3Int rect, int value) {
            rect.position = rect.position.WithY(value);
            return rect;
        }
        public static Rect3Int WithPosY(this Rect3Int rect, Func<int, int> operation) {
            rect.position = rect.position.WithY(operation.Invoke(rect.position.y));
            return rect;
        }
        public static Rect3Int WithPosZ(this Rect3Int rect, int value) {
            rect.position = rect.position.WithZ(value);
            return rect;
        }
        public static Rect3Int WithPosZ(this Rect3Int rect, Func<int, int> operation) {
            rect.position = rect.position.WithZ(operation.Invoke(rect.position.z));
            return rect;
        }
        public static Rect3Int WithPos(this Rect3Int rect, Vector3Int value) {
            rect.position = value;
            return rect;
        }
        public static Rect3Int WithPos(this Rect3Int rect, Func<Vector3Int, Vector3Int> operation) {
            rect.position = operation.Invoke(rect.position);
            return rect;
        }

        public static Rect3Int WithMaxX(this Rect3Int rect, int value) {
            rect.Max = rect.Max.WithX(value);
            return rect;
        }
        public static Rect3Int WithMaxX(this Rect3Int rect, Func<int, int> operation) {
            rect.Max = rect.Max.WithX(operation.Invoke(rect.Max.x));
            return rect;
        }
        public static Rect3Int WithMaxY(this Rect3Int rect, int value) {
            rect.Max = rect.Max.WithY(value);
            return rect;
        }
        public static Rect3Int WithMaxY(this Rect3Int rect, Func<int, int> operation) {
            rect.Max = rect.Max.WithY(operation.Invoke(rect.Max.y));
            return rect;
        }
        public static Rect3Int WithMaxZ(this Rect3Int rect, int value) {
            rect.Max = rect.Max.WithZ(value);
            return rect;
        }
        public static Rect3Int WithMaxZ(this Rect3Int rect, Func<int, int> operation) {
            rect.Max = rect.Max.WithZ(operation.Invoke(rect.Max.z));
            return rect;
        }
        public static Rect3Int WithMax(this Rect3Int rect, Vector3Int value) {
            rect.Max = value;
            return rect;
        }
        public static Rect3Int WithMax(this Rect3Int rect, Func<Vector3Int, Vector3Int> operation) {
            rect.Max = operation.Invoke(rect.Max);
            return rect;
        }

        public static Rect3Int WithMinX(this Rect3Int rect, int value) {
            rect.Min = rect.Min.WithX(value);
            return rect;
        }
        public static Rect3Int WithMinX(this Rect3Int rect, Func<int, int> operation) {
            rect.Min = rect.Min.WithX(operation.Invoke(rect.Min.x));
            return rect;
        }
        public static Rect3Int WithMinY(this Rect3Int rect, int value) {
            rect.Min = rect.Min.WithY(value);
            return rect;
        }
        public static Rect3Int WithMinY(this Rect3Int rect, Func<int, int> operation) {
            rect.Min = rect.Min.WithY(operation.Invoke(rect.Min.y));
            return rect;
        }
        public static Rect3Int WithMinZ(this Rect3Int rect, int value) {
            rect.Min = rect.Min.WithZ(value);
            return rect;
        }
        public static Rect3Int WithMinZ(this Rect3Int rect, Func<int, int> operation) {
            rect.Min = rect.Min.WithZ(operation.Invoke(rect.Min.z));
            return rect;
        }
        public static Rect3Int WithMin(this Rect3Int rect, Vector3Int value) {
            rect.Min = value;
            return rect;
        }
        public static Rect3Int WithMin(this Rect3Int rect, Func<Vector3Int, Vector3Int> operation) {
            rect.Min = operation.Invoke(rect.Min);
            return rect;
        }


        public static Rect3Int WithInvertedX(this Rect3Int rect) {
            return rect.WithPosX(p => -p).WithWidth(p => -p);
        }
        public static Rect3Int WithInvertedY(this Rect3Int rect) {
            return rect.WithPosY(p => -p).WithHeight(p => -p);
        }
        public static Rect3Int WithInvertedZ(this Rect3Int rect) {
            return rect.WithPosZ(p => -p).WithDepth(p => -p);
        }
        public static Rect3Int Inverted(this Rect3Int rect) {
            return rect.WithInvertedX().WithInvertedY().WithInvertedZ();
        }

        public static Rect3Int Inset(this Rect3Int rect, int thickness) {
            return rect.WithMin(v => v += thickness.ToVector3()).WithMax(v => v -= thickness.ToVector3());
        }
        public static Rect3Int Outset(this Rect3Int rect, int thickness) {
            return rect.WithMin(v => v -= thickness.ToVector3()).WithMax(v => v += thickness.ToVector3());
        }

        public static Tuple<int, int, int> Aspect(this Rect3Int rect) {
            return new Tuple<int, int, int>(rect.size.x / rect.size.y, rect.size.x / rect.size.z, rect.size.y / rect.size.z);
        }
        public static Rect3Int FitTo(this Rect3Int r1, Rect3Int r2, RectFittingMode fittingMode) {
            Rect3Int result = r1.Clone();
            Tuple<int, int, int> aspect1 = r1.Aspect();
            Tuple<int, int, int> aspect2 = r2.Aspect();
            switch (fittingMode) {
                case RectFittingMode.Fit:
                    if (aspect1.Item1 > aspect2.Item1 && aspect1.Item2 > aspect2.Item2) {
                        result = result.WithWidth(r2.Width, true);
                    }
                    else if (aspect1.Item1 < aspect2.Item1 && aspect1.Item3 > aspect2.Item3) {
                        result = result.WithHeight(r2.Height, true);
                    }
                    else {
                        result = result.WithDepth(r2.Depth, true);
                    }
                    break;
                case RectFittingMode.Envelope:
                    if (aspect1.Item1 < aspect2.Item1 && aspect1.Item2 < aspect2.Item2) {
                        result = result.WithWidth(r2.Width, true);
                    }
                    else if (aspect1.Item1 > aspect2.Item1 && aspect1.Item3 < aspect2.Item3) {
                        result = result.WithHeight(r2.Height, true);
                    }
                    else {
                        result = result.WithDepth(r2.Depth, true);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        public static float DistanceTo(this Rect3Int r, Rect3Int other) {
            return Rect3IntUtils.Distance(r, other);
        }

        public static List<Vector3Int> PointList(this Rect3Int rect) {
            List<Vector3Int> result = new List<Vector3Int>();
            for (int i = rect.Min.x; i <= rect.Max.x; i++) {
                for (int j = rect.Min.y; j <= rect.Max.y; j++) {
                    for (int k = rect.Min.z; k <= rect.Max.z; k++) {
                        result.Add(new Vector3Int(i, j, k));
                    }
                }
            }
            return result;
        }
        
    }

    public enum RectFittingMode {
        Fit,
        Envelope
    }

}