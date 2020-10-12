using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCompletion : MonoBehaviour {

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

    private List<Connection> connections = new List<Connection>();

    private void OnEnable() {
        PointerEventRegion.onDragFromTo.AddListener(ProcessDragEvent);
    }
    private void OnDisable() {
        PointerEventRegion.onDragFromTo.RemoveListener(ProcessDragEvent);
    }

    public void ProcessDragEvent(PointerEventRegion start, PointerEventRegion end) {
        int startId = nodes.IndexOf(start.gameObject);
        int endId = nodes.IndexOf(end.gameObject);
        if (startId >= 0 && endId >= 0) {
            Connection newConnection = new Connection(startId, endId);
            Connection existingConnection = connections.Find(c => c.Equals(newConnection));
        }
    }

}
