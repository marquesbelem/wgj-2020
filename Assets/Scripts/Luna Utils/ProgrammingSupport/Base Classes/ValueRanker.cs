using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public abstract class ValueRanker<T> {

        public abstract float Rank(T value);
        public int Compare(T x, T y) {
            float fResult = Rank(x) - Rank(y);
            if (fResult < 0f) {//x lesser than y
                return -1;
            }
            else if (fResult == 0f) {//x equals y
                return 0;
            }
            else {//x greater than y
                return 1;
            }
        }

        public T BestFrom(List<T> list) {
            return list.MaxElement(t => Rank(t));
        }
        public T WorstFrom(List<T> list) {
            return list.MinElement(t => Rank(t));
        }
        public List<T> Sorted(List<T> list) {
            List<T> result = new List<T>(list);
            result.Sort(Compare);
            return result;
        }

    }

    [Serializable]
    public class Vector3Ranker : ValueRanker<Vector3> {
     
        [Flags]
        public enum RankingType {
            None = 0,
            Distance = 1,
            Direction = 2,
            DistanceAndDirection = 3
        }
        public RankingType rankingType;

        public bool RanksDistance {
            get {
                return rankingType.HasFlag(RankingType.Distance);
            }
        }
        public bool RanksDirection {
            get {
                return rankingType.HasFlag(RankingType.Direction);
            }
        }

        public Vector3 origin;
        public float baseDistance;
        public float distanceFallSpeed;
        public Vector3 baseDirection;
        public float directionFallSpeed;

        public override float Rank(Vector3 value) {
            float result = 0f;
            Vector3 delta = value - origin;
            float dist = delta.magnitude;
            if (RanksDistance) {
                result += -distanceFallSpeed * Mathf.Abs(dist - baseDistance);
            }
            if (RanksDirection && dist > 0f) {
                float angle = Vector3.Angle(baseDirection, delta);
                result += -directionFallSpeed * Mathf.Abs(angle);
            }
            return result;
        }

    }

    [Serializable]
    public class TransformRanker : ValueRanker<Transform> {

        [Flags]
        public enum RankingType {
            None = 0,
            Distance = 1,
            Direction = 2,
            DistanceAndDirection = 3,
            Rotation = 4,
            DistanceAndRotation = 5,
            DirectionAndRotation = 6,
            DistanceDirectionAndRotation = 7
        }
        public RankingType rankingType;

        public bool RanksDistance {
            get {
                return rankingType.HasFlag(RankingType.Distance);
            }
        }
        public bool RanksDirection {
            get {
                return rankingType.HasFlag(RankingType.Direction);
            }
        }
        public bool RanksRotation {
            get {
                return rankingType.HasFlag(RankingType.Rotation);
            }
        }

        public Vector3 origin;
        public float baseDistance;
        public float distanceFallSpeed;
        public Vector3 baseDirection;
        public float directionFallSpeed;
        public float rotationFallSpeed;

        public override float Rank(Transform value) {
            float result = 0f;
            Vector3 delta = value.position - origin;
            float dist = delta.magnitude;
            if (RanksDistance) {
                result += -distanceFallSpeed * Mathf.Abs(dist - baseDistance);
            }
            if (RanksDirection && dist > 0f) {
                float angle = Vector3.Angle(baseDirection, delta);
                result += -directionFallSpeed * Mathf.Abs(angle);
            }
            if (RanksRotation) {
                float angle = Vector3.Angle(baseDirection, value.forward);
                result += -rotationFallSpeed * Mathf.Abs(angle);
            }
            return result;
        }

    }

}

