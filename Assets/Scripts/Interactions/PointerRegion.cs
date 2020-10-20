using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointerRegion : MonoBehaviour {

    public static List<PointerRegion> instanceList = new List<PointerRegion>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }
    public static void AllUpdateState(bool isInputAllowed) {
        instanceList.ForEach(i => i.UpdateState(isInputAllowed));
    }
    public static PointerRegion RegionBeingPointed => instanceList.Find(i => i.IsPointerOver);
    public static Vector3 InputDirection => Camera.main.ScreenPointToRay(Input.mousePosition).direction;

    public SphericalRegionCircle region;
    public UnityEvent onIdle;
    public UnityEvent onHover;
    public UnityEvent onClicked;
    public bool IsPointerOver => region.Contains(InputDirection);
    public bool IsClickedOver => IsPointerOver && Input.GetMouseButton(0);

    private bool wasHovering = false;
    private bool wasBeingClicked = false;

    private void UpdateState(bool isInputAllowed) {
        if (isInputAllowed) {
            if (!wasBeingClicked && IsClickedOver) {
                onClicked.Invoke();
                wasBeingClicked = true;
            }
            if (wasBeingClicked && !IsClickedOver) {
                if (IsPointerOver) {
                    onHover.Invoke();
                    wasHovering = true;
                }
                else {
                    onIdle.Invoke();
                    wasHovering = false;
                }
                wasBeingClicked = false;
            }
            if (!IsClickedOver && !wasHovering && IsPointerOver) {
                onHover.Invoke();
                wasHovering = true;
            }
            if (!IsClickedOver && wasHovering && !IsPointerOver) {
                onIdle.Invoke();
                wasHovering = false;
            }
        }
        else if (wasHovering || wasBeingClicked) {
            wasHovering = false;
            wasBeingClicked = false;
            onIdle.Invoke();
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
