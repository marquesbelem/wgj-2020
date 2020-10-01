using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GalloUtils {
    [Serializable]
    public struct Rect3Int : IEnumerable<Vector3Int> {

        public Vector3Int position;
        public Vector3Int size;

        public static Rect3Int Zero => new Rect3Int(Vector3Int.zero, Vector3Int.zero);

        public Vector3 Center {
            get => position + ((Vector3)size / 2f);
            set => position = (value - ((Vector3)size / 2f)).RoundedToInt();
        }
        public Vector3Int Min {
            get => position;
            set {
                size -= value - position;
                position = value;
            }
        }
        public Vector3Int Max {
            get => position + size;
            set => size = value - position;
        }
        public int Width {
            get => size.x;
            set => size.x = value;
        }
        public int Height {
            get => size.y;
            set => size.y = value;
        }
        public int Depth {
            get => size.z;
            set => size.z = value;
        }

        public Rect3Int(int x, int y, int z, int width, int height, int depth) {
            position = new Vector3Int(x, y, z);
            size = new Vector3Int(width, height, depth);
        }
        public Rect3Int(Vector3Int position, Vector3Int size) {
            this.position = position;
            this.size = size;
        }

        public bool Contains(Vector3Int point) => (point.x >= Min.x && point.x <= Max.x)
            && (point.y >= Min.y && point.y <= Max.y)
            && (point.z >= Min.z && point.z <= Max.z);
        public bool Overlaps(Rect3Int other) => (other.Max.x >= Min.x && other.Min.x <= Max.x)
            && (other.Max.y >= Min.y && other.Min.y <= Max.y)
            && (other.Max.z >= Min.z && other.Min.z <= Max.z);

        public Vector3Int RandomPointInside() => new Vector3Int(UnityEngine.Random.Range(Min.x, Max.x), UnityEngine.Random.Range(Min.y, Max.y), UnityEngine.Random.Range(Min.z, Max.z));

        public Vector3Int NormalizedToPoint(Vector3Int normalizedRectCoordinates) => position + size.ScaledBy(normalizedRectCoordinates);
        public Vector3Int PointToNormalized(Vector3Int point) => point.DividedBy(size) - position;

        public static bool operator ==(Rect3Int lhs, Rect3Int rhs) => (lhs.position == rhs.position) && (lhs.size == rhs.size);
        public static bool operator !=(Rect3Int lhs, Rect3Int rhs) => (lhs.position != rhs.position) || (lhs.size != rhs.size);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => position.ToString() + ", " + size.ToString();

        public IEnumerator<Vector3Int> GetEnumerator() {
            for (int i = Min.x; i <= Max.x; i++) {
                for (int j = Min.y; j <= Max.y; j++) {
                    for (int k = Min.z; k <= Max.z; k++) {
                        yield return new Vector3Int(i, j, k);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Rect3Int))]
    public class Rect3IntDrawer : PropertyDrawer {

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