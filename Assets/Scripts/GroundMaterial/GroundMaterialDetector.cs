using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class GroundMaterialDetector : MonoBehaviour {

    public Vector3 raycastLocalDir = -Vector3.up;
    public float raycastMaxDist = 1f;
    public LayerMask raycastLayerMask;
    public QueryTriggerInteraction raycastTriggerInteraction = QueryTriggerInteraction.Ignore;

    public GroundMaterial.Type defaultType = GroundMaterial.Type.Grass;
    
    [Serializable] public class GroundMaterialEvent {
        public GroundMaterial.Type type;
        public UnityEvent onTypeMatch;
    }
    public List<GroundMaterialEvent> events;

    public GroundMaterial.Type Detect() {
        if (Physics.Raycast(transform.position, transform.rotation * raycastLocalDir, out RaycastHit hit, raycastMaxDist, raycastLayerMask, raycastTriggerInteraction)) {
            GroundMaterial groundMaterial = hit.collider.GetComponent<GroundMaterial>();
            if (groundMaterial != null) {
                return groundMaterial.type;
            }
        }
        return defaultType;
    }

    public void InvokeDetectedEvent() => events.Find(e => e.type == Detect())?.onTypeMatch.Invoke();

}
