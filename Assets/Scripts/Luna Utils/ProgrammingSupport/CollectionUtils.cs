using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    
public static partial class CollectionUtils {

    public static List<int> NaturalsList(int quantity, int start = 0) {
        List<int> result = new List<int>();
        for (int i = 0; i < quantity; i++) {
            result.Add(i + start);
        }
        return result;
    }
    public static int[] NaturalsArray(int quantity, int start = 0) {
        int[] result = new int[quantity];
        for (int i = 0; i < quantity; i++) {
            result[i] = i + start;
        }
        return result;
    }

    public static Vector2Int[,] Naturals2(int quantity, int start = 0) {
        Vector2Int[,] result = new Vector2Int[quantity, quantity];
        for (int i = 0; i < quantity; i++) {
            for (int j = 0; j < quantity; j++) {
                result[i, j] = new Vector2Int(i + start, j + start);
            }
        }
        return result;
    }
    public static Vector2Int[,] Naturals2(Vector2Int quantity, int start = 0) {
        Vector2Int[,] result = new Vector2Int[quantity.x, quantity.y];
        for (int i = 0; i < quantity.x; i++) {
            for (int j = 0; j < quantity.y; j++) {
                result[i, j] = new Vector2Int(i + start, j + start);
            }
        }
        return result;
    }
    public static Vector2Int[,] Naturals2(int quantity, Vector2Int start) {
        Vector2Int[,] result = new Vector2Int[quantity, quantity];
        for (int i = 0; i < quantity; i++) {
            for (int j = 0; j < quantity; j++) {
                result[i, j] = new Vector2Int(i, j) + start;
            }
        }
        return result;
    }
    public static Vector2Int[,] Naturals2(Vector2Int quantity, Vector2Int start) {
        Vector2Int[,] result = new Vector2Int[quantity.x, quantity.y];
        for (int i = 0; i < quantity.x; i++) {
            for (int j = 0; j < quantity.y; j++) {
                result[i, j] = new Vector2Int(i, j) + start;
            }
        }
        return result;
    }

    public static Vector3Int[,,] Naturals3(int quantity, int start = 0) {
        Vector3Int[,,] result = new Vector3Int[quantity, quantity, quantity];
        for (int i = 0; i < quantity; i++) {
            for (int j = 0; j < quantity; j++) {
                for (int k = 0; k < quantity; k++) {
                    result[i, j, k] = new Vector3Int(i + start, j + start, k + start);
                }
            }
        }
        return result;
    }
    public static Vector3Int[,,] Naturals3(Vector3Int quantity, int start = 0) {
        Vector3Int[,,] result = new Vector3Int[quantity.x, quantity.y, quantity.z];
        for (int i = 0; i < quantity.x; i++) {
            for (int j = 0; j < quantity.y; j++) {
                for (int k = 0; k < quantity.z; k++) {
                    result[i, j, k] = new Vector3Int(i + start, j + start, k + start);
                }
            }
        }
        return result;
    }
    public static Vector3Int[,,] Naturals3(int quantity, Vector3Int start) {
        Vector3Int[,,] result = new Vector3Int[quantity, quantity, quantity];
        for (int i = 0; i < quantity; i++) {
            for (int j = 0; j < quantity; j++) {
                for (int k = 0; k < quantity; k++) {
                    result[i, j, k] = new Vector3Int(i, j, k) + start;
                }
            }
        }
        return result;
    }
    public static Vector3Int[,,] Naturals3(Vector3Int quantity, Vector3Int start) {
        Vector3Int[,,] result = new Vector3Int[quantity.x, quantity.y, quantity.z];
        for (int i = 0; i < quantity.x; i++) {
            for (int j = 0; j < quantity.y; j++) {
                for (int k = 0; k < quantity.z; k++) {
                    result[i, j, k] = new Vector3Int(i, j, k) + start;
                }
            }
        }
        return result;
    }

    public static T[] CreateArray<T>(this T value, int length) {
        T[] result = new T[length];
        for (int i = 0; i < length; i++) {
            result[i] = value;
        }
        return result;
    }
    public static List<T> CreateList<T>(this T value, int length) {
        List<T> result = new List<T>();
        for (int i = 0; i < length; i++) {
            result.Add(value);
        }
        return result;
    }

    public static T[] CreateArrayByIndex<T>(int length, Func<int, T> elementByIndex) {
        T[] result = new T[length];
        for (int i = 0; i < length; i++) {
            result[i] = elementByIndex.Invoke(i);
        }
        return result;
    }
    public static List<T> CreateListByIndex<T>(int length, Func<int, T> elementByIndex) {
        List<T> result = new List<T>();
        for (int i = 0; i < length; i++) {
            result.Add(elementByIndex.Invoke(i));
        }
        return result;
    }

    public static List<List<T>> NewNestedList<T>(int length) {
        List<List<T>> result = new List<List<T>>();
        for (int i = 0; i < length; i++) {
            result.Add(new List<T>());
        }
        return result;
    }

    public static float SumPairs<T>(Func<T, T, float> converter, bool ciclic = false, bool jumpUsedElement = false, params T[] values) {
        return values.SumPairs(converter, ciclic, jumpUsedElement);
    }

}

