using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static class BezierCurveSolver {

        #region Generic
        public static T InterpolateQuadraticBezier<T>(T p1, T p2, T p3, float lerpValue, Func<T, T, float, T> lerpFunc) {
            T a1 = lerpFunc.Invoke(p1, p2, lerpValue);
            T a2 = lerpFunc.Invoke(p2, p3, lerpValue);
            return lerpFunc.Invoke(a1, a2, lerpValue);
        }
        public static T InterpolateCubicBezier<T>(T p1, T p2, T p3, T p4, float lerpValue, Func<T, T, float, T> lerpFunc) {
            T a1 = lerpFunc.Invoke(p1, p2, lerpValue);
            T a2 = lerpFunc.Invoke(p2, p3, lerpValue);
            T a3 = lerpFunc.Invoke(p3, p4, lerpValue);
            return InterpolateQuadraticBezier(a1, a2, a3, lerpValue, lerpFunc);
        }
        public static T InterpolateBezier<T>(float lerpValue, Func<T, T, float, T> lerpFunc, List<T> p) {
            if (p.Count == 1) {
                return p.First();
            }
            return InterpolateBezier(lerpValue, lerpFunc, p.MergedInPairs((v1, v2) => lerpFunc.Invoke(v1, v2, lerpValue)));
        }

        public static List<T> DiscretizeQuadraticBezier<T>(T p1, T p2, T p3, int subdivisions, Func<T, T, float, T> lerpFunc) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateQuadraticBezier(p1, p2, p3, (float)i / (subdivisions + 1), lerpFunc));
        }
        public static List<T> DiscretizeCubicBezier<T>(T p1, T p2, T p3, T p4, int subdivisions, Func<T, T, float, T> lerpFunc) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateCubicBezier(p1, p2, p3, p4, (float)i / (subdivisions + 1), lerpFunc));
        }
        public static List<T> DiscretizeBezier<T>(List<T> p, int subdivisions, Func<T, T, float, T> lerpFunc) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateBezier((float)i / (subdivisions + 1), lerpFunc, p));
        }

        public static List<T> DiscretizeQuadraticBezier<T>(T p1, T p2, T p3, float maxSubdivisionLength, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            int subdivisions = Mathf.CeilToInt(CollectionUtils.SumPairs(distFunc, false, false, p1, p2 ,p3) / maxSubdivisionLength);
            return DiscretizeQuadraticBezier(p1, p2, p3, subdivisions, lerpFunc);
        }
        public static List<T> DiscretizeCubicBezier<T>(T p1, T p2, T p3, T p4, float maxSubdivisionLength, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            int subdivisions = Mathf.CeilToInt(CollectionUtils.SumPairs(distFunc, false, false, p1, p2, p3, p4) / maxSubdivisionLength);
            return DiscretizeCubicBezier(p1, p2, p3, p4, subdivisions, lerpFunc);
        }
        public static List<T> DiscretizeBezier<T>(List<T> p, float maxSubdivisionLength, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            int subdivisions = Mathf.CeilToInt(p.SumPairs(distFunc) / maxSubdivisionLength);
            return DiscretizeBezier(p, subdivisions, lerpFunc);
        }

        public static float ApproximateQuadraticBezierLength<T>(T p1, T p2, T p3, int subdivisions, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            return DiscretizeQuadraticBezier(p1, p2, p3, subdivisions, lerpFunc).SumPairs(distFunc);
        }
        public static float ApproximateCubicBezierLength<T>(T p1, T p2, T p3, T p4, int subdivisions, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            return DiscretizeCubicBezier(p1, p2, p3, p4, subdivisions, lerpFunc).SumPairs(distFunc);
        }
        public static float ApproximateBezierLength<T>(List<T> p, int subdivisions, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            return DiscretizeBezier(p, subdivisions, lerpFunc).SumPairs(distFunc);
        }

        public static float ApproximateQuadraticBezierLength<T>(T p1, T p2, T p3, float maxSubdivisionLength, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            return DiscretizeQuadraticBezier(p1, p2, p3, maxSubdivisionLength, lerpFunc, distFunc).SumPairs(distFunc);
        }
        public static float ApproximateCubicBezierLength<T>(T p1, T p2, T p3, T p4, float maxSubdivisionLength, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            return DiscretizeCubicBezier(p1, p2, p3, p4, maxSubdivisionLength, lerpFunc, distFunc).SumPairs(distFunc);
        }
        public static float ApproximateBezierLength<T>(List<T> p, float maxSubdivisionLength, Func<T, T, float, T> lerpFunc, Func<T, T, float> distFunc) {
            return DiscretizeBezier(p, maxSubdivisionLength, lerpFunc, distFunc).SumPairs(distFunc);
        }
        #endregion

        #region float
        public static float InterpolateQuadraticBezier(float p1, float p2, float p3, float lerpValue) {
            float a1 = Mathf.Lerp(p1, p2, lerpValue);
            float a2 = Mathf.Lerp(p2, p3, lerpValue);
            return Mathf.Lerp(a1, a2, lerpValue);
        }
        public static float InterpolateCubicBezier(float p1, float p2, float p3, float p4, float lerpValue) {
            float a1 = Mathf.Lerp(p1, p2, lerpValue);
            float a2 = Mathf.Lerp(p2, p3, lerpValue);
            float a3 = Mathf.Lerp(p3, p4, lerpValue);
            return InterpolateQuadraticBezier(a1, a2, a3, lerpValue);
        }
        public static float InterpolateBezier(float lerpValue, List<float> p) {
            if (p.Count == 1) {
                return p.First();
            }
            return InterpolateBezier(lerpValue, p.MergedInPairs((v1, v2) => Mathf.Lerp(v1, v2, lerpValue)));
        }

        public static List<float> DiscretizeQuadraticBezier(float p1, float p2, float p3, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateQuadraticBezier(p1, p2, p3, (float)i / (subdivisions + 1)));
        }
        public static List<float> DiscretizeCubicBezier(float p1, float p2, float p3, float p4, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateCubicBezier(p1, p2, p3, p4, (float)i / (subdivisions + 1)));
        }
        public static List<float> DiscretizeBezier(List<float> p, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateBezier((float)i / (subdivisions + 1), p));
        }

        public static float ApproximateQuadraticBezierLength(float p1, float p2, float p3, int subdivisions) {
            return DiscretizeQuadraticBezier(p1, p2, p3, subdivisions).Sum();
        }
        public static float ApproximateCubicBezierLength(float p1, float p2, float p3, float p4, int subdivisions) {
            return DiscretizeCubicBezier(p1, p2, p3, p4, subdivisions).Sum();
        }
        public static float ApproximateBezierLength(List<float> p, int subdivisions) {
            return DiscretizeBezier(p, subdivisions).Sum();
        }
        #endregion

        #region Vector2
        public static Vector2 InterpolateQuadraticBezier(Vector2 p1, Vector2 p2, Vector2 p3, float lerpValue) {
            Vector2 a1 = Vector2.Lerp(p1, p2, lerpValue);
            Vector2 a2 = Vector2.Lerp(p2, p3, lerpValue);
            return Vector2.Lerp(a1, a2, lerpValue);
        }
        public static Vector2 InterpolateCubicBezier(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float lerpValue) {
            Vector2 a1 = Vector2.Lerp(p1, p2, lerpValue);
            Vector2 a2 = Vector2.Lerp(p2, p3, lerpValue);
            Vector2 a3 = Vector2.Lerp(p3, p4, lerpValue);
            return InterpolateQuadraticBezier(a1, a2, a3, lerpValue);
        }
        public static Vector2 InterpolateBezier(float lerpValue, List<Vector2> p) {
            if (p.Count == 1) {
                return p.First();
            }
            return InterpolateBezier(lerpValue, p.MergedInPairs((v1, v2) => Vector2.Lerp(v1, v2, lerpValue)));
        }

        public static List<Vector2> DiscretizeQuadraticBezier(Vector2 p1, Vector2 p2, Vector2 p3, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateQuadraticBezier(p1, p2, p3, (float)i / (subdivisions + 1)));
        }
        public static List<Vector2> DiscretizeCubicBezier(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateCubicBezier(p1, p2, p3, p4, (float)i / (subdivisions + 1)));
        }
        public static List<Vector2> DiscretizeBezier(List<Vector2> p, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateBezier((float)i / (subdivisions + 1), p));
        }

        public static float ApproximateQuadraticBezierLength(Vector2 p1, Vector2 p2, Vector2 p3, int subdivisions) {
            return DiscretizeQuadraticBezier(p1, p2, p3, subdivisions).PathLength();
        }
        public static float ApproximateCubicBezierLength(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, int subdivisions) {
            return DiscretizeCubicBezier(p1, p2, p3, p4, subdivisions).PathLength();
        }
        public static float ApproximateBezierLength(List<Vector2> p, int subdivisions) {
            return DiscretizeBezier(p, subdivisions).PathLength();
        }
        #endregion

        #region Vector3
        public static Vector3 InterpolateQuadraticBezier(Vector3 p1, Vector3 p2, Vector3 p3, float lerpValue) {
            Vector3 a1 = Vector3.Lerp(p1, p2, lerpValue);
            Vector3 a2 = Vector3.Lerp(p2, p3, lerpValue);
            return Vector3.Lerp(a1, a2, lerpValue);
        }
        public static Vector3 InterpolateCubicBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float lerpValue) {
            Vector3 a1 = Vector3.Lerp(p1, p2, lerpValue);
            Vector3 a2 = Vector3.Lerp(p2, p3, lerpValue);
            Vector3 a3 = Vector3.Lerp(p3, p4, lerpValue);
            return InterpolateQuadraticBezier(a1, a2, a3, lerpValue);
        }
        public static Vector3 InterpolateBezier(float lerpValue, List<Vector3> p) {
            if (p.Count == 1) {
                return p.First();
            }
            return InterpolateBezier(lerpValue, p.MergedInPairs((v1, v2) => Vector3.Lerp(v1, v2, lerpValue)));
        }

        public static List<Vector3> DiscretizeQuadraticBezier(Vector3 p1, Vector3 p2, Vector3 p3, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateQuadraticBezier(p1, p2, p3, (float)i / (subdivisions + 1)));
        }
        public static List<Vector3> DiscretizeCubicBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateCubicBezier(p1, p2, p3, p4, (float)i / (subdivisions + 1)));
        }
        public static List<Vector3> DiscretizeBezier(List<Vector3> p, int subdivisions) {
            return CollectionUtils.CreateListByIndex(subdivisions + 2, i => InterpolateBezier((float)i / (subdivisions + 1), p));
        }

        public static float ApproximateQuadraticBezierLength(Vector3 p1, Vector3 p2, Vector3 p3, int subdivisions) {
            return DiscretizeQuadraticBezier(p1, p2, p3, subdivisions).PathLength();
        }
        public static float ApproximateCubicBezierLength(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int subdivisions) {
            return DiscretizeCubicBezier(p1, p2, p3, p4, subdivisions).PathLength();
        }
        public static float ApproximateBezierLength(List<Vector3> p, int subdivisions) {
            return DiscretizeBezier(p, subdivisions).PathLength();
        }
        #endregion

    }

}
