using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerRegionManager : MonoBehaviour {

    public static bool isInputAllowed = true;
    public static PointerRegion startDragRegion = null;
    public class DragEvent : UnityEvent<PointerRegion, PointerRegion> { }
    public static DragEvent onDragFromTo = new DragEvent();

    public bool IsInputAllowed {
        get => isInputAllowed;
        set => isInputAllowed = value;
    }

    private void Update() {
        PointerRegion.AllUpdateState(isInputAllowed);
        if (isInputAllowed) {
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
