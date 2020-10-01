using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.TerrainAPI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SphereCoordinates : MonoBehaviour {

    public Vector3 rotation;
    public float radius = 1f;
    public bool setRotation = true;
    public bool applyOnUpdate = false;

    public Quaternion QuaternionRotation => Quaternion.Euler(rotation);

    public void ApplyToTransform() {
        transform.position = QuaternionRotation * Vector3.up * radius;
        if (setRotation) {
            transform.rotation = QuaternionRotation;
        }
    }
    public void GetFromTransform() {
        Vector3 pos = transform.position;
        radius = pos.magnitude;
        rotation = Quaternion.FromToRotation(Vector3.up, pos).eulerAngles;
    }

    private void Update() {
        if (applyOnUpdate) {
            ApplyToTransform();
        }
    }

}

#if UNITY_EDITOR
[CanEditMultipleObjects, CustomEditor(typeof(SphereCoordinates))]
public class SphereCoordinatesEditor : Editor {

    SphereCoordinates obj;
    SerializedObject serializedObj;
    SerializedProperty rotationProperty;
    SerializedProperty radiusProperty;
    SerializedProperty setRotationProperty;
    SerializedProperty applyOnUpdateProperty;

    private void OnEnable() {
        obj = (SphereCoordinates)target;
        serializedObj = new SerializedObject(obj);
        rotationProperty = serializedObj.FindProperty("rotation");
        radiusProperty = serializedObj.FindProperty("radius");
        setRotationProperty = serializedObj.FindProperty("setRotation");
        applyOnUpdateProperty = serializedObj.FindProperty("applyOnUpdate");
        obj.GetFromTransform();
    }

    public override void OnInspectorGUI() {
        serializedObj.Update();
        EditorGUILayout.PropertyField(rotationProperty);
        EditorGUILayout.PropertyField(radiusProperty);
        if (serializedObj.ApplyModifiedProperties()) {
            obj.ApplyToTransform();
        }
        serializedObj.Update();
        EditorGUILayout.PropertyField(setRotationProperty);
        EditorGUILayout.PropertyField(applyOnUpdateProperty);
        serializedObj.ApplyModifiedProperties();

        if (GUILayout.Button("Get from Transform")) {
            Undo.RecordObject(obj, "Sphere coordinates from transform");
            obj.GetFromTransform();
        }
    }

}
#endif

