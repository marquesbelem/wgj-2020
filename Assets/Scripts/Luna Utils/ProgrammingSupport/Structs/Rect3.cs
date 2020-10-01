using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [Serializable]
    public struct Rect3 {

        public Vector3 position;
        public Vector3 size;

        public static Rect3 Zero => new Rect3(Vector3.zero, Vector3.zero);

        public Vector3 Center {
            get => position + (size / 2f);
            set => position = value - (size / 2f);
        }
        public Vector3 Min {
            get => position;
            set {
                size -= value - position;
                position = value;
            }
        }
        public Vector3 Max {
            get => position + size;
            set => size = value - position;
        }
        public float Width {
            get => size.x;
            set => size.x = value;
        }
        public float Height {
            get => size.y;
            set => size.y = value;
        }
        public float Depth {
            get => size.z;
            set => size.z = value;
        }

        public Rect3(float x, float y, float z, float width, float height, float depth) {
            position = new Vector3(x, y, z);
            size = new Vector3(width, height, depth);
        }
        public Rect3(Vector3 position, Vector3 size) {
            this.position = position;
            this.size = size;
        }
        public Rect3(Rect3 source) {
            position = source.position;
            size = source.size;
        }

        public bool Contains(Vector3 point) {
            return (point.x >= Min.x && point.x <= Max.x)
                && (point.y >= Min.y && point.y <= Max.y)
                && (point.z >= Min.z && point.z <= Max.z);
        }
        public bool Overlaps(Rect3 other) {
            return (other.Max.x >= Min.x && other.Min.x <= Max.x)
                && (other.Max.y >= Min.y && other.Min.y <= Max.y)
                && (other.Max.z >= Min.z && other.Min.z <= Max.z);
        }

        public Vector3 RandomPointInside() {
            return new Vector3(UnityEngine.Random.Range(Min.x, Max.x), UnityEngine.Random.Range(Min.y, Max.y), UnityEngine.Random.Range(Min.z, Max.z));
        }

        public Vector3 NormalizedToPoint(Vector3 normalizedRectCoordinates) {
            return position + size.ScaledBy(normalizedRectCoordinates);
        }
        public Vector3 PointToNormalized(Vector3 point) {
            return point.DividedBy(size) - position;
        }

        public static bool operator ==(Rect3 lhs, Rect3 rhs) {
            return (lhs.position == rhs.position) && (lhs.size == rhs.size);
        }
        public static bool operator !=(Rect3 lhs, Rect3 rhs) {
            return (lhs.position != rhs.position) || (lhs.size != rhs.size);
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return position.ToString() + ", " + size.ToString();
        }

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Rect3))]
    public class Rect3Drawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float halfHeight = position.height / 2f;

            Rect positionRect = new Rect(position.x, position.y, position.width, halfHeight);
            Rect sizeRect = new Rect(position.x, position.y + halfHeight, position.width, halfHeight);

            float thirdSizeWidth = sizeRect.width / 3f;
            float labelWidth = 13;
            float space = 5;
            float fieldWidth = thirdSizeWidth - labelWidth - space;
            
            SerializedProperty sizeProperty = property.FindPropertyRelative("size");
            SerializedProperty positionProperty = property.FindPropertyRelative("position");

            EditorGUI.LabelField(positionRect.WithWidth(labelWidth), "X");
            EditorGUI.PropertyField(positionRect.WithPosX(x => x + labelWidth).WithWidth(fieldWidth), positionProperty.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.LabelField(positionRect.WithPosX(x => x + thirdSizeWidth + space).WithWidth(labelWidth), "Y");
            EditorGUI.PropertyField(positionRect.WithPosX(x => x + thirdSizeWidth + labelWidth + space).WithWidth(fieldWidth), positionProperty.FindPropertyRelative("y"), GUIContent.none);
            EditorGUI.LabelField(positionRect.WithPosX(x => x + thirdSizeWidth * 2 + space).WithWidth(labelWidth), "Z");
            EditorGUI.PropertyField(positionRect.WithPosX(x => x + thirdSizeWidth * 2 + labelWidth + space).WithWidth(fieldWidth), positionProperty.FindPropertyRelative("z"), GUIContent.none);

            EditorGUI.LabelField(sizeRect.WithWidth(labelWidth), "W");
            EditorGUI.PropertyField(sizeRect.WithPosX(x => x + labelWidth).WithWidth(fieldWidth), sizeProperty.FindPropertyRelative("x"), GUIContent.none);
            EditorGUI.LabelField(sizeRect.WithPosX(x => x + thirdSizeWidth + space).WithWidth(labelWidth), "H");
            EditorGUI.PropertyField(sizeRect.WithPosX(x => x + thirdSizeWidth + labelWidth + space).WithWidth(fieldWidth), sizeProperty.FindPropertyRelative("y"), GUIContent.none);
            EditorGUI.LabelField(sizeRect.WithPosX(x => x + thirdSizeWidth * 2 + space).WithWidth(labelWidth), "D");
            EditorGUI.PropertyField(sizeRect.WithPosX(x => x + thirdSizeWidth * 2 + labelWidth + space).WithWidth(fieldWidth), sizeProperty.FindPropertyRelative("z"), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * 2f;
        }
    }
#endif

}