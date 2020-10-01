using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class ValueTypeUtils {

        public static TOut Remap<TIn, TOut, TMid>(TIn iMin, TIn iMax, TOut oMin, TOut oMax, TIn value, Func<TIn, TIn, TIn, TMid> inverseLerpFunc, Func<TOut, TOut, TMid, TOut> lerpFunc) {
            return lerpFunc.Invoke(oMin, oMax, inverseLerpFunc.Invoke(iMin, iMax, value));
        }
        public static T Remap<T>(T iMin, T iMax, T oMin, T oMax, T value, Func<T, T, T, T> inverseLerpFunc, Func<T, T, T, T> lerpFunc) {
            return lerpFunc.Invoke(oMin, oMax, inverseLerpFunc.Invoke(iMin, iMax, value));
        }

    }
    public static partial class FloatUtils {

        public static float Distance(float v0, float v1) {
            return Mathf.Abs(v1 - v0);
        }

        public static float InverseLerpUnclamped(float a, float b, float value) {
            return (value - a) / (b - a);
        }
        public static float Remap(float iMin, float iMax, float oMin, float oMax, float value) {
            return Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
        }
        public static float Remap(Range iRange, Range oRange, float value) {
            return oRange.Lerp(iRange.InverseLerp(value));
        }
        public static float RemapUnclamped(float iMin, float iMax, float oMin, float oMax, float value) {
            return Mathf.LerpUnclamped(oMin, oMax, InverseLerpUnclamped(iMin, iMax, value));
        }
        public static float RemapUnclamped(Range iRange, Range oRange, float value) {
            return oRange.LerpUnclamped(iRange.InverseLerpUnclamped(value));
        }

    }

    public static partial class IntUtils {

        public static int RotateInside(int v, int d) {
            int result = v % d;
            return (result < 0)? result+d : result;
        }

    }

}
