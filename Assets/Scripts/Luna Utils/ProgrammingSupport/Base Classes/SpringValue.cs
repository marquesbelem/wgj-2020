using System;
using UnityEngine;

namespace GalloUtils {

    [Serializable]
    public  class Spring<T> {

        public T targetValue;
        public T currentValue;
        public T velocity;

        public float tightness;
        public float damping;
        public float mass;

        public T CurrentSpringForce {
            get {
                return subtractionFunc.Invoke(inverseFunc.Invoke(multiplicationFunc.Invoke(subtractionFunc.Invoke(currentValue, targetValue), tightness)), multiplicationFunc.Invoke(velocity, damping));
            }
        }

        public void Accelerate(T acceleration, float duration) {
            velocity = additionFunc.Invoke(velocity, multiplicationFunc.Invoke(acceleration, duration));
        }
        public void ApplyForce(T force, float duration) {
            Accelerate(divisionFunc.Invoke(force, mass), duration);
        }

        public void Update(float deltaTime) {
            ApplyForce(CurrentSpringForce, deltaTime);
            currentValue = additionFunc.Invoke(currentValue, multiplicationFunc.Invoke(velocity, deltaTime));
        }

        [HideInInspector] public Func<T, T, T> additionFunc;
        [HideInInspector] public Func<T, T, T> subtractionFunc;
        [HideInInspector] public Func<T, T> inverseFunc;
        [HideInInspector] public Func<T, float, T> multiplicationFunc;
        [HideInInspector] public Func<T, float, T> divisionFunc;

    }

    [Serializable]
    public class SpringFloat : Spring<float> {
        public SpringFloat() {
            additionFunc = (v1, v2) => v1 + v2;
            subtractionFunc = (v1, v2) => v1 - v2;
            inverseFunc = (v) => -v;
            multiplicationFunc = (v, f) => v * f;
            divisionFunc = (v, f) => v / f;
        }
    }

    [Serializable]
    public class SpringVector2 : Spring<Vector2> {
        public SpringVector2() {
            additionFunc = (v1, v2) => v1 + v2;
            subtractionFunc = (v1, v2) => v1 - v2;
            inverseFunc = (v) => -v;
            multiplicationFunc = (v, f) => v * f;
            divisionFunc = (v, f) => v / f;
        }
    }

    [Serializable]
    public class SpringVector3 : Spring<Vector3> {
        public SpringVector3() {
            additionFunc = (v1, v2) => v1 + v2;
            subtractionFunc = (v1, v2) => v1 - v2;
            inverseFunc = (v) => -v;
            multiplicationFunc = (v, f) => v * f;
            divisionFunc = (v, f) => v / f;
        }
    }

    [Serializable]
    public class SpringQuaternion : Spring<Quaternion> {
        public SpringQuaternion() {
            additionFunc = (v1, v2) => v2 * v1;
            subtractionFunc = QuaternionExtension.Less;
            inverseFunc = Quaternion.Inverse;
            multiplicationFunc = QuaternionExtension.ScaledBy;
            divisionFunc = (v, f) => v.ScaledBy(1f / f);
        }
    }

    [Serializable]
    public class SpringColor : Spring<Color> {
        public SpringColor() {
            additionFunc = (v1, v2) => v1 + v2;
            subtractionFunc = (v1, v2) => v1 - v2;
            inverseFunc = (v) => new Color(-v.r, -v.g, -v.b, -v.a);
            multiplicationFunc = (v, f) => v * f;
            divisionFunc = (v, f) => v / f;
        }
    }

}
