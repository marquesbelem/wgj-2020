using System;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class IntCounter {

        [SerializeField] private int value = 0;
        public Action<int> updateValue;

        public bool useMin;
        public int min = 0;

        public bool useMax;
        public int max = 1;

        public bool cycleOnMinCrossed = false;
        public bool cycleOnMaxCrossed = false;


        public int Value {
            get {
                return value;
            }
            set {
                int newValue = value;
                if (useMin) {
                    if (newValue < min) {
                        if (useMax && cycleOnMinCrossed) {
                            while (newValue < min) {
                                newValue = max + 1 + (newValue - min);
                            }
                        }
                        else {
                            newValue = Math.Max(newValue, min);
                        }
                    }
                }
                if (useMax) {
                    if (newValue > max) {
                        if (useMin && cycleOnMaxCrossed) {
                            while (newValue > max) {
                                newValue = (newValue - max) + min - 1;
                            }
                        }
                        else {
                            newValue = Math.Min(newValue, max);
                        }
                    }
                }
                bool changed = (this.value != newValue);
                this.value = newValue;
                if (changed) {
                    if (updateValue != null) {
                        updateValue.Invoke(this.value);
                    }
                }
            }
        }

        public void Reset() {
            if (useMin) {
                SetToMinValue();
            }
            else {
                Value = 0;
            }
        }
        public void Add(int valueToAdd) {
            Value += valueToAdd;
        }
        public void Subtract(int valueToSubtract) {
            Value -= valueToSubtract;
        }

        public void Increment() {
            Add(1);
        }
        public void Decrement() {
            Subtract(1);
        }

        public void SetToMinValue() {
            if (useMin) {
                Value = min;
            }
        }
        public void SetToMaxValue() {
            if (useMax) {
                Value = max;
            }
        }

        public static implicit operator int(IntCounter c) => c.Value;

    }

}
