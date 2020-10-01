using GalloUtils;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransformData {

    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;

    public static float Distance(TransformData t1, TransformData t2) {
        return Mathf.Max(t1.localPosition.DistanceTo(t2.localPosition), t1.localRotation.AngleTo(t2.localRotation), t1.localScale.DistanceTo(t2.localScale));
    }
    public static TransformData Lerp(TransformData t1, TransformData t2, float f) {
        return new TransformData() {
            localPosition = t1.localPosition.LerpedTo(t2.localPosition, f),
            localRotation = t1.localRotation.LerpedTo(t2.localRotation, f),
            localScale = t1.localScale.LerpedTo(t2.localScale, f)
        };
    }

    public static TransformData FromTransform(Transform t) {
        return new TransformData() {
            localPosition = t.localPosition,
            localRotation = t.localRotation,
            localScale = t.localScale
        };
    }

}
