using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGraphConnection : MonoBehaviour {

    public static List<InputGraphConnection> instanceList = new List<InputGraphConnection>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }
    public static void AllUpdateGraphics() {
        instanceList.ForEach(i => i.UpdateGraphics());
    }

    public static Vector3 InputDirection => Camera.main.ScreenPointToRay(Input.mousePosition).direction;

    public SphericalLine line;
    public SphericalCoordinates lineFinishPoint;

    private void Update() {
        UpdateGraphics();
    }
    public void UpdateGraphics() {
        if (PointerRegionManager.startDragRegion != null) {
            line.start = PointerRegionManager.startDragRegion.region.SphereCoordinatesRef;
            line.finish = lineFinishPoint;
            lineFinishPoint.transform.up = InputDirection;
            lineFinishPoint.ApplyToTransform();
            line.lineRenderer.enabled = true;
            line.UpdateGraphics();
        }
        else {
            line.lineRenderer.enabled = false;
        }
    }

}
