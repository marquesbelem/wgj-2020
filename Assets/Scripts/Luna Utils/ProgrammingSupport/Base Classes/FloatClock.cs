using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class FloatClock {

        [SerializeField] private float timeElapsed;
        public bool isCounting = true;
        public Action<float> updateValue;

        public float TimeElapsed {
            get {
                return timeElapsed;
            }
            set {
                bool changed = (this.timeElapsed != value);
                timeElapsed = value;
                timeElapsed = value;
                if (changed) {
                    updateValue?.Invoke(value);
                }
            }
        }

        public void ResetTime() {
            TimeElapsed = 0f;
        }
        public void AddTime(float valueToAdd) {
            TimeElapsed += valueToAdd;
        }

        public void Play() {
            isCounting = true;
            ResetTime();
        }
        public void Pause() {
            isCounting = false;
        }
        public void Stop() {
            isCounting = false;
            ResetTime();
        }
        public void Update(float deltaTime) {
            if (isCounting) {
                AddTime(deltaTime);
            }
        }

        public static implicit operator float(FloatClock c) => c.TimeElapsed;

    }

}
