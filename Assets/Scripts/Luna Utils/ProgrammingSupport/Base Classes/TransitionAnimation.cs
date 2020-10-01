using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GalloUtils {
    public class TransitionAnimation<T> {

        public enum LerpType {
            None,
            LinearPath,
            DiagonalLine,
            QudraticBezierCurve
        }
        public LerpType lerpType;
        public bool IsNone { get { return lerpType == LerpType.None; } }
        public bool IsLinearPath { get { return lerpType == LerpType.LinearPath; } }
        public bool IsDiagonalLine { get { return lerpType == LerpType.DiagonalLine; } }
        public bool IsQudraticBezierCurve { get { return lerpType == LerpType.QudraticBezierCurve; } }
        public bool IsEnabled { get { return !IsNone; } }

        public enum MovementType {
            ConstantSpeed,
            ConstantDuration,
            SpeedByDistance,
            InstantTeleport
        }
        public MovementType movementType;
        public bool IsConstantSpeed { get { return movementType == MovementType.ConstantSpeed; } }
        public bool IsConstantDuration { get { return movementType == MovementType.ConstantDuration; } }
        public bool IsSpeedByDistance { get { return movementType == MovementType.SpeedByDistance; } }
        public bool IsTeleport { get { return movementType == MovementType.InstantTeleport; } }


        public float constantSpeed = 1f;
        public float constantDuration = 1f;
        public float speedByDistance = 1f;
        public Action onFinished;

        [HideInInspector] public Func<T, T, float> distFunc;
        [HideInInspector] public Func<T, T, float, T> lerpValueFunc;

        private readonly List<T> targetValues = new List<T>();
        private T startingValue;
        private float progress = 0f;
        private float totalDistance = 0f;
        private float firstSegmentDistance = 0f;

        private float FirstSegmentProgress {
            get {
                return progress / firstSegmentDistance;
            }
        }

        public void RestartAt(T value) {
            progress = 0f;
            SetStartingValue(value);
        }
        public void SetStartingValue(T value) {
            startingValue = value;
            RecalculateDistance();
        }
        public void NewTargetValue(T newValue) {
            switch (lerpType) {
                case LerpType.LinearPath:
                    if (targetValues.Empty() || !targetValues.Last().Equals(newValue)) {
                        targetValues.Add(newValue);
                        RecalculateDistance();
                    }
                    break;
                case LerpType.DiagonalLine:
                    if (!targetValues.Empty()) {
                        startingValue = CurrentValue;
                    }
                    targetValues.Clear();
                    targetValues.Add(newValue);
                    RecalculateDistance();
                    break;
                case LerpType.QudraticBezierCurve:
                    if (!targetValues.Last().Equals(newValue)) {
                        targetValues.Add(newValue);
                        RecalculateDistance();
                    }
                    break;
            }
        }
        public void NewTargetValues(List<T> newValues) {
            switch (lerpType) {
                case LerpType.LinearPath:
                    targetValues.AddRange(newValues);
                    RecalculateDistance();
                    break;
                case LerpType.DiagonalLine:
                    if (!targetValues.Empty()) {
                        startingValue = CurrentValue;
                    }
                    targetValues.Clear();
                    targetValues.Add(newValues.Last());
                    RecalculateDistance();
                    break;
                case LerpType.QudraticBezierCurve:
                    targetValues.AddRange(newValues);
                    RecalculateDistance();
                    break;
            }
        }
        public void Update(float deltaTime) {
            if (!targetValues.Empty() && IsEnabled) {
                switch (movementType) {
                    case MovementType.ConstantSpeed:
                        progress += deltaTime * constantSpeed;
                        break;
                    case MovementType.ConstantDuration:
                        progress += deltaTime * totalDistance / constantDuration;
                        break;
                    case MovementType.SpeedByDistance:
                        progress += deltaTime * totalDistance * speedByDistance;
                        break;
                    case MovementType.InstantTeleport:
                        progress = totalDistance;
                        break;
                }
                switch (lerpType) {
                    case LerpType.LinearPath:
                        while (!targetValues.Empty() && progress >= firstSegmentDistance) {
                            progress -= firstSegmentDistance;
                            startingValue = targetValues.First();
                            targetValues.RemoveFirst();
                            RecalculateDistance();
                        }
                        if (targetValues.Empty()) {
                            progress = 0f;
                            onFinished?.Invoke();
                        }
                        break;
                    case LerpType.DiagonalLine:
                        if (progress >= totalDistance) {
                            progress = 0f;
                            startingValue = targetValues.First();
                            targetValues.RemoveFirst();
                            onFinished?.Invoke();
                        }
                        break;
                    case LerpType.QudraticBezierCurve:
                        while (targetValues.Count > 1 && progress >= firstSegmentDistance) {
                            progress -= firstSegmentDistance;
                            startingValue = targetValues.First();
                            targetValues.RemoveFirst();
                            RecalculateDistance();
                        }
                        if (targetValues.Count == 1 && progress >= totalDistance) {
                            progress = 0f;
                            startingValue = targetValues.First();
                            targetValues.RemoveFirst();
                            onFinished?.Invoke();
                        }
                        break;
                }
            }
        }
        public T CurrentValue {
            get {
                if (targetValues.Empty()) {
                    return startingValue;
                }
                switch (lerpType) {
                    case LerpType.None:
                        return default;
                    case LerpType.LinearPath:
                        return LerpFirstLineSegment();
                    case LerpType.DiagonalLine:
                        return LerpFirstLineSegment();
                    case LerpType.QudraticBezierCurve:
                        return (targetValues.Count == 1) ? LerpFirstLineSegment() : LerpFirstCurveSegment();
                    default:
                        return default;
                }
            }
        }
        public T LastTargetValue {
            get {
                if (targetValues.Empty()) {
                    return startingValue;
                }
                return targetValues.Last();
            }
        }

        private void RecalculateDistance() {
            if (!targetValues.Empty()) {
                switch (lerpType) {
                    case LerpType.LinearPath:
                        firstSegmentDistance = distFunc.Invoke(startingValue, targetValues.First());
                        totalDistance = firstSegmentDistance + targetValues.SumPairs(distFunc);
                        break;
                    case LerpType.DiagonalLine:
                        firstSegmentDistance = distFunc.Invoke(startingValue, targetValues.First());
                        totalDistance = firstSegmentDistance;
                        break;
                    case LerpType.QudraticBezierCurve:
                        if (targetValues.Count == 1) {
                            firstSegmentDistance = distFunc.Invoke(startingValue, targetValues.First());
                            totalDistance = firstSegmentDistance;
                        }
                        else {
                            firstSegmentDistance = FirstBezierCurveSegmentLength();
                            totalDistance = firstSegmentDistance + targetValues.SumTrios(BezierCurveSegmentLength);
                            totalDistance += distFunc.Invoke(Midpoint(targetValues[targetValues.LastIndex() - 1], targetValues.Last()), targetValues.Last());
                        }
                        break;
                }
            }
        }
        private T LerpFirstLineSegment() {
            return lerpValueFunc.Invoke(startingValue, targetValues.First(), FirstSegmentProgress);
        }
        private T LerpFirstCurveSegment() {
            return BezierCurveSolver.InterpolateQuadraticBezier(startingValue, targetValues.First(), Midpoint(targetValues[0], targetValues[1]), FirstSegmentProgress, lerpValueFunc);
        }
        private float FirstBezierCurveSegmentLength() {
            return BezierCurveMidpointSegmentLength(startingValue, targetValues[0], Midpoint(targetValues[0], targetValues[1]));
        }
        private float BezierCurveMidpointSegmentLength(T v1, T v2, T v3) {
            return BezierCurveSolver.ApproximateQuadraticBezierLength(v1, v2, v3, 0.1f, lerpValueFunc, distFunc);
        }
        private float BezierCurveSegmentLength(T v1, T v2, T v3) {
            return BezierCurveMidpointSegmentLength(Midpoint(v1, v2), v2, Midpoint(v2, v3));
        }
        private T Midpoint(T v1, T v2) {
            return lerpValueFunc.Invoke(v1, v2, 0.5f);
        }

    }

    [Serializable]
    public class FloatTransitionAnimation : TransitionAnimation<float> {
        public FloatTransitionAnimation() {
            lerpValueFunc = Mathf.Lerp;
            distFunc = FloatUtils.Distance;
        }
    }
    [Serializable]
    public class Vector2TransitionAnimation : TransitionAnimation<Vector2> {
        public Vector2TransitionAnimation() {
            lerpValueFunc = Vector2.Lerp;
            distFunc = Vector2Utils.Distance;
        }
    }
    [Serializable]
    public class Vector3TransitionAnimation : TransitionAnimation<Vector3> {
        public Vector3TransitionAnimation() {
            lerpValueFunc = Vector3.Lerp;
            distFunc = Vector3Utils.Distance;
        }
    }
    [Serializable]
    public class QuaternionTransitionAnimation : TransitionAnimation<Quaternion> {
        public QuaternionTransitionAnimation() {
            lerpValueFunc = Quaternion.Lerp;
            distFunc = Quaternion.Angle;
        }
    }
    [Serializable]
    public class ColorTransitionAnimation : TransitionAnimation<Color> {
        public ColorTransitionAnimation() {
            lerpValueFunc = Color.Lerp;
            distFunc = (v1, v2) => v1.DistanceTo(v2, ColorSystem.RGBA);
        }
    }
    [Serializable]
    public class RectTransitionAnimation : TransitionAnimation<Rect> {
        public RectTransitionAnimation() {
            lerpValueFunc = RectUtils.Lerp;
            distFunc = RectUtils.Distance;
        }
    }
    [Serializable]
    public class TransformDataTransitionAnimation : TransitionAnimation<TransformData> {
        public TransformDataTransitionAnimation() {
            lerpValueFunc = TransformData.Lerp;
            distFunc = TransformData.Distance;
        }
    }

}
