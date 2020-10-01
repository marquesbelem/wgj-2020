using UnityEngine;

namespace GalloUtils {

    public static partial class QuaternionUtils {

        public static Quaternion Difference(Quaternion q1, Quaternion q2) {
            return q1 * Quaternion.Inverse(q2);
        }

    }

}
