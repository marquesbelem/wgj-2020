using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [CreateAssetMenu(fileName = "New Color Ramp", menuName = "GalloUtils/ColorRamp")]
    public class ColorRamp : ColorPalette {

        [Space(20)]

        public int shadeCount = 5;
        public ScaledAnimationCurve hue;
        public ScaledAnimationCurve saturation;
        public ScaledAnimationCurve value;

        public void ApplyPalette() {
            swatches = ColorUtils.ColorRamp(hue, saturation, value, shadeCount);
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ColorRamp))]
    public class ColorRampEditor : Editor {

        private ColorRamp obj;

        public void OnEnable() {
            obj = (ColorRamp)target;
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            obj.ApplyPalette();
        }

    }
#endif

}
