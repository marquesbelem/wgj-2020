using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class GeometryUtils {

        #region 2D

        #region Directions
        public static bool IsBetweenDirections(Vector2 v, Vector2 dir1, Vector2 dir2, bool inclusive = true) {
            float angleV = Vector2.SignedAngle(dir1, v);
            float angleD2 = Vector2.SignedAngle(dir1, dir2);
            if (inclusive) {
                return angleV == 0f || (angleV.Sign() == angleD2.Sign() && angleV.Abs() <= angleD2.Abs());
            }
            else {
                return angleV.Sign() == angleD2.Sign() && angleV.Abs() < angleD2.Abs();
            }
        }
        public static bool SameDirection(Vector2 dir1, Vector2 dir2, bool allowOpposite = false, float angleTreshold = 0f) {
            float angle = Vector2.Angle(dir1, dir2);
            if (allowOpposite && angle > 90f) {
                angle = Vector2.Angle(dir1, -dir2);
            }
            return angle <= angleTreshold;
        }
        #endregion

        #region Lines
        public static bool AreParallelLines(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float angleTreshold = 0f) {
            return ((v1.x - v2.x) * (v3.y - v4.y) - (v1.y - v2.y) * (v3.x - v4.x)) == angleTreshold;
        }
        public static Vector2? LineLineIntersection(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, bool limitedLines = true) {
            float denominator = (v1.x - v2.x) * (v3.y - v4.y) - (v1.y - v2.y) * (v3.x - v4.x);
            if (denominator == 0f) {
                return null;
            }
            else {
                float t = ((v1.x - v3.x) * (v3.y - v4.y) - (v1.y - v3.y) * (v3.x - v4.x)) / denominator;
                return new Vector2(v1.x + t*(v2.x - v1.x), v1.y + t * (v2.y - v1.y));
            }
        }

        public static List<Vector2> HorizontalLine(float y, float x0, float x1) {
            return new List<Vector2>() { new Vector2(x0, y), new Vector2(x1, y) };
        }
        public static List<Vector2> VerticalLine(float x, float y0, float y1) {
            return new List<Vector2>() { new Vector2(x, y0), new Vector2(x, y1) };
        }

        public static List<Vector2Int> HorizontalLine(int y, int x0, int x1) {
            return new List<Vector2Int>() { new Vector2Int(x0, y), new Vector2Int(x1, y) };
        }
        public static List<Vector2Int> VerticalLine(int x, int y0, int y1) {
            return new List<Vector2Int>() { new Vector2Int(x, y0), new Vector2Int(x, y1) };
        }

        public static Vector2 Midpoint(Vector2 v1, Vector2 v2) {
            return (v1 + v2) / 2f;
        }
        #endregion

        #region Polygons
        #region Square
        public static partial class Square {
            public static List<Vector2> Vertices(float scale) {
            return new List<Vector2>() {
                new Vector2(-scale / 2f,  scale / 2f),
                new Vector2( scale / 2f,  scale / 2f),
                new Vector2( scale / 2f, -scale / 2f),
                new Vector2(-scale / 2f, -scale / 2f)
            };
            }

            public static List<Vector2> BoundVerticalLines(Rect rect) {
                List<Vector2> result = new List<Vector2>();
                result.AddRange(VerticalLine(rect.xMax, rect.yMin, rect.yMax));
                result.AddRange(VerticalLine(rect.xMin, rect.yMin, rect.yMax));
                return result;
            }
            public static List<Vector2> BoundHorizontalLines(Rect rect) {
                List<Vector2> result = new List<Vector2>();
                result.AddRange(HorizontalLine(rect.yMax, rect.xMin, rect.xMax));
                result.AddRange(HorizontalLine(rect.yMin, rect.xMin, rect.xMax));
                return result;
            }
            public static List<Vector2Int> BoundVerticalLines(RectInt rect) {
                List<Vector2Int> result = new List<Vector2Int>();
                result.AddRange(VerticalLine(rect.xMax - 1, rect.yMin, rect.yMax - 1));
                result.AddRange(VerticalLine(rect.xMin, rect.yMin, rect.yMax - 1));
                return result;
            }
            public static List<Vector2Int> BoundHorizontalLines(RectInt rect) {
                List<Vector2Int> result = new List<Vector2Int>();
                result.AddRange(HorizontalLine(rect.yMax - 1, rect.xMin, rect.xMax - 1));
                result.AddRange(HorizontalLine(rect.yMin, rect.xMin, rect.xMax - 1));
                return result;
            }

        }
        #endregion

        #region Hexagon
        public static partial class Hexagon {

            public static readonly float sqrt3 = Mathf.Sqrt(3f);

            public enum Orientation {
                PointyTop,
                FlatTop
            }

            public static float WidthFromSide(float side, Orientation orientation) {
                switch (orientation) {
                    case Orientation.PointyTop:
                        return sqrt3 * side;
                    case Orientation.FlatTop:
                        return 2f * side;
                    default:
                        return 0f;
                }
            }
            public static float HeightFromSide(float side, Orientation orientation) {
                switch (orientation) {
                    case Orientation.PointyTop:
                        return 2f * side;
                    case Orientation.FlatTop:
                        return sqrt3 * side;
                    default:
                        return 0f;
                }
            }
            public static float SideFromWidth(float width, Orientation orientation) {
                switch (orientation) {
                    case Orientation.PointyTop:
                        return width / sqrt3;
                    case Orientation.FlatTop:
                        return width / 2f;
                    default:
                        return 0f;
                }
            }
            public static float SideFromHeight(float height, Orientation orientation) {
                switch (orientation) {
                    case Orientation.PointyTop:
                        return height / 2f;
                    case Orientation.FlatTop:
                        return height / sqrt3;
                    default:
                        return 0f;
                }
            }
            public static List<Vector2> VertexPositions(float scale, Orientation orientation) {
                float width;
                float height;
                switch (orientation) {
                    case Orientation.PointyTop:
                        width = scale;
                        height = HeightFromSide(SideFromWidth(width, orientation), orientation);
                        return new List<Vector2>() {
                            new Vector2( width / 2f,  height / 4f),
                            new Vector2( width / 2f, -height / 4f),
                            new Vector2(         0f, -height / 2f),
                            new Vector2(-width / 2f, -height / 4f),
                            new Vector2(-width / 2f,  height / 4f),
                            new Vector2(         0f,  height / 2f)
                        };
                    case Orientation.FlatTop:
                        height = scale;
                        width = WidthFromSide(SideFromHeight(height, orientation), orientation);
                        return new List<Vector2>() {
                            new Vector2( width / 4f,  height / 2f),
                            new Vector2( width / 2f,           0f),
                            new Vector2( width / 4f, -height / 2f),
                            new Vector2(-width / 4f, -height / 2f),
                            new Vector2(-width / 2f,           0f),
                            new Vector2(-width / 4f,  height / 2f)
                        };
                    default:
                        return null;
                }
            }

            #region Grid
            private static readonly float[,] pothMatrix = new float[2, 2] {
                { sqrt3 / 3f, -1f / 3f },
                {         0f,  2f / 3f }
            };
            private static readonly float[,] fothMatrix = new float[2, 2] {
                {  2f / 3f,         0f },
                { -1f / 3f, sqrt3 / 3f }
            };
            private static readonly float[,] phtoMatrix = new float[2, 2] {
                { sqrt3, sqrt3 / 2f },
                {    0f,       1.5f }
            };
            private static readonly float[,] fhtoMatrix = new float[2, 2] {
                {       1.5f,    0f },
                { sqrt3 / 2f, sqrt3 }
            };

            public static Vector2 OrthonormalToHex(Vector2 orthonormal, Orientation orientation) {
                switch (orientation) {
                    case Orientation.PointyTop:
                        return orthonormal.MultipliedBy(pothMatrix) * sqrt3;
                    case Orientation.FlatTop:
                        return orthonormal.MultipliedBy(fothMatrix) * sqrt3;
                    default:
                        return default;
                }
            }
            public static Vector2 HexToOrthonormal(Vector2 hex, Orientation orientation) {
                switch (orientation) {
                    case Orientation.PointyTop:
                        return hex.MultipliedBy(phtoMatrix) / sqrt3;
                    case Orientation.FlatTop:
                        return hex.MultipliedBy(fhtoMatrix) / sqrt3;
                    default:
                        return default;
                }
            }

            public static Vector2Int HexRound(Vector2 hex) {
                return CubeToHex(CubeRound(HexToCube(hex)));
            }
            public static Vector3Int CubeRound(Vector3 cube) {
                int rx = Mathf.RoundToInt(cube.x);
                int ry = Mathf.RoundToInt(cube.y);
                int rz = Mathf.RoundToInt(cube.z);

                float x_diff = Mathf.Abs(rx - cube.x);
                float y_diff = Mathf.Abs(ry - cube.y);
                float z_diff = Mathf.Abs(rz - cube.z);

                if (x_diff > y_diff && x_diff > z_diff) {
                    rx = -ry - rz;
                }
                else if (y_diff > z_diff) {
                    ry = -rx - rz;
                }
                else {
                    rz = -rx - ry;
                }

                return new Vector3Int(rx, ry, rz);
            }

            public static Vector3 HexToCube(Vector2 hex) {
                var x = hex.x;
                var z = hex.y;
                var y = -x - z;
                return new Vector3(x, y, z);
            }
            public static Vector2 CubeToHex(Vector3 cube) {
                return new Vector2(cube.x, cube.z);
            }
            public static Vector3Int HexToCube(Vector2Int hex) {
                var x = hex.x;
                var z = hex.y;
                var y = -x - z;
                return new Vector3Int(x, y, z);
            }
            public static Vector2Int CubeToHex(Vector3Int cube) {
                return new Vector2Int(cube.x, cube.z);
            }

            public static List<Vector2Int> HexPointList(RectInt limits) {
                return limits.PointList();
            }
            public static List<Vector3Int> CubePointList(Rect3Int limits) {
                return HexPointList(new RectInt() {
                    xMin = limits.Min.x,
                    xMax = limits.Max.x,
                    yMin = limits.Min.z,
                    yMax = limits.Max.z,
                }).ConvertAll(HexToCube).FindAll(c => c.y >= limits.Min.y && c.y < limits.Max.y);
            }

            public static List<Vector2Int> HexBoundPoints(RectInt limits) {
                return limits.BoundPoints();
            }
            public static List<Vector3Int> CubeBoundPoints(Rect3Int limits) {
                return CubePointList(limits).FindAll(
                    c => c.x == limits.Min.x || c.x == limits.Max.x
                     ||  c.y == limits.Min.y || c.y == limits.Max.y
                     ||  c.z == limits.Min.z || c.z == limits.Max.z
                );
            }

            public static Vector3Int CubeClamped(Vector3Int cube, Rect3Int limits) {
                return CubeBoundPoints(limits).MinElement(c => c.DistanceTo(cube));
            }

            public static Vector3 CubeFromXY(float x, float y) {
                return new Vector3(x, y, -x - y);
            }
            public static Vector3 CubeFromXZ(float x, float z) {
                return new Vector3(x, -x - z, z);
            }
            public static Vector3 CubeFromYZ(float y, float z) {
                return new Vector3(-y - z, y, z);
            }
            public static Vector3Int CubeFromXY(int x, int y) {
                return new Vector3Int(x, y, -x - y);
            }
            public static Vector3Int CubeFromXZ(int x, int z) {
                return new Vector3Int(x, -x - z, z);
            }
            public static Vector3Int CubeFromYZ(int y, int z) {
                return new Vector3Int(-y - z, y, z);
            }

            public static List<Vector3> CubeXLineFromY(float x, float yMin, float yMax) {
                return new List<Vector3>() { CubeFromXY(x, yMin), CubeFromXY(x, yMax) };
            }
            public static List<Vector3> CubeXLineFromZ(float x, float zMin, float zMax) {
                return new List<Vector3>() { CubeFromXZ(x, zMin), CubeFromXZ(x, zMax) };
            }
            public static List<Vector3> CubeYLineFromX(float y, float xMin, float xMax) {
                return new List<Vector3>() { CubeFromXY(xMin, y), CubeFromXY(xMax, y) };
            }
            public static List<Vector3> CubeYLineFromZ(float y, float zMin, float zMax) {
                return new List<Vector3>() { CubeFromYZ(y, zMin), CubeFromYZ(y, zMax) };
            }
            public static List<Vector3> CubeZLineFromX(float z, float xMin, float xMax) {
                return new List<Vector3>() { CubeFromXZ(xMin, z), CubeFromXZ(xMax, z) };
            }
            public static List<Vector3> CubeZLineFromY(float z, float yMin, float yMax) {
                return new List<Vector3>() { CubeFromYZ(yMin, z), CubeFromYZ(yMax, z) };
            }
            public static List<Vector3Int> CubeXLineFromY(int x, int yMin, int yMax) {
                return new List<Vector3Int>() { CubeFromXY(x, yMin), CubeFromXY(x, yMax) };
            }
            public static List<Vector3Int> CubeXLineFromZ(int x, int zMin, int zMax) {
                return new List<Vector3Int>() { CubeFromXZ(x, zMin), CubeFromXZ(x, zMax) };
            }
            public static List<Vector3Int> CubeYLineFromX(int y, int xMin, int xMax) {
                return new List<Vector3Int>() { CubeFromXY(xMin, y), CubeFromXY(xMax, y) };
            }
            public static List<Vector3Int> CubeYLineFromZ(int y, int zMin, int zMax) {
                return new List<Vector3Int>() { CubeFromYZ(y, zMin), CubeFromYZ(y, zMax) };
            }
            public static List<Vector3Int> CubeZLineFromX(int z, int xMin, int xMax) {
                return new List<Vector3Int>() { CubeFromXZ(xMin, z), CubeFromXZ(xMax, z) };
            }
            public static List<Vector3Int> CubeZLineFromY(int z, int yMin, int yMax) {
                return new List<Vector3Int>() { CubeFromYZ(yMin, z), CubeFromYZ(yMax, z) };
            }

            public static List<Vector3> CubeXLine(float x, float yMin, float yMax, float zMin, float zMax) {
                List<Vector3> fromY = CubeXLineFromY(x, yMin, yMax);
                List<Vector3> fromZ = CubeXLineFromZ(x, zMin, zMax);
                if (fromY[1].z > fromZ[1].z || fromY[0].z < fromZ[0].z) {
                    return new List<Vector3>();
                }
                return new List<Vector3> {
                    (fromY[0].z < fromZ[1].z) ? fromY[0] : fromZ[1],
                    (fromY[1].z > fromZ[0].z) ? fromY[1] : fromZ[0]
                };
            }
            public static List<Vector3> CubeYLine(float y, float xMin, float xMax, float zMin, float zMax) {
                List<Vector3> fromX = CubeYLineFromX(y, xMin, xMax);
                List<Vector3> fromZ = CubeYLineFromZ(y, zMin, zMax);
                if (fromX[1].z > fromZ[1].z || fromX[0].z < fromZ[0].z) {
                    return new List<Vector3>();
                }
                return new List<Vector3> {
                    (fromX[0].z < fromZ[1].z) ? fromX[0] : fromZ[1],
                    (fromX[1].z > fromZ[0].z) ? fromX[1] : fromZ[0]
                };
            }
            public static List<Vector3> CubeZLine(float z, float xMin, float xMax, float yMin, float yMax) {
                List<Vector3> fromX = CubeZLineFromX(z, xMin, xMax);
                List<Vector3> fromY = CubeZLineFromY(z, yMin, yMax);
                if (fromX[1].y > fromY[1].y || fromX[0].y < fromY[0].y) {
                    return new List<Vector3>();
                }
                return new List<Vector3> {
                    (fromX[0].y < fromY[1].y) ? fromX[0] : fromY[1],
                    (fromX[1].y > fromY[0].y) ? fromX[1] : fromY[0]
                };
            }
            public static List<Vector3Int> CubeXLine(int x, int yMin, int yMax, int zMin, int zMax) {
                List<Vector3Int> fromY = CubeXLineFromY(x, yMin, yMax);
                List<Vector3Int> fromZ = CubeXLineFromZ(x, zMin, zMax);
                if (fromY[1].z > fromZ[1].z || fromY[0].z < fromZ[0].z) {
                    return new List<Vector3Int>();
                }
                return new List<Vector3Int> {
                    (fromY[0].z < fromZ[1].z) ? fromY[0] : fromZ[1],
                    (fromY[1].z > fromZ[0].z) ? fromY[1] : fromZ[0]
                };
            }
            public static List<Vector3Int> CubeYLine(int y, int xMin, int xMax, int zMin, int zMax) {
                List<Vector3Int> fromX = CubeYLineFromX(y, xMin, xMax);
                List<Vector3Int> fromZ = CubeYLineFromZ(y, zMin, zMax);
                if (fromX[1].z > fromZ[1].z || fromX[0].z < fromZ[0].z) {
                    return new List<Vector3Int>();
                }
                return new List<Vector3Int> {
                    (fromX[0].z < fromZ[1].z) ? fromX[0] : fromZ[1],
                    (fromX[1].z > fromZ[0].z) ? fromX[1] : fromZ[0]
                };
            }
            public static List<Vector3Int> CubeZLine(int z, int xMin, int xMax, int yMin, int yMax) {
                List<Vector3Int> fromX = CubeZLineFromX(z, xMin, xMax);
                List<Vector3Int> fromY = CubeZLineFromY(z, yMin, yMax);
                if (fromX[1].y > fromY[1].y || fromX[0].y < fromY[0].y) {
                    return new List<Vector3Int>();
                }
                return new List<Vector3Int> {
                    (fromX[0].y < fromY[1].y) ? fromX[0] : fromY[1],
                    (fromX[1].y > fromY[0].y) ? fromX[1] : fromY[0]
                };
            }

            public static List<Vector3> CubeBoundXLines(Rect3 rect) {
                List<Vector3> result = new List<Vector3>();
                result.AddRange(CubeXLine(rect.Max.x, rect.Min.y, rect.Max.y, rect.Min.z, rect.Max.z));
                result.AddRange(CubeXLine(rect.Min.x, rect.Min.y, rect.Max.y, rect.Min.z, rect.Max.z));
                return result;
            }
            public static List<Vector3> CubeBoundYLines(Rect3 rect) {
                List<Vector3> result = new List<Vector3>();
                result.AddRange(CubeYLine(rect.Max.y, rect.Min.x, rect.Max.x, rect.Min.z, rect.Max.z));
                result.AddRange(CubeYLine(rect.Min.y, rect.Min.x, rect.Max.x, rect.Min.z, rect.Max.z));
                return result;
            }
            public static List<Vector3> CubeBoundZLines(Rect3 rect) {
                List<Vector3> result = new List<Vector3>();
                result.AddRange(CubeZLine(rect.Max.z, rect.Min.x, rect.Max.x, rect.Min.y, rect.Max.y));
                result.AddRange(CubeZLine(rect.Min.z, rect.Min.x, rect.Max.x, rect.Min.y, rect.Max.y));
                return result;
            }
            public static List<Vector3Int> CubeBoundXLines(Rect3Int rect) {
                List<Vector3Int> result = new List<Vector3Int>();
                result.AddRange(CubeXLine(rect.Max.x - 1, rect.Min.y, rect.Max.y - 1, rect.Min.z, rect.Max.z - 1));
                result.AddRange(CubeXLine(rect.Min.x, rect.Min.y, rect.Max.y - 1, rect.Min.z, rect.Max.z - 1));
                return result;
            }
            public static List<Vector3Int> CubeBoundYLines(Rect3Int rect) {
                List<Vector3Int> result = new List<Vector3Int>();
                result.AddRange(CubeYLine(rect.Max.y - 1, rect.Min.x, rect.Max.x - 1, rect.Min.z, rect.Max.z - 1));
                result.AddRange(CubeYLine(rect.Min.y, rect.Min.x, rect.Max.x - 1, rect.Min.z, rect.Max.z - 1));
                return result;
            }
            public static List<Vector3Int> CubeBoundZLines(Rect3Int rect) {
                List<Vector3Int> result = new List<Vector3Int>();
                result.AddRange(CubeZLine(rect.Max.z - 1, rect.Min.x, rect.Max.x - 1, rect.Min.y, rect.Max.y - 1));
                result.AddRange(CubeZLine(rect.Min.z, rect.Min.x, rect.Max.x - 1, rect.Min.y, rect.Max.y - 1));
                return result;
            }

            public static List<Vector2Int> HexDirections() {
                return new List<Vector2Int>() {
                    new Vector2Int( 1, -1),
                    new Vector2Int( 1,  0),
                    new Vector2Int( 0,  1),
                    new Vector2Int(-1,  1),
                    new Vector2Int(-1,  0),
                    new Vector2Int( 0, -1)
                };
            }

            public static float HexDist(Vector2 h1, Vector2 h2) {
                return CubeDist(HexToCube(h1), HexToCube(h2));
            }
            public static float CubeDist(Vector3 a, Vector3 b) {
                return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
            }
            public static int HexDist(Vector2Int h1, Vector2Int h2) {
                return CubeDist(HexToCube(h1), HexToCube(h2));
            }
            public static int CubeDist(Vector3Int a, Vector3Int b) {
                return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
            }

            public static List<Vector2Int> RasterizeLine(Vector2Int start, Vector2Int end) {
                int n = HexDist(start, end);
                return CollectionUtils.CreateListByIndex(n + 1, i => HexRound(Vector2.Lerp(start, end, (float)i / n)));
            }
            public static List<Vector2Int> RasterizeLine(Vector2 start, Vector2 end) {
                throw new NotImplementedException();
            }
            #endregion

        }

        #endregion
        #endregion

        #endregion

    }
}
