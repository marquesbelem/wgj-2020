using System;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class FloatCounter {

        public Action<float> updateValue;
        [SerializeField] private float value;

        public float Value {
            get {
                return value;
            }
            set {
                bool changed = (this.value != value);
                this.value = value;
                if (changed) {
                    updateValue.Invoke(value);
                }
            }
        }

        public void Reset() {
            Value = 0f;
        }
        public void Add(float valueToAdd) {
            Value += valueToAdd;
        }
        public void Subtract(float valueToSubtract) {
            Value -= valueToSubtract;
        }

        public static implicit operator float(FloatCounter c) => c.Value;

    }

}
