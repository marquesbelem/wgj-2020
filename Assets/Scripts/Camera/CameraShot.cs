using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraShot : MonoBehaviour {

    public static List<CameraShot> selectedList = new List<CameraShot>();
    public static bool AnySelected {
        get {
            return selectedList.Count > 0;
        }
    }
    public static CameraShot FirstSelected {
        get {
            return selectedList[0];
        }
    }
    public static int HighestPrioritySelected {
        get {
            return selectedList.Max(c => c.priority);
        }
    }
    public static CameraShot FirstHighestPrioritySelected {
        get {
            int highestPrioritySelected = HighestPrioritySelected;
            return selectedList.Find(c => c.priority == highestPrioritySelected);
        }
    }

    public int priority = 0;
    public Transform pivot;
    public Transform pose;
    public Vector2 rotationControlLimits;

    public Vector3 PivotPoint => pivot.position;
    public Quaternion Rotation => pose.rotation;
    public float Distance => Vector3.Dot(Delta, Forward);
    public Vector3 Forward => Rotation * Vector3.forward;
    public Vector3 Up => Rotation * Vector3.up;
    private Vector3 Delta => pivot.position - pose.position;

    public void OnEnable() {
        selectedList.Add(this);
    }
    public void OnDisable() {
        selectedList.Remove(this);
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected() {
        if (pivot != null && pose != null) {
            float dist = Distance;
            if (dist != 0f) {
                Vector3 pivotPos = PivotPoint;
                Quaternion rot = Rotation;
                Vector3 forward = Forward;
                Vector3 up = Up;
                Vector3 right = rot * Vector3.right;
                Vector3 rotationOrigin = pivotPos - (forward * dist);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(rotationOrigin, pivotPos);

                float anglesHorizontal = rotationControlLimits.x / 2f;
                float anglesVertical = rotationControlLimits.y / 2f;
                Vector3 centerDir = -Delta * Mathf.Sign(dist);
                Handles.color = Color.yellow;
                Handles.DrawWireArc(pivotPos, up, centerDir, anglesHorizontal, dist);
                Handles.DrawWireArc(pivotPos, -up, centerDir, anglesHorizontal, dist);
                Handles.DrawWireArc(pivotPos, right, centerDir, anglesVertical, dist);
                Handles.DrawWireArc(pivotPos, -right, centerDir, anglesVertical, dist);

                Vector3 leftDir = Quaternion.AngleAxis(anglesHorizontal, up) * centerDir;
                Vector3 leftDirRight = Quaternion.AngleAxis(anglesHorizontal, up) * right;
                Vector3 rightDir = Quaternion.AngleAxis(-anglesHorizontal, up) * centerDir;
                Vector3 rightDirRight = Quaternion.AngleAxis(-anglesHorizontal, up) * right;
                Handles.DrawWireArc(pivotPos, leftDirRight, leftDir, anglesVertical, dist);
                Handles.DrawWireArc(pivotPos, -leftDirRight, leftDir, anglesVertical, dist);
                Handles.DrawWireArc(pivotPos, rightDirRight, rightDir, anglesVertical, dist);
                Handles.DrawWireArc(pivotPos, -rightDirRight, rightDir, anglesVertical, dist);

                float cos = Mathf.Cos(anglesVertical * Mathf.Deg2Rad);
                float sin = Mathf.Sin(anglesVertical * Mathf.Deg2Rad);
                Vector3 topCenter = pivotPos + (up * dist * sin);
                Vector3 botCenter = pivotPos - (up * dist * sin);
                Handles.DrawWireArc(topCenter, up, centerDir, anglesHorizontal, dist * cos);
                Handles.DrawWireArc(topCenter, -up, centerDir, anglesHorizontal, dist * cos);
                Handles.DrawWireArc(botCenter, up, centerDir, anglesHorizontal, dist * cos);
                Handles.DrawWireArc(botCenter, -up, centerDir, anglesHorizontal, dist * cos);
            }
        }
    }
#endif

}
