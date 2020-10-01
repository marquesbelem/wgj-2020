using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class SimpleNodeConnection : MonoBehaviour {

        public static List<SimpleNodeConnection> instanceList = new List<SimpleNodeConnection>();
        public void Awake() {
            instanceList.Add(this);
        }
        private void OnDestroy() {
            instanceList.Remove(this);
        }
        public static void AddConnectionsAll() {
            instanceList.ForEach(i => i.AddConnections());
        }

        public Transform end1 = null;
        public Transform end2 = null;
        public GraphNode node1 = null;
        public GraphNode node2 = null;
        public float endDistTreshold = 0.1f;
        public bool bothDirections = false;

        public void AddConnections() {
            FindEndNodes();
            if (node1 != null && node2 != null) {
                node1.neighbors.Add(node2);
                if (bothDirections) {
                    node2.neighbors.Add(node1);
                }
            }
        }
        public void FindEndNodes() {
            node1 = GraphNode.instanceList.Find(i => i.transform.DistanceTo(end1) < endDistTreshold);
            node2 = GraphNode.instanceList.Find(i => i.transform.DistanceTo(end2) < endDistTreshold);
        }

    }

}
