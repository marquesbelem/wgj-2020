using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GalloUtils {
    public static partial class VisualElementExtension {
    
        public static void SetDisplay(this VisualElement visualElement, bool flag) {
            visualElement.style.display = (flag) ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }

}
