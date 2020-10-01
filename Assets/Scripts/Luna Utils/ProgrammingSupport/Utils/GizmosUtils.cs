using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static class GizmosUtils {
    
        public static void DrawPath(List<Vector3> path, bool ciclic) {
            path.ForEachConsecutivePair(Gizmos.DrawLine, ciclic);
        }

        public static void DrawRect(Vector3 position, Vector2 size, Quaternion rotation) {
            List<Vector3> path = new List<Vector3>();
            path.Add(position + (rotation * new Vector2( size.x / 2f,  size.y / 2f)));
            path.Add(position + (rotation * new Vector2(-size.x / 2f,  size.y / 2f)));
            path.Add(position + (rotation * new Vector2(-size.x / 2f, -size.y / 2f)));
            path.Add(position + (rotation * new Vector2( size.x / 2f, -size.y / 2f)));
            DrawPath(path, true);
        }
        public static void DrawCube(Vector3 position, Vector3 size, Quaternion rotation) {
            Vector3[,,] vertices = new Vector3[2, 2, 2];
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    for (int k = 0; k < 2; k++) {
                        vertices[i, j, k] = position + (rotation * new Vector3(size.x * (i*2 - 1f) * 0.5f, size.y * (j*2 - 1f) * 0.5f, size.z * (k*2 - 1f) * 0.5f));
                    }
                }
            }
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    Gizmos.DrawLine(vertices[i,j,0], vertices[i,j,1]);
                    Gizmos.DrawLine(vertices[i,0,j], vertices[i,1,j]);
                    Gizmos.DrawLine(vertices[0,i,j], vertices[1,i,j]);
                }
            }
        }

        public static void DrawCircle(Vector3 center, float radius, Quaternion rotation, Vector3 startDir, Vector3 axis, float maxAmgleStep) {
            DrawArc(center, radius, rotation, startDir, axis, 360f, maxAmgleStep);
        }
        public static void DrawArc(Vector3 center, float radius, Quaternion rotation, Vector3 startDir, Vector3 axis, float arcAngle, float maxAmgleStep) {
            Vector3 baseVector = rotation * startDir * radius;
            int segmentCount = Mathf.CeilToInt(arcAngle / maxAmgleStep);
            List<Vector3> path = CollectionUtils.CreateListByIndex(segmentCount + 1, i => center + (Quaternion.AngleAxis(arcAngle * i / segmentCount, axis) * baseVector));
            DrawPath(path, false);
        }

        public static void DrawSquareGrid(Vector3 center, Vector2 cellSize, Vector2Int cellCount, Quaternion rotation, float cellDrawScale = 1f) {
            Vector2 posFromCenter = -((Vector2)cellCount / 2f).ScaledBy(cellSize);
            foreach (Vector2Int pos in RectIntUtils.Enumerate(cellCount)) {
                DrawRect(center + (rotation * (posFromCenter + (pos + (0.5f).ToVector2()).ScaledBy(cellSize))), cellSize * cellDrawScale, rotation );
            }
        }

    }

}
