using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class MotorFloat {

        public float currentValue = 0f;
        public float velocity = 0f;
        public float propulsionForce = 1f;

        public float frictionCoefficient = 1f;
        public float drag = 0f;
        public float mass = 1f;

        public float externalForce = 0f;
        public float gravityMovementComponent = 0f;
        public float gravityFrictionComponent = 0f;

        public void Update(float deltaTime) {
            float resultForce = propulsionForce + externalForce + (gravityMovementComponent * mass);
            resultForce = Mathf.Max(0f, resultForce - (velocity * drag) - (gravityFrictionComponent * frictionCoefficient));
            velocity += (resultForce / mass) * deltaTime;
            currentValue += velocity * deltaTime;
        }

    }

}
