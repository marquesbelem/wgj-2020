using System;
using System.Collections.Generic;
using UnityEngine;


public class SphereSpawner : MonoBehaviour {

    public float radius = 1;
    public Transform parent;
    [Serializable] public class Spawnable {
        public List<GameObject> prefabs;
        public int quantity;
    }
    public List<Spawnable> spawnables;
    public SphereRegion spawnRegion;

    public List<string> obstacleIds;
    public float maxTimeRandomizing = 5f;

    private float startTime;
    private float curTime;

    void Start() {
        TrySpawnAll();
    }
    public void TrySpawnAll() {
        foreach (Spawnable spawnable in spawnables) {
            startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < spawnable.quantity; i++) {
                GameObject prefab = spawnable.prefabs.RandomElement();
                SpawnObstacle prefabObstacle = prefab.GetComponent<SpawnObstacle>();
                SphereRegion prefabRegion = (prefabObstacle != null)? prefabObstacle.region : null;
                Vector3 direction;
                bool IsStillOnTime() => (curTime - startTime) <= maxTimeRandomizing;
                do {
                    direction = UnityEngine.Random.insideUnitSphere.normalized;
                    curTime = Time.realtimeSinceStartup;
                } while ((direction == Vector3.zero || !spawnRegion.Contains(direction) || SpawnObstacle.AnyBlocksSpawn(obstacleIds, prefabRegion, direction)) && IsStillOnTime());
                if (!IsStillOnTime()) {
                    Debug.Log("Timed out spawning " + prefab + " #" + (i+1));
                    break;
                }
                Transform newObj = Instantiate(prefab).transform;
                newObj.position = direction.normalized * radius;
                newObj.up = direction;
                newObj.parent = parent;
            }
            Debug.Log("Time spent spawning: " + (Time.realtimeSinceStartup - startTime));
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (spawnRegion != null) {
            spawnRegion.DrawGizmos(Color.green);
        }
    }
#endif

}
