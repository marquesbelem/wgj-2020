using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class ComponentExtension {

        public static float DistanceTo(this Component c1, Component c2) {
            return c1.transform.DistanceTo(c2.transform);
        }

    }

}
