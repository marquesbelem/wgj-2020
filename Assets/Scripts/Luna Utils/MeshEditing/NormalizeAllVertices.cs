#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteAlways]
public class NormalizeAllVertices : MonoBehaviour {

    public Mesh mesh;

    private void OnEnable() {
        Selection.selectionChanged += LogSelection;
        Apply();
    }
    private void OnDisable() {
        Selection.selectionChanged -= LogSelection;
    }
    private void OnValidate() {
        Apply();
    }

    public void LogSelection() {
        Object selectedObject = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(selectedObject);
        Debug.Log(selectedObject.GetType().Name + ": " + selectedObject.name + " at " + path);
    }
    public void Apply() {
        if (mesh != null) {
            List<Vector3> vertices = new List<Vector3>(mesh.vertices);
            vertices.SetEachElement(v => v.normalized);
            mesh.SetVertices(vertices.ToArray());
            AssetDatabase.SaveAssets();
        }
    }

}
#endif