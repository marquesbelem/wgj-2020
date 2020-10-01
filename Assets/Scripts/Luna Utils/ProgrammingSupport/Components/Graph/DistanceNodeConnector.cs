using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class DistanceNodeConnector : MonoBehaviour {

        public static List<DistanceNodeConnector> instanceList = new List<DistanceNodeConnector>();
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

        public void ResetConnections() {
            controlledNode.ClearNeighbors();
            AddConnections();
        }
        public void AddConnections() {
            controlledNode.neighbors.AddRange(GraphNode.instanceList.FindAll(i => (i != controlledNode) && (controlledNode.DistanceTo(i) <= maxDist)));
        }

    }

}
