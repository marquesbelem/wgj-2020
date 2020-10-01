using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GalloUtils {
    public static partial class UIElementsUtils {
    
        public static Foldout NewLabeledFoldout(string label) {
            Foldout result = new Foldout();
            result.Q(null, "unity-toggle__input").Add(new Label(label));
            return result;
        }

    }

}
