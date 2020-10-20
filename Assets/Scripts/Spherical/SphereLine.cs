using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SphereLine : MonoBehaviour {

    public SphereCoordinates start;
    public SphereCoordinates finish;
    public LineRenderer lineRenderer;
    public float segmentMaxAngle = 2f;
    public float radiusOffset = 1f;

    private void Update() {
        UpdateGraphics();
    }
    public void UpdateGraphics() {
        if (start != null && finish != null && lineRenderer != null) {
            if (start == finish) {
                lineRenderer.positionCount = 0;
            }
            else {
                float angle = Vector3.Angle(start.transform.up, finish.transform.up);
                int segmentCount = Mathf.CeilToInt(angle / segmentMaxAngle);
                lineRenderer.positionCount = segmentCount + 1;
                for (int i = 0; i <= segmentCount; i++) {
                    float t = (float)i / segmentCount;
                    lineRenderer.SetPosition(i, Quaternion.Lerp(start.transform.rotation, finish.transform.rotation, t) * Vector3.up * Mathf.Lerp(start.radius + radiusOffset, finish.radius + radiusOffset, t));
                }
            }
        }
    }

}
