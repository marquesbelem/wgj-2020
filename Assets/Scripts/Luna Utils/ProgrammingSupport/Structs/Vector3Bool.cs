using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [Serializable]
    public struct Vector3Bool {

        public bool x, y, z;

        public static bool operator ==(Vector3Bool lhs, Vector3Bool rhs) => (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z);
        public static bool operator !=(Vector3Bool lhs, Vector3Bool rhs) => (lhs.x != rhs.x) || (lhs.y != rhs.y) || (lhs.z != rhs.z);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => x + ", " + y + ", " + z;

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Vector3Bool))]
    public class Vector3BoolDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float thirdSizeWidth = position.width / 3f;
            float labelWidth = 13;
            float space = 5;
            float fieldWidth = thirdSizeWidth - labelWidth - space;

            SerializedProperty xProperty = property.FindPropertyRelative("x");
            SerializedProperty yProperty = property.FindPropertyRelative("y");
            SerializedProperty zProperty = property.FindPropertyRelative("z");

            EditorGUI.LabelField(position.WithWidth(labelWidth), "X");
            EditorGUI.PropertyField(position.WithPosX(x => x + labelWidth).WithWidth(fieldWidth), xProperty, GUIContent.none);
            EditorGUI.LabelField(position.WithPosX(x => x + thirdSizeWidth + space).WithWidth(labelWidth), "Y");
            EditorGUI.PropertyField(position.WithPosX(x => x + thirdSizeWidth + labelWidth + space).WithWidth(fieldWidth), yProperty, GUIContent.none);
            EditorGUI.LabelField(position.WithPosX(x => x + thirdSizeWidth * 2 + space).WithWidth(labelWidth), "Z");
            EditorGUI.PropertyField(position.WithPosX(x => x + thirdSizeWidth * 2 + labelWidth + space).WithWidth(fieldWidth), zProperty, GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
#endif

}
