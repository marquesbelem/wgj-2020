using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerRegionManager : MonoBehaviour {

    private static List<bool> inputBlockers = new List<bool>();
    private static void GrantBlockerIndex(int index) {
        if (index >= inputBlockers.Count) {
            for (int i = inputBlockers.Count; i <= index; i++) {
                inputBlockers.Add(false);
            }
        }
    }
    public static void SetInputBlocker(int index, bool value) {
        GrantBlockerIndex(index);
        inputBlockers[index] = value;
    }
    public static bool IsInputBlocked => inputBlockers.Contains(true);

    public static PointerRegion startDragRegion = null;
    public class DragEvent : UnityEvent<PointerRegion, PointerRegion> { }
    public static DragEvent onDragFromTo = new DragEvent();

    public void BlockInput(int blockerIndex) {
        SetInputBlocker(blockerIndex, true);
    }
    public void FreeInput(int blockerIndex) {
        SetInputBlocker(blockerIndex, false);
    }
    public void SetInputBlocked(bool value) {
        SetInputBlocker(0, value);
    }

    private void Update() {
        PointerRegion.AllUpdateState(!IsInputBlocked);
        if (!IsInputBlocked) {
            PointerRegion regionBeingPointed = PointerRegion.RegionBeingPointed;
            if (Input.GetMouseButtonDown(0)) {
                startDragRegion = regionBeingPointed;
            }
            if (Input.GetMouseButtonUp(0)) {
                startDragRegion = null;
            }
            if (Input.GetMouseButton(0) && startDragRegion != regionBeingPointed && startDragRegion != null && regionBeingPointed != null) {
                onDragFromTo.Invoke(startDragRegion, regionBeingPointed);
                startDragRegion = regionBeingPointed;
            }
        }
    }

}
