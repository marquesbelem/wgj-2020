using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [CreateAssetMenu(fileName = "New Color Palette", menuName = "GalloUtils/ColorPalette")]
    public class ColorPalette : ScriptableObject {

        public List<Color> swatches;

    }

}
