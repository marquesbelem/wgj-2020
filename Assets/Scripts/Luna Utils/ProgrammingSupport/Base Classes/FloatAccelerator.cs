using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class FloatAccelerator {

        public float currentValue = 0f;
        public float currentSpeed = 0f;
        public float currentAcceleration = 0f;

        public float maxSpeed = 1f;
        public float maxAcceleration = 1f;
        public bool isBreaking = false;

        public float RelativeSpeed {
            get {
                return currentSpeed / maxSpeed;
            }
            set {
                currentSpeed = value * maxSpeed;
            }
        }
        public float RelativeAcceleration {
            get {
                return currentAcceleration / maxAcceleration;
            }
            set {
                currentAcceleration = value  * maxAcceleration;
            }
        }

        public void Update(float deltaTime) {
            currentAcceleration = Mathf.Clamp(currentAcceleration, -maxAcceleration, maxAcceleration);
            bool isSpeedPositive = currentSpeed > 0f;
            currentSpeed += currentAcceleration * deltaTime;
            if (isBreaking) {
                if (isSpeedPositive) {
                    currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
                }
                else {
                    currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, 0f);
                }
            }
            else {
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            }
            currentValue += currentSpeed * deltaTime;
        }

    }

}
