using Boo.Lang;
using UnityEngine;

namespace GalloUtils {
    public class AreaEffector : MonoBehaviour {

        public Vector3 forceToApply;
        public bool isForceLocal = true;
        public ForceMode forceMode = ForceMode.Force;
        public bool proportionalToBoundsIntersection = true;

        private readonly List<Collider> collidersInsideTrigger = new List<Collider>();
        private Collider colliderRef;
        private Collider ColliderRef {
            get {
                if (colliderRef == null) {
                    colliderRef = GetComponent<Collider>();
                }
                return colliderRef;
            }
        }

        private void FixedUpdate() {
            Vector3 force = forceToApply;
            if (isForceLocal) {
                force = transform.TransformVector(force);
            }
            foreach (Collider c in collidersInsideTrigger) {
                Rigidbody r = c.attachedRigidbody;
                if (r != null) {
                    if (proportionalToBoundsIntersection) {
                        force *= IntersectionPercentage(c.bounds);
                    }
                    r.AddForce(force, forceMode);
                }
            }
        }

        public void OnTriggerEnter(Collider other) {
            collidersInsideTrigger.Add(other);
        }
        private void OnTriggerExit(Collider other) {
            collidersInsideTrigger.Remove(other);
        }

        private float IntersectionPercentage(Bounds bounds) {
            bounds.center = transform.InverseTransformPoint(bounds.center).Abs();
            Vector3 intersectionMin = Vector3.Max(ColliderRef.bounds.min, bounds.min);
            Vector3 intersectionMax = Vector3.Min(ColliderRef.bounds.max, bounds.max);
            Vector3 relativeIntersectionSize = (intersectionMax - intersectionMin).DividedBy(bounds.size);
            return relativeIntersectionSize.x * relativeIntersectionSize.y * relativeIntersectionSize.z;
        }

    }
}