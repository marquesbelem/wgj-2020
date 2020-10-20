using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalGraph : MonoBehaviour {

    public SphereLine linePrefab;
    public List<GameObject> nodes;
    [Serializable] public class Connection : object {
        public int id1;
        public int id2;

        public Connection(int id1, int id2) {
            this.id1 = id1;
            this.id2 = id2;
        }

        public override bool Equals(object obj) {
            Connection con = (Connection)obj;
            if (con != null) {
                return (id1 == con.id1 && id2 == con.id2)
                    || (id1 == con.id2 && id2 == con.id1);
            }
            return false;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
    public List<Connection> wishedConnections;
    public UnityEvent onCompletion;
    
    private readonly List<Connection> connections = new List<Connection>();
    private readonly List<SphereLine> instantiatedLines = new List<SphereLine>();

    private void OnEnable() => PointerRegionManager.onDragFromTo.AddListener(ProcessDragEvent);
    private void OnDisable() => PointerRegionManager.onDragFromTo.RemoveListener(ProcessDragEvent);

    public void ProcessDragEvent(PointerRegion start, PointerRegion end) {
        int startId = nodes.IndexOf(start.gameObject);
        int endId = nodes.IndexOf(end.gameObject);
        if (startId >= 0 && endId >= 0) {
            Connection newConnection = new Connection(startId, endId);
            Connection existingConnection = connections.Find(c => c.Equals(newConnection));
            if (existingConnection != null) {
                int existingConnectionIndex = connections.IndexOf(existingConnection);
                connections.RemoveAt(existingConnectionIndex);
                Destroy(instantiatedLines[existingConnectionIndex].gameObject);
                instantiatedLines.RemoveAt(existingConnectionIndex);
            }
            else {
                connections.Add(newConnection);
                SphereLine newLine = Instantiate(linePrefab);
                newLine.start = start.region.SphereCoordinatesRef;
                newLine.finish = end.region.SphereCoordinatesRef;
                instantiatedLines.Add(newLine);
            }
        }
    }
    private void Update() {
        if (AllWishedConnectionsMade) {
            PointerRegionManager.startDragRegion = null;
            InputGraphConnection.AllUpdateGraphics();
            onCompletion.Invoke();
        }
    }
    private bool AllWishedConnectionsMade {
        get {
            return wishedConnections.Count == connections.Count && wishedConnections.TrueForAll(w => connections.Contains(w));
        }
    }

}
