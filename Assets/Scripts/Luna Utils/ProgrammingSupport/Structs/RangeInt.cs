using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [Serializable]
    public struct RangeInt : IEnumerable<int> {

        public int position;
        public int size;

        public static RangeInt Zero => new RangeInt(0, 0);

        public int Center {
            get => position + (int)(size / 2f);
            set => position = Mathf.RoundToInt(value - (size / 2f));
        }
        public int Min {
            get => position;
            set {
                size -= value - position;
                position = value;
            }
        }
        public int Max {
            get => position + size;
            set => size = value - position;
        }

        public RangeInt(int position, int size) {
            this.position = position;
            this.size = size;
        }

        public bool Contains(int point) => point >= Min && point <= Max;
        public bool Overlaps(RangeInt other) => other.Max >= Min && other.Min <= Max;

        public int RandomValue() => UnityEngine.Random.Range(Min, Max);

        public int NormalizedToPoint(int normalizedRectCoordinates) => position + (size * normalizedRectCoordinates);
        public int PointToNormalized(int point) => (point / size) - position;

        public static bool operator ==(RangeInt lhs, RangeInt rhs) => (lhs.position == rhs.position) && (lhs.size == rhs.size);
        public static bool operator !=(RangeInt lhs, RangeInt rhs) => (lhs.position != rhs.position) || (lhs.size != rhs.size);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => position + ", " + size;

        public IEnumerator<int> GetEnumerator() {
            for (int i = Min; i <= Max; i++) {
                yield return i;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RangeInt))]
    public class RangeIntDrawer : PropertyDrawer {

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
