using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

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
    public static int HighestPrioritySelected {
        get {
            return selectedList.Max(c => c.priority);
        }
    }
    public static CameraShot FirstHighestPrioritySelected {
        get {
            if (AnySelected) {
                int highestPrioritySelected = HighestPrioritySelected;
                return selectedList.Find(c => c.priority == highestPrioritySelected);
            }
            return null;
        }
    }

    public void OnEnable() {
        selectedList.Add(this);
    }
    public void OnDisable() {
        selectedList.Remove(this);
    }

    public int priority = 0;
    public Transform pivot;
    public Vector3 localBaseDirection = Vector3.zero;
    public float distOnMinAngle = 10f;
    public float distOnMaxAngle = 10f;
    public Rect rotationControlLimits = Rect.zero;
    public LayerMask raycastMask;
    public QueryTriggerInteraction queryTriggerInteraction;
    public float offsetFromCollisionPoint = 0.75f;
    public float distMultiplyerOnActionInput = 0.5f;
    public Vector3 pivotLocalOffsetOnActionInput = Vector3.zero;
    public UnityEvent onBeginTargeted;
    public UnityEvent onEndTargeted;

    public Vector3 PivotPoint => pivot.position;
    public Quaternion BaseRotation => Quaternion.LookRotation(pivot.TransformDirection(localBaseDirection), pivot.up);
    public Vector3 Forward => BaseRotation * Vector3.forward;
    public Vector3 Up => BaseRotation * Vector3.up;
    public bool IsControlledHorizontalRotationLimited => rotationControlLimits.width < 360f;

    public Vector3 GetCurrentPivot(float actionInput) => PivotPoint + (pivot.rotation * pivotLocalOffsetOnActionInput * actionInput);
    public Quaternion GetCurrentRotation(Vector2 controlledRotation) => BaseRotation * Quaternion.Euler(controlledRotation);
    public float GetCurrentDistance(float actionInput, Vector2 controlledRotation) {
        float result = Mathf.Lerp(1f, distMultiplyerOnActionInput, actionInput) * Mathf.Lerp(distOnMinAngle, distOnMaxAngle, Mathf.InverseLerp(rotationControlLimits.yMin, rotationControlLimits.yMax, controlledRotation.x));
        if (Physics.Raycast(GetCurrentPivot(actionInput), GetCurrentRotation(controlledRotation) * -Vector3.forward, out RaycastHit hit, result, raycastMask, queryTriggerInteraction)) {
            result = hit.distance - offsetFromCollisionPoint;
        }
        return result;
    }
    public void ClampControlledRotation(ref Vector2 curControlledRotation) {
        curControlledRotation.x = Mathf.Clamp(curControlledRotation.x, rotationControlLimits.yMin, rotationControlLimits.yMax);
        if (IsControlledHorizontalRotationLimited) {
            curControlledRotation.y = Mathf.Clamp(curControlledRotation.y, rotationControlLimits.xMin, rotationControlLimits.xMax);
        }
    }

    private Quaternion BaseCamRot => Quaternion.AngleAxis(rotationControlLimits.center.x, -Up) * Quaternion.AngleAxis(rotationControlLimits.center.y, BaseRotation * Vector3.right) * BaseRotation;
    private Vector3 BaseCamPos => PivotPoint - (BaseCamRot * Vector3.forward * distOnMinAngle);

    private void OnValidate() {
        rotationControlLimits = PositiveRect(rotationControlLimits);
        localBaseDirection.Normalize();
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected() {
        if (pivot != null && localBaseDirection != Vector3.zero) {
            float dist = distOnMinAngle;
            if (dist != 0f) {
                Vector3 pivotPos = PivotPoint;
                Quaternion rot = BaseRotation;
                Vector3 forward = Forward;
                Vector3 up = Up;
                Vector3 right = rot * Vector3.right;
                Vector3 rotationOrigin = pivotPos - (forward * dist);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(rotationOrigin, pivotPos);

                float anglesRight = rotationControlLimits.xMax;
                float anglesLeft = rotationControlLimits.xMin;
                float anglesTop = rotationControlLimits.yMax;
                float anglesBot = rotationControlLimits.yMin;
                Vector3 centerDir = -forward * distOnMinAngle * Mathf.Sign(dist);
                Vector3 leftDirRight = Quaternion.AngleAxis(anglesLeft, -up) * right;
                Vector3 rightDir = Quaternion.AngleAxis(anglesRight, -up) * centerDir;
                Vector3 rightDirRight = Quaternion.AngleAxis(anglesRight, -up) * right;
                Vector3 topDir = Quaternion.AngleAxis(anglesTop, right) * centerDir;
                Vector3 botLeftDir = Quaternion.AngleAxis(anglesLeft, -up) * Quaternion.AngleAxis(anglesBot, right) * centerDir;
                Vector3 topRightDir = Quaternion.AngleAxis(anglesRight, -up) * Quaternion.AngleAxis(anglesTop, right) * centerDir;

                float topCos = Mathf.Cos(anglesTop * Mathf.Deg2Rad);
                float topSin = Mathf.Sin(anglesTop * Mathf.Deg2Rad);
                float botCos = Mathf.Cos(anglesBot * Mathf.Deg2Rad);
                float botSin = Mathf.Sin(anglesBot * Mathf.Deg2Rad);
                Vector3 topCenter = pivotPos + (up * dist * topSin);
                Vector3 botCenter = pivotPos + (up * dist * botSin);

                Handles.color = Color.yellow;
                Handles.DrawWireArc(pivotPos, up, rightDir, anglesRight - anglesLeft, dist);
                Handles.DrawWireArc(pivotPos, right, topDir, anglesBot - anglesTop, dist);

                if (IsControlledHorizontalRotationLimited) {
                    Handles.DrawWireArc(pivotPos, leftDirRight, botLeftDir, anglesTop - anglesBot, dist);
                    Handles.DrawWireArc(pivotPos, rightDirRight, topRightDir, anglesBot - anglesTop, dist);
                }

                Handles.DrawWireArc(topCenter, up, rightDir, anglesRight - anglesLeft, dist * topCos);
                Handles.DrawWireArc(botCenter, up, rightDir, anglesRight - anglesLeft, dist * botCos);
            }
            /*
            Camera sceneViewCam = SceneView.lastActiveSceneView.camera;
            float height = SceneView.lastActiveSceneView.position.height / 5f;
            float width = height * sceneViewCam.aspect;
            Pose oldCamPose = new Pose(sceneViewCam.transform.position, sceneViewCam.transform.rotation);
            sceneViewCam.transform.position = BaseCamPos;
            sceneViewCam.transform.rotation = BaseCamRot;
            Gizmos.DrawGUITexture(new Rect(3f, 3f, width, height), GetRenderedTexture(sceneViewCam));
            sceneViewCam.transform.position = oldCamPose.position;
            sceneViewCam.transform.rotation = oldCamPose.rotation;
            */
        }
    }
#endif

    public static Rect PositiveRect(Rect rect) {
        if (rect.width < 0f) {
            rect.x -= rect.width *= -1;
        }
        if (rect.height < 0f) {
            rect.y -= rect.height *= -1;
        }
        return rect;
    }
    private static Texture2D GetRenderedTexture(Camera camera) {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        camera.Render();
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;
        return image;
    }

}
