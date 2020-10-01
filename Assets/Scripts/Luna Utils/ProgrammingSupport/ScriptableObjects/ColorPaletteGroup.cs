using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [CreateAssetMenu(fileName = "New Color Palette Group", menuName = "GalloUtils/ColorPaletteGroup")]
    public class ColorPaletteGroup : ColorPalette {

        [Space(20)]

        public List<ColorPalette> childrenPaletttes;

        public void ApplyPalette() {
            swatches = childrenPaletttes.ToSingleList(p => p.swatches);
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ColorPaletteGroup))]
    public class ColorPaletteGroupEditor : Editor {

        private ColorPaletteGroup obj;

        public void OnEnable() {
            obj = (ColorPaletteGroup)target;
        }
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            obj.ApplyPalette();
        }

    }
#endif

}
