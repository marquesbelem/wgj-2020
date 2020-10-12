using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnObstacle : MonoBehaviour {

    public static List<SpawnObstacle> instanceList = new List<SpawnObstacle>();
    private void Awake() => instanceList.Add(this);
    private void OnDestroy() => instanceList.Remove(this);
    public static bool AnyBlocksSpawn(List<string> possibilities, SphereRegion other, Vector3 hypotheticalUp) => instanceList.Exists(i => i.BlocksSpawn(possibilities, other, hypotheticalUp));

    public SphereRegion region;
    public string identifier;

    public bool BlocksSpawn(List<string> possibilities, SphereRegion other, Vector3 hypotheticalUp) => MatchesIdentifiers(possibilities) && region != null && region.WouldOverlap(other, hypotheticalUp);
    public bool MatchesIdentifiers(List<string> possibilities) => possibilities.Count == 0 || possibilities.Contains(identifier);

#if UNITY_EDITOR
    public void OnDrawGizmosSelected() {
        if (region != null) {
            region.DrawGizmos(Color.red);
        }
    }
#endif

}
