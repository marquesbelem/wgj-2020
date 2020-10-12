using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointerEventRegion : MonoBehaviour {

    public static PointerEventRegion startDragRegion = null;

    public class DragEvent : UnityEvent<PointerEventRegion, PointerEventRegion> { }
    public static DragEvent onDragFromTo = new DragEvent();

    public SphereRegion region;
    public bool IsPointerOver => region.Contains(Camera.main.ScreenPointToRay(Input.mousePosition).direction);

    public UnityEvent onIdle;
    public UnityEvent onHover;
    public UnityEvent onClicked;

    private bool wasHovering = false;

    private void Update() {
        if (IsPointerOver) {
            if (Input.GetMouseButtonDown(0)) {
                startDragRegion = this;
                onClicked.Invoke();
            }
            if (!wasHovering) {
                onHover.Invoke();
                wasHovering = true;
            }
            if (startDragRegion != null && startDragRegion != this) {
                onDragFromTo.Invoke(startDragRegion, this);
                startDragRegion = this;
            }
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                startDragRegion = null;
            }
            if (wasHovering) {
                onIdle.Invoke();
                wasHovering = false;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            startDragRegion = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (region != null) {
            region.DrawGizmos(new Color(0f, 0.5f, 0f));
        }
    }
#endif

}
