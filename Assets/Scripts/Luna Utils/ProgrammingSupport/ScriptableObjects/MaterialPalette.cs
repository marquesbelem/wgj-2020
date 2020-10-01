using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [CreateAssetMenu(fileName = "New Material Palette", menuName = "GalloUtils/MaterialPalette")]
    public class MaterialPalette : ScriptableObject {

        public List<Material> swatches;

    }

}
