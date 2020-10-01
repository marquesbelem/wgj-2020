using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [Serializable]
    public struct Range {

        public float position;
        public float size;

        public static Range Zero => new Range(0f, 0f);

        public float Center {
            get => position + (size / 2f);
            set => position = value - (size / 2f);
        }
        public float Min {
            get => position;
            set {
                size -= value - position;
                position = value;
            }
        }
        public float Max {
            get => position + size;
            set => size = value - position;
        }

        public Range(float position, float size) {
            this.position = position;
            this.size = size;
        }

        public bool Contains(float point) => point >= Min && point <= Max;
        public bool Overlaps(Range other) => other.Max >= Min && other.Min <= Max;
        public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

        public float Lerp(float t) => Mathf.Lerp(Min, Max, t);
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(Min, Max, t);
        public float InverseLerp(float value) => Mathf.InverseLerp(Min, Max, value);
        public float InverseLerpUnclamped(float value) => FloatUtils.InverseLerpUnclamped(Min, Max, value);
        public float RemapTo(Range target, float value) => target.Lerp(InverseLerp(value));

        public float RandomValue() => UnityEngine.Random.Range(Min, Max);

        public float NormalizedToPoint(float normalizedRectCoordinates) => position + (size * normalizedRectCoordinates);
        public float PointToNormalized(float point) => (point / size) - position;

        public static bool operator ==(Range lhs, Range rhs) => (lhs.position == rhs.position) && (lhs.size == rhs.size);
        public static bool operator !=(Range lhs, Range rhs) => (lhs.position != rhs.position) || (lhs.size != rhs.size);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => position + ", " + size;

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float halfSizeWidth = position.width / 2f;
            float labelWidth = 13;
            float space = 5;
            float fieldWidth = halfSizeWidth - labelWidth - space;

            SerializedProperty positionProperty = property.FindPropertyRelative("position");
            SerializedProperty sizeProperty = property.FindPropertyRelative("size");

            EditorGUI.LabelField(position.WithWidth(labelWidth), "P");
            EditorGUI.PropertyField(position.WithPosX(x => x + labelWidth).WithWidth(fieldWidth), positionProperty, GUIContent.none);
            EditorGUI.LabelField(position.WithPosX(x => x + halfSizeWidth + space).WithWidth(labelWidth), "S");
            EditorGUI.PropertyField(position.WithPosX(x => x + halfSizeWidth + labelWidth + space).WithWidth(fieldWidth), sizeProperty, GUIContent.none);
            
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
#endif

}
