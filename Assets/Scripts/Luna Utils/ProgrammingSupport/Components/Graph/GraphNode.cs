using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public class GraphNode : MonoBehaviour {

        public static List<GraphNode> instanceList = new List<GraphNode>();
        public void Awake() {
            instanceList.Add(this);
        }
        private void OnDestroy() {
            instanceList.Remove(this);
        }
        public static void ClearNeighborsAll() {
            instanceList.ForEach(i => i.ClearNeighbors());
        }
        public static List<List<GraphNode>> GetIslands() {
            return instanceList.Group((n1, n2) => n1.CanReach(n2) || n2.CanReach(n1));
        }

        public List<GraphNode> neighbors;
        public void ClearNeighbors() {
            neighbors.Clear();
        }
        public Vector3 DirectionTo(GraphNode neighbor) {
            return neighbor.transform.position - transform.position;
        }
        public GraphNode ClosestNeighborToDirection(Vector3 direction) {
            return neighbors.MinElement(e => Vector3.Angle(DirectionTo(e), direction));
        }

        public float PathDistanceTo(GraphNode other) {
            return AStar_PathFinding(other).SumPairs((n1, n2) => n1.DistanceTo(n2));
        }

        #region GraphSolver
        public bool CanReach(GraphNode toNode) {
            return GraphSolver.CanReach(n => n.neighbors, this, toNode);
        }
        public bool CanReach(GraphNode toNode, int maxHops) {
            return GraphSolver.CanReach(n => n.neighbors, this, toNode, maxHops);
        }
        public bool CanReach(GraphNode toNode, float maxCost) {
            return GraphSolver.CanReach(n => n.neighbors, (n1, n2) => n1.DistanceTo(n2), this, toNode, maxCost);
        }

        public List<GraphNode> ReachableNodes() {
            return GraphSolver.ReachableNodes(n => n.neighbors, this);
        }
        public List<GraphNode> ReachableNodes(int maxHops) {
            return GraphSolver.ReachableNodes(n => n.neighbors, this, maxHops);
        }
        public List<GraphNode> ReachableNodes(float maxCost) {
            return GraphSolver.ReachableNodes(n => n.neighbors, (n1, n2) => n1.DistanceTo(n2), this, maxCost);
        }

        public List<GraphNode> ReachableNodes(Predicate<GraphNode> isFinal, bool startCanBeFinal = false) {
            return GraphSolver.ReachableNodes(n => n.neighbors,this, isFinal, startCanBeFinal);
        }
        public List<GraphNode> ReachableNodes(int maxHops, Predicate<GraphNode> isFinal, bool startCanBeFinal = false) {
            return GraphSolver.ReachableNodes(n => n.neighbors, this, maxHops, isFinal, startCanBeFinal);
        }
        public List<GraphNode> ReachableNodes(float maxCost, Predicate<GraphNode> isFinal, bool startCanBeFinal = false) {
            return GraphSolver.ReachableNodes(n => n.neighbors, (n1, n2) => n1.DistanceTo(n2), this, maxCost, isFinal, startCanBeFinal);
        }


        public List<GraphNode> BFS_PathFinding(Predicate<GraphNode> validFinal) {
            return GraphSolver.BFS_PathFinding(n => n.neighbors, this, validFinal);
        }
        public List<GraphNode> BFS_PathFinding(GraphNode toNode) {
            return GraphSolver.BFS_PathFinding(n => n.neighbors, this, toNode);
        }

        public List<GraphNode> Dijkstra_PathFinding(Predicate<GraphNode> validFinal) {
            return GraphSolver.Dijkstra_PathFinding(n => n.neighbors, (n1, n2) => n1.DistanceTo(n2), this, validFinal);
        }
        public List<GraphNode> Dijkstra_PathFinding(GraphNode toNode) {
            return GraphSolver.Dijkstra_PathFinding(n => n.neighbors, (n1, n2) => n1.DistanceTo(n2), this, toNode);
        }

        public List<GraphNode> AStar_PathFinding(GraphNode toNode) {
            return GraphSolver.AStar_PathFinding(n => n.neighbors, (n1, n2) => n1.DistanceTo(n2), n => n.DistanceTo(toNode), this, toNode);
        }


        public GraphNode LocalMinimumValueNode(Func<GraphNode, float> values) {
            return GraphSolver.LocalMinimumValueNode(n => n.neighbors, values, this);
        }
        public GraphNode LocalMaximumValueNode(Func<GraphNode, float> values) {
            return GraphSolver.LocalMaximumValueNode(n => n.neighbors, values, this);
        }
        #endregion

    }

}
