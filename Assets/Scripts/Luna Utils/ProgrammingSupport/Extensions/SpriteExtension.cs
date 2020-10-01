using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class SpriteExtension {
    
        public static Texture2D CroppedTexture(this Sprite s) {
            return s.texture.Cropped(Vector2Int.RoundToInt(s.textureRect.position), Vector2Int.RoundToInt(s.textureRect.size));
        }

    }

}
