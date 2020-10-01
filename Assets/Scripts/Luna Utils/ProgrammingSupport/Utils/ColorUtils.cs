using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {

    public static class ColorUtils {

        public static List<Color> ColorRamp(AnimationCurve hueCurve, AnimationCurve saturationCurve, AnimationCurve valueCurve, int shadeCount) {
            List<Color> result = new List<Color>();
            if (shadeCount == 1) {
                result.Add(Color.HSVToRGB(hueCurve.Evaluate(0.5f), saturationCurve.Evaluate(0.5f), valueCurve.Evaluate(0.5f)));
            }
            else if (shadeCount > 1) {
                for (int i = 0; i < shadeCount; i++) {
                    float t = i / (shadeCount - 1f);
                    result.Add(Color.HSVToRGB(hueCurve.Evaluate(t), saturationCurve.Evaluate(t), valueCurve.Evaluate(t)));
                }
            }
            return result;
        }
        public static List<Color> ColorRamp(ScaledAnimationCurve hueCurve, ScaledAnimationCurve saturationCurve, ScaledAnimationCurve valueCurve, int shadeCount) {
            List<Color> result = new List<Color>();
            if (shadeCount == 1) {
                result.Add(Color.HSVToRGB(hueCurve.Evaluate(0.5f), saturationCurve.Evaluate(0.5f), valueCurve.Evaluate(0.5f)));
            }
            else if (shadeCount > 1) {
                for (int i = 0; i < shadeCount; i++) {
                    float t = i / (shadeCount - 1f);
                    result.Add(Color.HSVToRGB(hueCurve.Evaluate(t), saturationCurve.Evaluate(t), valueCurve.Evaluate(t)));
                }
            }
            return result;
        }

        public static List<Color> ColorLine(Color c1, Color c2, int shadeCount) {
            List<Color> result = new List<Color>();
            if (shadeCount == 1) {
                result.Add(c1.LerpedTo(c2, 0.5f));
            }
            else if (shadeCount > 1) {
                for (int i = 0; i < shadeCount; i++) {
                    float t = i / (shadeCount - 1f);
                    result.Add(c1.LerpedTo(c2, t));
                }
            }
            return result;
        }

        public static Color Iterpolate(Color min, Color max, Color t) {
            return new Color(Mathf.Lerp(min.r, max.r, t.r), Mathf.Lerp(min.g, max.g, t.g), Mathf.Lerp(min.b, max.b, t.b), Mathf.Lerp(min.a, max.a, t.a));
        }
        public static Color InverseIterpolate(Color min, Color max, Color value) {
            return new Color(Mathf.InverseLerp(min.r, max.r, value.r), Mathf.InverseLerp(min.g, max.g, value.g), Mathf.InverseLerp(min.b, max.b, value.b), Mathf.InverseLerp(min.a, max.a, value.a));
        }
        public static Color Remap(Color iMin, Color iMax, Color oMin, Color oMax, Color value) {
            return Iterpolate(oMin, oMax, InverseIterpolate(iMin, iMax, value));
        }
        public static Color Remap(float iMin, float iMax, Color oMin, Color oMax, float value) {
            return Color.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));
        }

        public static Color IterpolateUnclamped(Color min, Color max, Color t) {
            return new Color(Mathf.LerpUnclamped(min.r, max.r, t.r), Mathf.LerpUnclamped(min.g, max.g, t.g), Mathf.LerpUnclamped(min.b, max.b, t.b), Mathf.LerpUnclamped(min.a, max.a, t.a));
        }
        public static Color InverseIterpolateUnclamped(Color min, Color max, Color value) {
            return new Color(FloatUtils.InverseLerpUnclamped(min.r, max.r, value.r),
                             FloatUtils.InverseLerpUnclamped(min.g, max.g, value.g),
                             FloatUtils.InverseLerpUnclamped(min.b, max.b, value.b),
                             FloatUtils.InverseLerpUnclamped(min.a, max.a, value.a));
        }
        public static Color RemapUnclamped(Color iMin, Color iMax, Color oMin, Color oMax, Color value) {
            return IterpolateUnclamped(oMin, oMax, InverseIterpolateUnclamped(iMin, iMax, value));
        }
        public static Color RemapUnclamped(float iMin, float iMax, Color oMin, Color oMax, float value) {
            return Color.LerpUnclamped(oMin, oMax, FloatUtils.InverseLerpUnclamped(iMin, iMax, value));
        }

    }

}