using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [Serializable]
    public struct Vector2Bool {

        public bool x, y;

        public static bool operator ==(Vector2Bool lhs, Vector2Bool rhs) => (lhs.x == rhs.x) && (lhs.y == rhs.y);
        public static bool operator !=(Vector2Bool lhs, Vector2Bool rhs) => (lhs.x != rhs.x) || (lhs.y != rhs.y);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => x + ", " + y;

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Vector2Bool))]
    public class Vector2BoolDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float halfSizeWidth = position.width / 2f;
            float labelWidth = 13;
            float space = 5;
            float fieldWidth = halfSizeWidth - labelWidth - space;

            SerializedProperty xProperty = property.FindPropertyRelative("x");
            SerializedProperty yProperty = property.FindPropertyRelative("y");

            EditorGUI.LabelField(position.WithWidth(labelWidth), "X");
            EditorGUI.PropertyField(position.WithPosX(x => x + labelWidth).WithWidth(fieldWidth), xProperty, GUIContent.none);
            EditorGUI.LabelField(position.WithPosX(x => x + halfSizeWidth + space).WithWidth(labelWidth), "Y");
            EditorGUI.PropertyField(position.WithPosX(x => x + halfSizeWidth + labelWidth + space).WithWidth(fieldWidth), yProperty, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
#endif

}
