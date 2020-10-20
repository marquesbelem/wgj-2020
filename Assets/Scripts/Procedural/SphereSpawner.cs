using System;
using System.Collections.Generic;
using UnityEngine;


public class SphereSpawner : MonoBehaviour {

    public float radius = 1;
    public Transform parent;
    public SphericalRegionCircle spawnRegion;
    public float maxTimeRandomizing = 5f;
    [Serializable] public class Spawnable {
        public List<GameObject> prefabs;
        public int quantity;
    }
    public List<Spawnable> spawnables;
    public List<string> obstacleIds;

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
                SpawnObstacle prefabObstacle = prefab.GetComponentInChildren<SpawnObstacle>();
                SphericalRegionCircle prefabRegion = (prefabObstacle != null)? prefabObstacle.region : null;
                Vector3 direction;
                bool zeroDirection, outsideSpawnRegion, blockedBySomeObstacle;
                bool IsStillOnTime() => (curTime - startTime) <= maxTimeRandomizing;
                do {
                    direction = UnityEngine.Random.insideUnitSphere.normalized;
                    curTime = Time.realtimeSinceStartup;
                    zeroDirection = direction == Vector3.zero;
                    outsideSpawnRegion = !spawnRegion.Contains(direction);
                    blockedBySomeObstacle = SpawnObstacle.AnyBlocksSpawn(obstacleIds, prefabRegion, direction);
                } while ((zeroDirection || outsideSpawnRegion || blockedBySomeObstacle) && IsStillOnTime());
                if (!IsStillOnTime()) {
                    Debug.Log("Timed out spawning " + prefab + " #" + (i+1) + "." + (outsideSpawnRegion? " Outside spawn region.":"") + (blockedBySomeObstacle ? " Blocked by some obstacle." : ""));
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
