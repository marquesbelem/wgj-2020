using System.Collections.Generic;
using System;

namespace GalloUtils {
    public static class GraphSolver {

        public static bool CanReach<TNode>(Func<TNode, List<TNode>> links, TNode fromNode, TNode toNode) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> borderNodes = new List<int>();
            visitedNodes.Add(fromNode);
            borderNodes.Add(0);
            while (borderNodes.Count > 0) {
                int index = borderNodes[0];
                if (visitedNodes[index].Equals(toNode)) {
                    return true;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[index])) {
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodes.Add(visitedNodes.Count);
                        visitedNodes.Add(neighbor);
                    }
                }
                borderNodes.RemoveAt(0);
            }
            return false;
        }
        public static bool CanReach<TNode>(Func<TNode, List<TNode>> links, TNode fromNode, TNode toNode, int maxHops) {
            return ReachableNodes(links, fromNode, maxHops).Contains(toNode);
        }
        public static bool CanReach<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode fromNode, TNode toNode, float maxCost) {
            return ReachableNodes(links, costs, fromNode, maxCost).Contains(toNode);
        }

        public static List<TNode> ReachableNodes<TNode>(Func<TNode, List<TNode>> links, TNode start) {
            List<TNode> result = new List<TNode>();
            List<int> newIndexes = new List<int>();
            result.Add(start);
            newIndexes.Add(0);
            while (newIndexes.Count > 0) {
                int index = newIndexes[0];
                foreach (TNode neighbor in links.Invoke(result[index])) {
                    if (!result.Contains(neighbor)) {
                        newIndexes.Add(result.Count);
                        result.Add(neighbor);
                    }
                }
                newIndexes.RemoveAt(0);
            }
            return result;
        }
        public static List<TNode> ReachableNodes<TNode>(Func<TNode, List<TNode>> links, TNode start, int maxHops) {
            List<TNode> result = new List<TNode>();
            result.Add(start);
            for (int i = 0; i < maxHops; i++) {
                List<TNode> newElements = new List<TNode>();
                foreach (TNode reachableNodes in result) {
                    newElements.AddRange(links.Invoke(reachableNodes));
                }
                result.AddRangeIfDoesntContain(newElements);
            }
            return result;
        }
        public static List<TNode> ReachableNodes<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode start, float maxCost) {
            List<TNode> result = new List<TNode>();
            List<float> distances = new List<float>();
            List<int> newIndexes = new List<int>();
            result.Add(start);
            distances.Add(0f);
            newIndexes.Add(0);
            do {
                int index = newIndexes[0];
                foreach (TNode candidate in links.Invoke(result[index])) {
                    float candidateCost = distances[index] + costs.Invoke(result[index], candidate);
                    if (candidateCost <= maxCost) {
                        if (result.Contains(candidate)) {
                            int candidateID = result.IndexOf(candidate);
                            if (distances[candidateID] > candidateCost) {
                                distances[candidateID] = candidateCost;
                            }
                        }
                        else {
                            newIndexes.Add(result.Count);
                            result.Add(candidate);
                            distances.Add(candidateCost);
                        }
                    }
                }
                newIndexes.RemoveAt(0);
            } while (newIndexes.Count > 0);
            return result;
        }

        public static List<TNode> ReachableNodes<TNode>(Func<TNode, List<TNode>> links, TNode start, Predicate<TNode> isFinal, bool startCanBeFinal = false) {
            return ReachableNodes(n => (isFinal.Invoke(n) && (!n.Equals(start) || (n.Equals(start) && startCanBeFinal))) ? new List<TNode>() : links.Invoke(n), start);
        }
        public static List<TNode> ReachableNodes<TNode>(Func<TNode, List<TNode>> links, TNode start, int maxHops, Predicate<TNode> isFinal, bool startCanBeFinal = false) {
            return ReachableNodes(n => (isFinal.Invoke(n) && (!n.Equals(start) || (n.Equals(start) && startCanBeFinal))) ? new List<TNode>() : links.Invoke(n), start, maxHops);
        }
        public static List<TNode> ReachableNodes<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode start, float maxCost, Predicate<TNode> isFinal, bool startCanBeFinal = false) {
            return ReachableNodes(n => (isFinal.Invoke(n) && (!n.Equals(start) || (n.Equals(start) && startCanBeFinal))) ? new List<TNode>() : links.Invoke(n), costs, start, maxCost);
        }

        public static List<TNode> BFS_PathTree<TNode>(Func<TNode, List<TNode>> links, TNode fromNode, out List<int> parentIDs) {
            List<TNode> visitedNodes = new List<TNode>();
            parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs[0];
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return visitedNodes;
        }
        public static List<TNode> Dijkstra_PathTree<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode fromNode, out List<int> parentIDs, out List<float> pathCosts) {
            List<TNode> visitedNodes = new List<TNode>();
            parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();
            List<float> accruedCosts = new List<float>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);
            accruedCosts.Add(0f);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs.FindMinimum(i => accruedCosts[i]).element;
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    float linkCost = costs.Invoke(visitedNodes[cur], neighbor);
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                        accruedCosts.Add(accruedCosts[cur] + linkCost);
                    }
                    else {
                        int neighborID = visitedNodes.IndexOf(neighbor);
                        if (accruedCosts[neighborID] > accruedCosts[cur] + linkCost) {
                            accruedCosts[neighborID] = accruedCosts[cur] + linkCost;
                            parentIDs[neighborID] = cur;
                        }
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            pathCosts = new List<float>();
            pathCosts.AddRange(accruedCosts);
            return visitedNodes;
        }


        public static List<TNode> BFS_PathFinding<TNode>(Func<TNode, List<TNode>> links, TNode fromNode, Predicate<TNode> validFinal) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs[0];
                if (validFinal.Invoke(visitedNodes[cur])) {
                    List<TNode> result = new List<TNode>();
                    while (cur >= 0) {
                        result.Add(visitedNodes[cur]);
                        cur = parentIDs[cur];
                    }
                    result.Reverse();
                    return result;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return null;
        }
        public static List<TNode> BFS_PathFinding<TNode>(Func<TNode, List<TNode>> links, TNode fromNode, TNode toNode) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs[0];
                if (visitedNodes[cur].Equals(toNode)) {
                    List<TNode> result = new List<TNode>();
                    while (cur >= 0) {
                        result.Add(visitedNodes[cur]);
                        cur = parentIDs[cur];
                    }
                    result.Reverse();
                    return result;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return null;
        }

        public static List<TNode> Dijkstra_PathFinding<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode fromNode, Predicate<TNode> validFinal) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();
            List<float> accruedCosts = new List<float>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);
            accruedCosts.Add(0f);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs.FindMinimum(i => accruedCosts[i]).element;
                if (validFinal.Invoke(visitedNodes[cur])) {
                    List<TNode> result = new List<TNode>();
                    while (cur >= 0) {
                        result.Add(visitedNodes[cur]);
                        cur = parentIDs[cur];
                    }
                    result.Reverse();
                    return result;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    float linkCost = costs.Invoke(visitedNodes[cur], neighbor);
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                        accruedCosts.Add(accruedCosts[cur] + linkCost);
                    }
                    else {
                        int neighborID = visitedNodes.IndexOf(neighbor);
                        if (accruedCosts[neighborID] > accruedCosts[cur] + linkCost) {
                            accruedCosts[neighborID] = accruedCosts[cur] + linkCost;
                            parentIDs[neighborID] = cur;
                        }
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return null;
        }
        public static List<TNode> Dijkstra_PathFinding<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode fromNode, TNode toNode) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();
            List<float> accruedCosts = new List<float>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);
            accruedCosts.Add(0f);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs.FindMinimum(i => accruedCosts[i]).element;
                if (visitedNodes[cur].Equals(toNode)) {
                    List<TNode> result = new List<TNode>();
                    while (cur >= 0) {
                        result.Add(visitedNodes[cur]);
                        cur = parentIDs[cur];
                    }
                    result.Reverse();
                    return result;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    float linkCost = costs.Invoke(visitedNodes[cur], neighbor);
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                        accruedCosts.Add(accruedCosts[cur] + linkCost);
                    }
                    else {
                        int neighborID = visitedNodes.IndexOf(neighbor);
                        if (accruedCosts[neighborID] > accruedCosts[cur] + linkCost) {
                            accruedCosts[neighborID] = accruedCosts[cur] + linkCost;
                            parentIDs[neighborID] = cur;
                        }
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return null;
        }

        public static List<TNode> AStar_PathFinding<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, Func<TNode, float> heuristics, TNode fromNode, Predicate<TNode> validFinal) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();
            List<float> accruedCosts = new List<float>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);
            accruedCosts.Add(0f);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs.FindMinimum(i => accruedCosts[i] + heuristics.Invoke(visitedNodes[i])).element;
                if (validFinal.Invoke(visitedNodes[cur])) {
                    List<TNode> result = new List<TNode>();
                    while (cur >= 0) {
                        result.Add(visitedNodes[cur]);
                        cur = parentIDs[cur];
                    }
                    result.Reverse();
                    return result;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    float linkCost = costs.Invoke(visitedNodes[cur], neighbor);
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                        accruedCosts.Add(accruedCosts[cur] + linkCost);
                    }
                    else {
                        int neighborID = visitedNodes.IndexOf(neighbor);
                        if (accruedCosts[neighborID] > accruedCosts[cur] + linkCost) {
                            accruedCosts[neighborID] = accruedCosts[cur] + linkCost;
                            parentIDs[neighborID] = cur;
                        }
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return null;
        }
        public static List<TNode> AStar_PathFinding<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, Func<TNode, float> heuristics, TNode fromNode, TNode toNode) {
            List<TNode> visitedNodes = new List<TNode>();
            List<int> parentIDs = new List<int>();
            List<int> borderNodeIDs = new List<int>();
            List<float> accruedCosts = new List<float>();

            visitedNodes.Add(fromNode);
            parentIDs.Add(-1);
            borderNodeIDs.Add(0);
            accruedCosts.Add(0f);

            while (borderNodeIDs.Count > 0) {
                int cur = borderNodeIDs.FindMinimum(i => accruedCosts[i] + heuristics.Invoke(visitedNodes[i])).element;
                if (visitedNodes[cur].Equals(toNode)) {
                    List<TNode> result = new List<TNode>();
                    while (cur >= 0) {
                        result.Add(visitedNodes[cur]);
                        cur = parentIDs[cur];
                    }
                    result.Reverse();
                    return result;
                }
                foreach (TNode neighbor in links.Invoke(visitedNodes[cur])) {
                    float linkCost = costs.Invoke(visitedNodes[cur], neighbor);
                    if (!visitedNodes.Contains(neighbor)) {
                        borderNodeIDs.Add(visitedNodes.Count);
                        parentIDs.Add(cur);
                        visitedNodes.Add(neighbor);
                        accruedCosts.Add(accruedCosts[cur] + linkCost);
                    }
                    else {
                        int neighborID = visitedNodes.IndexOf(neighbor);
                        if (accruedCosts[neighborID] > accruedCosts[cur] + linkCost) {
                            accruedCosts[neighborID] = accruedCosts[cur] + linkCost;
                            parentIDs[neighborID] = cur;
                        }
                    }
                }
                borderNodeIDs.RemoveAt(0);
            }
            return null;
        }
        public static List<TNode> AStar_PathFinding<TNode>(Func<TNode, List<TNode>> links, Func<TNode, TNode, float> costs, TNode fromNode, TNode toNode) {
            return AStar_PathFinding(links, costs, n => costs.Invoke(n, toNode), fromNode, toNode);
        }


        public static TNode LocalMinimumValueNode<TNode>(Func<TNode, List<TNode>> links, Func<TNode, float> values, TNode fromNode) {
            TNode result = default(TNode);
            float minValue = float.MaxValue;

            List<TNode> visitedNodes = new List<TNode>();
            List<int> borderNodes = new List<int>();
            visitedNodes.Add(fromNode);
            borderNodes.Add(0);

            while (borderNodes.Count > 0) {
                TNode curNode = visitedNodes[borderNodes[0]];
                float curValue = values.Invoke(curNode);
                if (minValue >= curValue) {
                    minValue = curValue;
                    result = curNode;
                    foreach (TNode neighbor in links.Invoke(curNode)) {
                        if (!visitedNodes.Contains(neighbor)) {
                            borderNodes.Add(visitedNodes.Count);
                            visitedNodes.Add(neighbor);
                        }
                    }
                }
                borderNodes.RemoveAt(0);
            }

            return result;
        }
        public static TNode LocalMaximumValueNode<TNode>(Func<TNode, List<TNode>> links, Func<TNode, float> values, TNode fromNode) {
            TNode result = default(TNode);
            float maxValue = float.MinValue;

            List<TNode> visitedNodes = new List<TNode>();
            List<int> borderNodes = new List<int>();
            visitedNodes.Add(fromNode);
            borderNodes.Add(0);

            while (borderNodes.Count > 0) {
                TNode curNode = visitedNodes[borderNodes[0]];
                float curValue = values.Invoke(curNode);
                if (maxValue <= curValue) {
                    maxValue = curValue;
                    result = curNode;
                    foreach (TNode neighbor in links.Invoke(curNode)) {
                        if (!visitedNodes.Contains(neighbor)) {
                            borderNodes.Add(visitedNodes.Count);
                            visitedNodes.Add(neighbor);
                        }
                    }
                }
                borderNodes.RemoveAt(0);
            }

            return result;
        }

    }

}