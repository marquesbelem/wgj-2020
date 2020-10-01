using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class MeshExtension {

        public static Vector3 RandomPoint(this Mesh mesh) {
            List<List<float>> triAreas = CollectionUtils.NewNestedList<float>(mesh.subMeshCount);
            List<Vector2Int> triIds = new List<Vector2Int>();
            mesh.ForEachTriangleId(id => {
                triIds.Add(id);
                triAreas[id.x].Add(mesh.TriArea(id));
            });
            Vector2Int chosenTriId = triIds.RandomElement(id => triAreas[id.x][id.y]);
            int[] tris = mesh.GetTriangles(chosenTriId.x);
            Vector3[] vertices = mesh.vertices;
            int tri = tris[chosenTriId.y * 3];
            Vector3 origin = vertices[tri];
            Vector3 u = vertices[tri + 1] - origin;
            Vector3 v = vertices[tri + 2] - origin;
            return RandomUtils.RandomPointInTriangle(origin, u, v);
        }

        public static void ForEachTriangleId(this Mesh mesh, Action<Vector2Int> triIdAction) {
            for (int i = 0; i < mesh.subMeshCount; i++) {
                int[] tris = mesh.GetTriangles(i);
                for (int j = 0; j < tris.Length / 3; j++) {
                    triIdAction.Invoke(new Vector2Int(i, j));
                }
            }
        }

        public static float TriArea(this Mesh mesh, Vector2Int triId) {
            int[] tris = mesh.GetTriangles(triId.x);
            return mesh.TriArea(tris[triId.y], tris[triId.y + 1], tris[triId.y + 2]);
        }
        public static float TriArea(this Mesh mesh, int v1, int v2, int v3) {
            Vector3[] vertices = mesh.vertices;
            Vector3 u = vertices[v2] - vertices[v1];
            Vector3 v = vertices[v3] - vertices[v1];
            return TriArea(u, v);
        }
        public static float TriArea(Vector3 u, Vector3 v) {
            return Vector3.Cross(u, v).magnitude / 2f;
        }


        public static List<int> TrisContainigEdge(this Mesh mesh, int v1, int v2) {
            List<int> result = new List<int>();
            for (int i = 0; i < mesh.triangles.Length; i += 3) {
                if (TriContainsEdge(mesh, i, v1, v2)) {
                    result.Add(i);
                }
            }
            return result;
        }
        public static bool TriContainsEdge(this Mesh mesh, int tri, int v1, int v2) {
            for (int i = 0; i < 3; i++) {
                int j = (i + 1) % 3;
                if ((v1 == mesh.triangles[tri + i]) && (v2 == mesh.triangles[tri + j])) {
                    return true;
                }
                if ((v2 == mesh.triangles[tri + i]) && (v1 == mesh.triangles[tri + j])) {
                    return true;
                }
            }
            return false;
        }

        public static List<int> EdgeIndexInTriList(this Mesh mesh, List<int> triList, int v1, int v2) {
            List<int> result = new List<int>();
            for (int i = 0; i < triList.Count; i++) {
                result.Add(EdgeIndexInTri(mesh, triList[i], v1, v2));
            }
            return result;
        }
        public static int EdgeIndexInTri(this Mesh mesh, int tri, int v1, int v2) {
            for (int i = 0; i < 3; i++) {
                int j = (i + 1) % 3;
                if ((v1 == mesh.triangles[tri + i]) && (v2 == mesh.triangles[tri + j])) {
                    return i;
                }
                if ((v2 == mesh.triangles[tri + i]) && (v1 == mesh.triangles[tri + j])) {
                    return i;
                }
            }
            return -1;
        }

        public static bool IsInnerVertex(this Mesh mesh, int vertexId) {
            return IsVertexListCiclic(mesh, GetVertexNeighbors(mesh, vertexId));
        }
        public static List<int> GetVertexNeighbors(this Mesh mesh, int vertexId) {
            List<int> vertexNeighborIDs = new List<int>();
            for (int i = 0; i < mesh.triangles.Length; i += 3) {
                for (int j = 0; j < 3; j++) {
                    if (mesh.triangles[i + j] == vertexId) {
                        for (int k = 0; k < 3; k++) {
                            if (j != k) {
                                int curNeighborId = mesh.triangles[i + k];
                                if (!vertexNeighborIDs.Contains(curNeighborId)) {
                                    vertexNeighborIDs.Add(curNeighborId);
                                }
                            }
                        }
                    }
                }
            }
            return vertexNeighborIDs;
        }
        public static bool IsVertexListCiclic(this Mesh mesh, List<int> vertexList) {
            if (vertexList != null) {
                if (vertexList.Count > 2) {
                    int curVertex = vertexList[0];
                    List<int> closedList = new List<int>();
                    List<int> openList = new List<int>();
                    openList.AddRange(vertexList);
                    openList.RemoveAt(0);
                    closedList.Add(curVertex);
                    return IsSemiCiclePossible(mesh, closedList, openList);
                }
            }
            return false;
        }
        private static bool IsSemiCiclePossible(this Mesh mesh, List<int> inClosedList, List<int> inOpenList) {
            if ((inClosedList != null) && (inOpenList != null)) {
                int curVertex = inClosedList[inClosedList.Count - 1];
                if (inOpenList.Count == 0) {
                    return AreVertexNeighbors(mesh, inClosedList[0], curVertex);
                }

                List<int> possibleNeighbors = inOpenList.FindAll(v => AreVertexNeighbors(mesh, v, curVertex));
                foreach (int vertex in possibleNeighbors) {
                    List<int> closedList = new List<int>();
                    List<int> openList = new List<int>();
                    openList.AddRange(inOpenList);
                    openList.Remove(vertex);
                    closedList.AddRange(inClosedList);
                    closedList.Add(vertex);
                    if (IsSemiCiclePossible(mesh, closedList, openList)) {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool AreVertexNeighbors(this Mesh mesh, int v1, int v2) {
            for (int i = 0; i < mesh.triangles.Length; i += 3) {
                for (int j = 0; j < 3; j++) {
                    if (mesh.triangles[i + j] == v1) {
                        for (int k = 0; k < 3; k++) {
                            if (j != k) {
                                if (mesh.triangles[i + k] == v2) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static void DissolveVertex(this Mesh mesh, int vertexId, int submesh) {
            BorderFillConvexPoly(mesh, GetVertexNeighbors(mesh, vertexId), submesh);
            DeleteVertex(mesh, vertexId);
        }
        public static void DeleteVertex(this Mesh mesh, int vertexId) {
            List<Vector3> vertexList = new List<Vector3>();
            vertexList.AddRange(mesh.vertices);

            List<Vector3> normalList = new List<Vector3>();
            normalList.AddRange(mesh.normals);

            List<Vector2> uvList = new List<Vector2>();
            uvList.AddRange(mesh.uv);

            for (int i = 0; i < mesh.subMeshCount; i++) {
                List<int> triList = new List<int>();
                triList.AddRange(mesh.GetTriangles(i));

                //Finding triangles that contains the vertex
                List<int> triIDs = new List<int>();
                for (int j = 0; j < triList.Count; j++) {
                    if (triList[j] == vertexId) {
                        int mod = j % 3;
                        triIDs.Add(j - mod);
                    }
                }
                triIDs.Reverse();
                //Removing triangles that contains the vertex
                foreach (int triID in triIDs) {
                    triList.RemoveAt(triID);
                    triList.RemoveAt(triID);
                    triList.RemoveAt(triID);
                }

                //Fixing vertex indexing
                for (int j = 0; j < triList.Count; j++) {
                    if (triList[j] > vertexId) {
                        triList[j]--;
                    }
                }

                mesh.SetTriangles(triList, i);
            }

            //Removing vertex
            vertexList.RemoveAt(vertexId);
            normalList.RemoveAt(vertexId);
            uvList.RemoveAt(vertexId);

            mesh.SetVertices(vertexList);
            mesh.SetNormals(normalList);
            mesh.SetUVs(0, uvList);
        }

        public static void FanFillConvexPoly(this Mesh mesh, List<int> polyVertices, int submesh) {
            int newTrisCount = polyVertices.Count - 2;
            for (int i = 1; i <= newTrisCount; i++) {
                AddTri(mesh, polyVertices[0], polyVertices[i], polyVertices[i + 1], submesh);
            }
        }
        public static void FanFillConvexPoly(this Mesh mesh, List<int> polyVertices, int submesh, Vector3 forceTriFacingDirection) {
            int newTrisCount = polyVertices.Count - 2;
            for (int i = 1; i <= newTrisCount; i++) {
                AddTri(mesh, polyVertices[0], polyVertices[i], polyVertices[i + 1], submesh, forceTriFacingDirection);
            }
        }
        public static void FanFillConvexPolyFacingNormals(this Mesh mesh, List<int> polyVertices, int submesh) {
            List<Vector3> normalList = polyVertices.ConvertAll(i => mesh.normals[i]);
            mesh.FanFillConvexPoly(polyVertices, submesh, normalList.Mean().normalized);
        }

        public static void BorderFillConvexPoly(this Mesh mesh, List<int> polyVertices, int submesh) {
            while (polyVertices.Count > 2) {
                for (int i = 0; i + 2 <= polyVertices.Count; i++) {
                    AddTri(mesh, polyVertices[i], polyVertices[i + 1], polyVertices[(i + 2) % polyVertices.Count], submesh);
                    polyVertices.RemoveAt(i + 1);
                }
            }
        }
        public static void BorderFillConvexPoly(this Mesh mesh, List<int> polyVertices, int submesh, Vector3 forceTriFacingDirection) {
            while (polyVertices.Count > 2) {
                for (int i = 0; i + 2 <= polyVertices.Count; i++) {
                    AddTri(mesh, polyVertices[i], polyVertices[i + 1], polyVertices[(i + 2) % polyVertices.Count], submesh, forceTriFacingDirection);
                    polyVertices.RemoveAt(i + 1);
                }
            }
        }
        public static void BorderFillConvexPolyFacingNormals(this Mesh mesh, List<int> polyVertices, int submesh) {
            List<Vector3> normalList = polyVertices.ConvertAll(i => mesh.normals[i]);
            mesh.BorderFillConvexPoly(polyVertices, submesh, normalList.Mean().normalized);
        }

        public static int AddVertex(this Mesh mesh, Vector3 position, Vector3 normal) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);

            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);

            int index = newVertexList.Count;
            newVertexList.Add(position);
            newNormalList.Add(normal);

            mesh.SetVertices(newVertexList);
            mesh.SetNormals(newNormalList);

            return index;
        }
        public static int AddVertex(this Mesh mesh, Vector3 position, Vector3 normal, Vector2 uv) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);

            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);

            List<Vector2> newUVList = new List<Vector2>();
            newUVList.AddRange(mesh.uv);

            int index = newVertexList.Count;
            newVertexList.Add(position);
            newNormalList.Add(normal);
            newUVList.Add(uv);

            mesh.SetVertices(newVertexList);
            mesh.SetNormals(newNormalList);
            mesh.SetUVs(0, newUVList);

            return index;
        }

        public static void AddTri(this Mesh mesh, int i0, int i1, int i2, int submesh) {
            List<int> newTriangleList = new List<int>();
            newTriangleList.AddRange(mesh.GetTriangles(submesh));

            newTriangleList.Add(i0);
            newTriangleList.Add(i1);
            newTriangleList.Add(i2);

            mesh.SetTriangles(newTriangleList, submesh);

        }
        public static void AddTri(this Mesh mesh, int i0, int i1, int i2, int submesh, Vector3 forceFacingDirection) {
            List<int> newTriangleList = new List<int>();
            newTriangleList.AddRange(mesh.GetTriangles(submesh));

            Vector3 v0 = mesh.vertices[i0];
            Vector3 v1 = mesh.vertices[i1];
            Vector3 v2 = mesh.vertices[i2];
            Vector3 facingDirection = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            if (Vector3.Dot(forceFacingDirection, facingDirection) >= 0) {
                newTriangleList.Add(i0);
                newTriangleList.Add(i1);
                newTriangleList.Add(i2);
            }
            else {
                newTriangleList.Add(i0);
                newTriangleList.Add(i2);
                newTriangleList.Add(i1);
            }

            mesh.SetTriangles(newTriangleList, submesh);

        }
        public static void AddTriFacingNormals(this Mesh mesh, int i0, int i1, int i2, int submesh) {
            List<Vector3> normalList = new List<Vector3>() { mesh.normals[i0], mesh.normals[i1], mesh.normals[i2] };
            mesh.AddTri(i0, i1, i2, submesh, normalList.Mean().normalized);
        }

        public static void AddConvexPoly(this Mesh mesh, List<int> polyVertices, int submesh) {
            BorderFillConvexPoly(mesh, polyVertices, submesh);
        }
        public static void AddConvexPoly(this Mesh mesh, List<int> polyVertices, int submesh, Vector3 forceTriFacingDirection) {
            BorderFillConvexPoly(mesh, polyVertices, submesh, forceTriFacingDirection);
        }
        public static void AddConvexPolyFacingNormals(this Mesh mesh, List<int> polyVertices, int submesh) {
            BorderFillConvexPolyFacingNormals(mesh, polyVertices, submesh);
        }

        public static int AddMesh(this Mesh thisMesh, Mesh mesh) {
            if (thisMesh.subMeshCount < mesh.subMeshCount) {
                thisMesh.subMeshCount = mesh.subMeshCount;
            }
            int n = thisMesh.vertexCount;
            if (mesh.uv.Length > 0) {
                for (int i = 0; i < mesh.vertexCount; i++) {
                    thisMesh.AddVertex(mesh.vertices[i], mesh.normals[i], mesh.uv[i]);
                }
            }
            else {
                for (int i = 0; i < mesh.vertexCount; i++) {
                    thisMesh.AddVertex(mesh.vertices[i], mesh.normals[i]);
                }
            }
            for (int i = 0; i < mesh.subMeshCount; i++) {
                int[] tris = mesh.GetTriangles(i);
                for (int j = 0; j < tris.Length; j += 3) {
                    thisMesh.AddTri(tris[j] + n, tris[j + 1] + n, tris[j + 2] + n, i);
                }
            }
            return n;
        }
        public static int AddMeshAsSubmesh(this Mesh thisMesh, Mesh mesh, int subMesh) {
            int n = thisMesh.vertexCount;
            if (mesh.uv.Length > 0) {
                for (int i = 0; i < mesh.vertexCount; i++) {
                    thisMesh.AddVertex(mesh.vertices[i], mesh.normals[i], mesh.uv[i]);
                }
            }
            else {
                for (int i = 0; i < mesh.vertexCount; i++) {
                    thisMesh.AddVertex(mesh.vertices[i], mesh.normals[i]);
                }
            }
            for (int i = 0; i < mesh.subMeshCount; i++) {
                int[] tris = mesh.GetTriangles(i);
                for (int j = 0; j < tris.Length; j += 3) {
                    thisMesh.AddTri(tris[j] + n, tris[j + 1] + n, tris[j + 2] + n, subMesh);
                }
            }
            return n;
        }

        public static void SetVertexPosition(this Mesh mesh, int vertexID, Vector3 newPosition) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            newVertexList[vertexID] = newPosition;
            mesh.SetVertices(newVertexList);
        }
        public static void AddVertexPosition(this Mesh mesh, int vertexID, Vector3 offset) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            newVertexList[vertexID] += offset;
            mesh.SetVertices(newVertexList);
        }
        public static void SetVertexPositions(this Mesh mesh, List<int> vertexIDs, Vector3 newPosition) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            foreach (int vertexID in vertexIDs) {
                newVertexList[vertexID] = newPosition;
            }
            mesh.SetVertices(newVertexList);
        }
        public static void AddVertexPositions(this Mesh mesh, List<int> vertexIDs, Vector3 offset) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            foreach (int vertexID in vertexIDs) {
                newVertexList[vertexID] += offset;
            }
            mesh.SetVertices(newVertexList);
        }
        public static void AddVertexPositions(this Mesh mesh, int index, Vector3 offset) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            for (int i = index; i < newVertexList.Count; i++) {
                newVertexList[i] += offset;
            }
            mesh.SetVertices(newVertexList);
        }
        public static void SetVertexPositions(this Mesh mesh, List<int> vertexIDs, List<Vector3> newPositions) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            for (int i = 0; i < vertexIDs.Count; i++) {
                int vertexID = vertexIDs[i];
                newVertexList[vertexID] = newPositions[i];
            }
            mesh.SetVertices(newVertexList);
        }
        public static void AddVertexPositions(this Mesh mesh, List<int> vertexIDs, List<Vector3> offsets) {
            List<Vector3> newVertexList = new List<Vector3>();
            newVertexList.AddRange(mesh.vertices);
            for (int i = 0; i < vertexIDs.Count; i++) {
                int vertexID = vertexIDs[i];
                newVertexList[vertexID] += offsets[i];
            }
            mesh.SetVertices(newVertexList);
        }

        public static void SetVertexNormal(this Mesh mesh, int vertexID, Vector3 newNormal) {
            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);
            newNormalList[vertexID] = newNormal;
            mesh.SetNormals(newNormalList);
        }
        public static void AddVertexNormal(this Mesh mesh, int vertexID, Vector3 offset) {
            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);
            newNormalList[vertexID] += offset;
            mesh.SetNormals(newNormalList);
        }
        public static void SetVertexNormals(this Mesh mesh, List<int> vertexIDs, Vector3 newNormal) {
            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);
            foreach (int vertexID in vertexIDs) {
                newNormalList[vertexID] = newNormal;
            }
            mesh.SetNormals(newNormalList);
        }
        public static void AddVertexNormals(this Mesh mesh, List<int> vertexIDs, Vector3 offset) {
            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);
            foreach (int vertexID in vertexIDs) {
                newNormalList[vertexID] += offset;
            }
            mesh.SetNormals(newNormalList);
        }
        public static void SetVertexNormals(this Mesh mesh, List<int> vertexIDs, List<Vector3> newNormals) {
            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);
            for (int i = 0; i < vertexIDs.Count; i++) {
                int vertexID = vertexIDs[i];
                newNormalList[vertexID] = newNormals[i];
            }
            mesh.SetNormals(newNormalList);
        }
        public static void AddVertexNormals(this Mesh mesh, List<int> vertexIDs, List<Vector3> offsets) {
            List<Vector3> newNormalList = new List<Vector3>();
            newNormalList.AddRange(mesh.normals);
            for (int i = 0; i < vertexIDs.Count; i++) {
                int vertexID = vertexIDs[i];
                newNormalList[vertexID] += offsets[i];
            }
            mesh.SetNormals(newNormalList);
        }

        public static void SolveTangents(this Mesh mesh) {
            int triangleCount = mesh.triangles.Length;
            int vertexCount = mesh.vertices.Length;

            Vector3[] tan1 = new Vector3[vertexCount];
            Vector3[] tan2 = new Vector3[vertexCount];
            Vector4[] tangents = new Vector4[vertexCount];
            for (long a = 0; a < triangleCount; a += 3) {
                long i1 = mesh.triangles[a + 0];
                long i2 = mesh.triangles[a + 1];
                long i3 = mesh.triangles[a + 2];
                Vector3 v1 = mesh.vertices[i1];
                Vector3 v2 = mesh.vertices[i2];
                Vector3 v3 = mesh.vertices[i3];
                Vector2 w1 = mesh.uv[i1];
                Vector2 w2 = mesh.uv[i2];
                Vector2 w3 = mesh.uv[i3];
                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;
                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;
                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;
                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;
            }
            for (long a = 0; a < vertexCount; ++a) {
                Vector3 n = mesh.normals[a];
                Vector3 t = tan1[a];
                Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
                tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
                tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
            }
            mesh.tangents = tangents;
        }


        public static int DuplicateVertex(this Mesh mesh, int vertex) {
            return mesh.AddVertex(mesh.vertices[vertex], mesh.normals[vertex], mesh.uv[vertex]);
        }
        public static int DuplicateVertex(this Mesh mesh, int vertex, Vector3 offset) {
            return mesh.AddVertex(mesh.vertices[vertex] + offset, mesh.normals[vertex], mesh.uv[vertex]);
        }

        public static List<int> DuplicateEdge(this Mesh mesh, int tri, int edge) {
            edge = edge % 3;
            return new List<int> {
            mesh.DuplicateVertex(mesh.triangles[tri + edge]),
            mesh.DuplicateVertex(mesh.triangles[tri + ((edge + 1) % 3)])
        };
        }
        public static List<int> DuplicateEdge(this Mesh mesh, int tri, int edge, Vector3 offset) {
            edge = edge % 3;
            return new List<int> {
            mesh.DuplicateVertex(mesh.triangles[tri + edge], offset),
            mesh.DuplicateVertex(mesh.triangles[tri + ((edge + 1) % 3)], offset)
        };
        }

        public static List<int> ExtrudeEdge(this Mesh mesh, int tri, int edge, Vector3 offset, int submesh) {
            List<int> newVertices = mesh.DuplicateEdge(tri, edge, offset);
            List<int> allVertices = new List<int>(newVertices);
            mesh.AddConvexPolyFacingNormals(allVertices, submesh);
            return newVertices;
        }

    }

}