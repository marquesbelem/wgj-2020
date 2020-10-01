using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [CreateAssetMenu(fileName = "New Sprite Palette", menuName = "GalloUtils/SpritePalette")]
    public class SpritePalatte : ScriptableObject {

        public List<Sprite> colors;

    }

}
