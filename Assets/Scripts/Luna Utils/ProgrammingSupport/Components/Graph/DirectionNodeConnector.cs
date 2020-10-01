using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class DirectionNodeConnector : MonoBehaviour {

        public static List<DirectionNodeConnector> instanceList = new List<DirectionNodeConnector>();
        public void Awake() {
            instanceList.Add(this);
        }
        private void OnDestroy() {
            instanceList.Remove(this);
        }
        public static void ResetConnectionsAll() {
            instanceList.ForEach(i => i.ResetConnections());
        }

        public GraphNode controlledNode;
        public float maxDist = 1f;
        public List<Vector3> eulerRotationsForDirections;
        public float angleTreshold = 1f;

        public void ResetConnections() {
            controlledNode.ClearNeighbors();
            AddConnections();
        }
        public void AddConnections() {
            foreach (Vector3 rotation in eulerRotationsForDirections) {
                Vector3 direction = Quaternion.Euler(rotation) * transform.forward;
                List<GraphNode> candidates = GraphNode.instanceList.FindAll(i => (i != controlledNode) && (controlledNode.DistanceTo(i) <= maxDist) && (Vector3.Angle(controlledNode.DirectionTo(i), direction) < angleTreshold));
                controlledNode.neighbors.Add(candidates.MinElement(i => controlledNode.DistanceTo(i)));
            }
        }

    }

}
