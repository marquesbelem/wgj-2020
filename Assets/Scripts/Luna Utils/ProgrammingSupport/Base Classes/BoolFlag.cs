using System;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class BoolFlag {

        [SerializeField] private bool value;
        public Action<bool> updateValue;

        public bool Value {
            get {
                return value;
            }
            set {
                bool changed = (this.value != value);
                this.value = value;
                if (changed) {
                    if (updateValue != null) {
                        updateValue.Invoke(this.value);
                    }
                }
            }
        }
        public void Toggle() {
            Value = !Value;
        }

        public static implicit operator bool(BoolFlag f) => f.Value;

    }

}
