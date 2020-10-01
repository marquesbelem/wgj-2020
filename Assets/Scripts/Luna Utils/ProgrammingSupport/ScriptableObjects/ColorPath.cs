using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [CreateAssetMenu(fileName = "New Color Path", menuName = "GalloUtils/ColorPath")]
    public class ColorPath : ColorPalette {

        [Space(20)]

        public Color startingColor = Color.white;

        [Serializable]
        public class KeyPoint {
            public int shadeCount = 1;
            public Color keyColor = Color.white;
        }
        public List<KeyPoint> keyPoints;

        public void ApplyPalette() {
            swatches = keyPoints.Reduce((e, v) => v.UnitedWith(ColorUtils.ColorLine(v.Last(), e.keyColor, e.shadeCount+1).StartingAt(1)), new List<Color>() { startingColor });
        }

    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ColorPath))]
    public class ColorPathEditor : Editor {

        private ColorPath obj;

        public void OnEnable() {
            obj = (ColorPath)target;
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            obj.ApplyPalette();
        }

    }
#endif

}
