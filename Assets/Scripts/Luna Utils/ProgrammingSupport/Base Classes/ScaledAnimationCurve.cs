using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {

    [Serializable]
    public class ScaledAnimationCurve {

        public float min = 0f;
        public float max = 1f;
        public AnimationCurve animationCurve;

        public float Evaluate(float f) {
            return min + (animationCurve.Evaluate(f) * (max - min));
        }

        public bool IsConfigured {
            get {
                return animationCurve != null;
            }
        }

    }

}