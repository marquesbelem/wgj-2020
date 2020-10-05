using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform point;

    public Vector3 PivotPoint {
        get {
            return pivot.position;
        }
    }
    public Quaternion Rotation {
        get {
            return Quaternion.FromToRotation(Vector3.forward, pivot.position - point.position);
        }
    }
    public float Distance {
        get {
            return (pivot.position - point.position).magnitude;
        }
    }

    public void OnEnable() {
        selectedList.Add(this);
    }
    public void OnDisable() {
        selectedList.Remove(this);
    }

}
