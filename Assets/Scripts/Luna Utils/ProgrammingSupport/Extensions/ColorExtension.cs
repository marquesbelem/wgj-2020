using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class ColorExtension {

        public static float GetHue(this Color self) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return h;
        }
        public static float GetSaturation(this Color self) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return s;
        }
        public static float GetValue(this Color self) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return v;
        }
        public static Vector3 GetHSV(this Color self) {
            Vector3 hsv;
            Color.RGBToHSV(self, out hsv.x, out hsv.y, out hsv.z);
            return hsv;
        }

        public static Color WithHue(this Color self, float hue) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return Color.HSVToRGB(hue, s, v);
        }
        public static Color WithSaturation(this Color self, float saturation) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return Color.HSVToRGB(h, saturation, v);
        }
        public static Color WithValue(this Color self, float value) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return Color.HSVToRGB(h, s, value);
        }
        public static Color WithRed(this Color self, float red) {
            return new Color(red, self.g, self.b, self.a);
        }
        public static Color WithGreen(this Color self, float green) {
            return new Color(self.r, green, self.g, self.a);
        }
        public static Color WithBlue(this Color self, float blue) {
            return new Color(self.r, self.g, blue, self.a);
        }
        public static Color WithAlfa(this Color self, float alfa) {
            return new Color(self.r, self.g, self.b, alfa);
        }

        public static Color WithHue(this Color self, Func<float, float> operation) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return Color.HSVToRGB(operation.Invoke(h), s, v);
        }
        public static Color WithSaturation(this Color self, Func<float, float> operation) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return Color.HSVToRGB(h, operation.Invoke(s), v);
        }
        public static Color WithValue(this Color self, Func<float, float> operation) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            return Color.HSVToRGB(h, s, operation.Invoke(v));
        }
        public static Color WithRed(this Color self, Func<float, float> operation) {
            return new Color(operation.Invoke(self.r), self.g, self.b, self.a);
        }
        public static Color WithGreen(this Color self, Func<float, float> operation) {
            return new Color(self.r, operation.Invoke(self.g), self.b, self.a);
        }
        public static Color WithBlue(this Color self, Func<float, float> operation) {
            return new Color(self.r, self.g, operation.Invoke(self.b), self.a);
        }
        public static Color WithAlfa(this Color self, Func<float, float> operation) {
            return new Color(self.r, self.g, self.b, operation.Invoke(self.a));
        }

        public static Color WithLerpedHue(this Color self, float targetHue, float lerp) {
            return self.WithHue(h => Mathf.Lerp(h, targetHue, lerp));
        }
        public static Color WithLerpedSaturation(this Color self, float targetSaturation, float lerp) {
            return self.WithSaturation(s => Mathf.Lerp(s, targetSaturation, lerp));
        }
        public static Color WithLerpedValue(this Color self, float targetValue, float lerp) {
            return self.WithValue(v => Mathf.Lerp(v, targetValue, lerp));
        }
        public static Color WithLerpedRed(this Color self, float targetRed, float lerp) {
            return self.WithRed(r => Mathf.Lerp(r, targetRed, lerp));
        }
        public static Color WithLerpedGreen(this Color self, float targetGreen, float lerp) {
            return self.WithGreen(g => Mathf.Lerp(g, targetGreen, lerp));
        }
        public static Color WithLerpedBlue(this Color self, float targetBlue, float lerp) {
            return self.WithBlue(b => Mathf.Lerp(b, targetBlue, lerp));
        }
        public static Color WithLerpedAlfa(this Color self, float targetAlfa, float lerp) {
            return self.WithAlfa(a => Mathf.Lerp(a, targetAlfa, lerp));
        }
    
        public static Color LerpedTo(this Color c1, Color c2, float lerp) {
            return Color.Lerp(c1, c2, lerp);
        }
    
        public static Vector4 ToColorSystemVector(this Color self, ColorSystem colorSystem) {
            float h, s, v;
            Color.RGBToHSV(self, out h, out s, out v);
            Vector3 vec, sl;
            switch (colorSystem) {
                case ColorSystem.RGB:
                    return new Vector4(self.r, self.g, self.b, 0f);
                case ColorSystem.HSV:
                    return Quaternion.AngleAxis(360f * h, Vector3.up) * new Vector3(s, v);
                case ColorSystem.HSL:
                    v = 2f * v - 1f;
                    if (v < s) {
                        sl = new Vector3(s, (v-s)/(s * 2f));
                    }
                    else {
                        sl = new Vector3(v+s, v-s);
                    }
                    return Quaternion.AngleAxis(360f * h, Vector3.up) * sl;
                case ColorSystem.HCV:
                    return Quaternion.AngleAxis(360f * h, Vector3.up) * new Vector3(s * v, v);
                case ColorSystem.HCL:
                    v = 2f * v - 1f;
                    if (v < s) {
                        sl = new Vector3(s, (v - s) / (s * 2f));
                    }
                    else {
                        sl = new Vector3(v + s, v - s);
                    }
                    sl.x *= 1f - Mathf.Abs(sl.y);
                    return Quaternion.AngleAxis(360f * h, Vector3.up) * sl;
                case ColorSystem.RGBA:
                    return new Vector4(self.r, self.g, self.b, self.a);
                case ColorSystem.HSVA:
                    vec = Quaternion.AngleAxis(360f * h, Vector3.up) * new Vector3(s, v);
                    return new Vector4(vec.x, vec.y, vec.z, self.a);
                case ColorSystem.HSLA:
                    v = 2f * v - 1f;
                    if (v < s) {
                        sl = new Vector3(s, (v - s) / (s * 2f));
                    }
                    else {
                        sl = new Vector3(v + s, v - s);
                    }
                    vec = Quaternion.AngleAxis(360f * h, Vector3.up) * sl;
                    return new Vector4(vec.x, vec.y, vec.z, self.a);
                case ColorSystem.HCVA:
                    vec = Quaternion.AngleAxis(360f * h, Vector3.up) * new Vector3(s * v, v);
                    return new Vector4(vec.x, vec.y, vec.z, self.a);
                case ColorSystem.HCLA:
                    v = 2f * v - 1f;
                    if (v < s) {
                        sl = new Vector3(s, (v - s) / (s * 2f));
                    }
                    else {
                        sl = new Vector3(v + s, v - s);
                    }
                    sl.x *= 1f - Mathf.Abs(sl.y);
                    vec = Quaternion.AngleAxis(360f * h, Vector3.up) * sl;
                    return new Vector4(vec.x, vec.y, vec.z, self.a);
                default:
                    return new Vector4();
            }
        }
        public static float DistanceTo(this Color c1, Color c2, ColorSystem colorSystem) {
            return c1.ToColorSystemVector(colorSystem).DistanceTo(c2.ToColorSystemVector(colorSystem));
        }
        public static Color ClosestColorIn(this Color c, List<Color> list, ColorSystem colorSystem) {
            return list.MinElement(e => c.DistanceTo(e, colorSystem));
        }


    }

}