using GalloUtils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SphereSpawner : MonoBehaviour {

    public float radius = 1;
    public Transform parent;
    [Serializable] public class Spawnable {
        public List<GameObject> prefabs;
        public int quantity;
    }
    public List<Spawnable> spawnables;
    public float spawnRegionAngularTreshold = 30f;
    public float SpawnRegionRadiansTreshold => Mathf.PI * 2f * spawnRegionAngularTreshold / 360f;

    public List<string> obstacleIds;

    public bool IsGlobal => transform.position == Vector3.zero;

    void Start() {
        foreach (Spawnable spawnable in spawnables) {
            for (int i = 0; i < spawnable.quantity; i++) {
                GameObject prefab = spawnable.prefabs.RandomElement();
                float prefabRegionSize = 0f;
                SpawnObstacle prefabObstacle = prefab.GetComponent<SpawnObstacle>();
                if (prefabObstacle != null) {
                    prefabRegionSize = prefabObstacle.angularTreshold;
                }
                Vector3 direction;
                do {
                    direction = UnityEngine.Random.insideUnitSphere.normalized;
                } while (direction == Vector3.zero 
                    || SpawnObstacle.instanceList.Exists(o => (obstacleIds.Count == 0 || obstacleIds.Exists(id => o.identifiers.Contains(id))) && Vector3.Angle(o.transform.position.normalized, direction) <= o.angularTreshold + prefabRegionSize)
                    || (!IsGlobal && Vector3.Angle(transform.position.normalized, direction) >= spawnRegionAngularTreshold));
                Transform newObj = Instantiate(prefab).transform;
                newObj.position = direction.normalized * radius;
                newObj.up = direction;
                newObj.parent = parent;
            }
        }
    }
    private void OnDrawGizmosSelected() {
        if (IsGlobal) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        else {
            float cos = Mathf.Cos(SpawnRegionRadiansTreshold);
            float sin = Mathf.Sin(SpawnRegionRadiansTreshold);
            float posMagnitude = transform.position.magnitude;
            Vector3 posNormalized = transform.position / posMagnitude;
            Handles.color = Color.green;
            Handles.DrawWireDisc(posNormalized * radius * cos, posNormalized, posMagnitude * sin);
            Handles.DrawWireArc(Vector3.zero, Vector3.right, posNormalized, spawnRegionAngularTreshold, posMagnitude);
            Handles.DrawWireArc(Vector3.zero, -Vector3.right, posNormalized, spawnRegionAngularTreshold, posMagnitude);
            Handles.DrawWireArc(Vector3.zero, Vector3.forward, posNormalized, spawnRegionAngularTreshold, posMagnitude);
            Handles.DrawWireArc(Vector3.zero, -Vector3.forward, posNormalized, spawnRegionAngularTreshold, posMagnitude);
        }
    }

}
