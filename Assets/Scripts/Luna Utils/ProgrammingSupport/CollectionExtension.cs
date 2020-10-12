using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Container of extension methods for generic Lists, ILists, ICollections and IEnumerables
/// </summary>
public static partial class CollectionExtension {

    #region Basis
    #region First/Last
    #region Get
    public static T First<T>(this IEnumerable<T> list) {
        foreach (T e in list) {
            return e;
        }
        return default(T);
    }
    public static T First<T>(this IList<T> list) {
        return list[0];
    }

    public static T Last<T>(this IEnumerable<T> list) {
        T result = default(T);
        foreach (T e in list) {
            result = e;
        }
        return result;
    }
    public static T Last<T>(this IList<T> list) {
        return list[list.Count - 1];
    }

    public static List<T> LastRange<T>(this IList<T> list, int count) {
        return list.StartingAt(list.Count - count);
    }

    public static int LastIndex<T>(this IList<T> list) {
        return list.Count - 1;
    }
    #endregion

    #region Set
    public static void First<T>(this IList<T> list, T value) {
        list[0] = value;
    }
    public static void Last<T>(this IList<T> list, T value) {
        list[list.Count - 1] = value;
    }
    #endregion

    #region Remove
    public static void RemoveFirst<T>(this IList<T> list) {
        list.RemoveAt(0);
    }
    public static void RemoveLast<T>(this IList<T> list) {
        list.RemoveAt(list.Count - 1);
    }
    public static List<T> WithoutFirst<T>(this IList<T> list) {
        List<T> result = new List<T>(list);
        result.RemoveFirst();
        return result;
    }
    public static List<T> WithoutLast<T>(this IList<T> list) {
        List<T> result = new List<T>(list);
        result.RemoveLast();
        return result;
    }
    #endregion
    #endregion

    #region Valid Index
    public static bool IsValidIndex<T>(this ICollection<T> list, int index) {
        return (index >= 0) && (index < list.Count);
    }
    public static bool EnsureValidIndex<T>(this ICollection<T> list, int index, T defaultValue = default(T)) {
        if (list.Count <= index) {
            for (int i = list.Count; i <= index; i++) {
                list.Add(defaultValue);
            }
        }
        return list.IsValidIndex(index);
    }
    #endregion

    #region Empty
    public static bool Empty<T>(this ICollection<T> list) {
        return list.Count == 0;
    }
    #endregion
    #endregion

    #region Random
    #region Single Element
    /// <summary>
    /// Gets a random element from <paramref name="list"/> 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <returns>A random element from <paramref name="list"/></returns>
    public static T RandomElement<T>(this IList<T> list) {
        return list[list.RandomIndex()];
    }

    /// <summary>
    /// Gets a random element from <paramref name="list"/> with chances determined by the integer <paramref name="weights"/> of each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="weights">The integer IList that determines the chance of each element being picked up</param>
    /// <returns>A random element from <paramref name="list"/></returns>
    public static T RandomElement<T>(this IList<T> list, IList<int> weights) {
        return list[list.RandomIndex(weights)];
    }

    /// <summary>
    /// Gets a random element from <paramref name="list"/> with chances determined by the floating point <paramref name="weights"/> of each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="weights">The floating point IList that determines the chance of each element being picked up</param>
    /// <returns>A random element from <paramref name="list"/></returns>
    public static T RandomElement<T>(this IList<T> list, IList<float> weights) {
        return list[list.RandomIndex(weights)];
    }

    /// <summary>
    /// Gets a random element from <paramref name="list"/> with chances determined by the integer returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="weightFunc">The delegate function that calculates an integer weight for an element to be chosen</param>
    /// <returns>A random element from <paramref name="list"/></returns>
    public static T RandomElement<T>(this IList<T> list, Func<T, int> weightFunc) {
        return list[list.RandomIndex(weightFunc)];
    }

    /// <summary>
    /// Gets a random element from <paramref name="list"/> with chances determined by the floating point returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="weightFunc">The delegate function that calculates a floating point weight for an element to be chosen</param>
    /// <returns>A random element from <paramref name="list"/></returns>
    public static T RandomElement<T>(this IList<T> list, Func<T, float> weightFunc) {
        return list[list.RandomIndex(weightFunc)];
    }
    #endregion

    #region Single Index
    /// <summary>
    /// Gets the integer index of a random element from <paramref name="list"/> 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The ICollection from which the index is taken</param>
    /// <returns>A random index of an element from <paramref name="list"/></returns>
    public static int RandomIndex<T>(this ICollection<T> list) {
        return UnityEngine.Random.Range(0, list.Count);
    }

    /// <summary>
    /// Gets the integer index of a random element from <paramref name="list"/> with chances determined by the integer <paramref name="weights"/> of each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable from which the element is taken</param>
    /// <param name="weights">The integer IList that determines the chance of each index being picked up</param>
    /// <returns>A random index of an element from <paramref name="list"/></returns>
    public static int RandomIndex<T>(this IEnumerable<T> list, IList<int> weights) {
        int sum = weights.Sum();
        int rand = UnityEngine.Random.Range(0, sum);
        int i = 0;
        int count = 0;
        while (rand >= count + weights[i]) {
            count += weights[i];
            i++;
        }
        return i;
    }

    /// <summary>
    /// Gets the integer index of a random element from <paramref name="list"/> with chances determined by the floating point <paramref name="weights"/> of each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable from which the element is taken</param>
    /// <param name="weights">The floating point IList that determines the chance of each index being picked up</param>
    /// <returns>A random index of an element from <paramref name="list"/></returns>
    public static int RandomIndex<T>(this IEnumerable<T> list, IList<float> weights) {
        float sum = weights.Sum();
        float rand = UnityEngine.Random.Range(0f, sum);
        int i = 0;
        float count = 0;
        while (rand >= count + weights[i]) {
            count += weights[i];
            i++;
        }
        return i;
    }

    /// <summary>
    /// Gets the integer index of a random element from <paramref name="list"/> with chances determined by the integer returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable from which the element is taken</param>
    /// <param name="weightFunc">The delegate function that calculates an integer weight for any element's index to be chosen</param>
    /// <returns>A random index of an element from <paramref name="list"/></returns>
    public static int RandomIndex<T>(this IEnumerable<T> list, Func<T, int> weightFunc) {
        List<int> weights = new List<int>();
        foreach (T element in list) {
            weights.Add(weightFunc.Invoke(element));
        }
        return list.RandomIndex(weights);
    }

    /// <summary>
    /// Gets the integer index of a random element from <paramref name="list"/> with chances determined by the floating point returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable from which the element is taken</param>
    /// <param name="weightFunc">The delegate function that calculates an floating point weight for any element's index to be chosen</param>
    /// <returns>A random index of an element from <paramref name="list"/></returns>
    public static int RandomIndex<T>(this IEnumerable<T> list, Func<T, float> weightFunc) {
        List<float> weights = new List<float>();
        foreach (T element in list) {
            weights.Add(weightFunc.Invoke(element));
        }
        return list.RandomIndex(weights);
    }
    #endregion

    #region Multiple Elements
    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, bool canRepeat = false) {
        return list.SortedBy(list.RandomIndices(quantity, canRepeat));
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the integer <paramref name="weights"/> of each element 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="weights">The integer IList that determines the chance of each element being picked up</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, IList<int> weights, bool canRepeat = false) {
        return list.SortedBy(list.RandomIndices(quantity, weights, canRepeat));
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the floating point <paramref name="weights"/> of each element 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="weights">The floating point IList that determines the chance of each element being picked up</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, IList<float> weights, bool canRepeat = false) {
        return list.SortedBy(list.RandomIndices(quantity, weights, canRepeat));
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the integer returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="weightFunc">The delegate function that calculates an integer weight for an element to be chosen</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, int> weightFunc, bool canRepeat = false) {
        return list.SortedBy(list.RandomIndices(quantity, weightFunc, canRepeat));
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the floating point returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="weightFunc">The delegate function that calculates a floating point weight for an element to be chosen</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, float> weightFunc, bool canRepeat = false) {
        return list.SortedBy(list.RandomIndices(quantity, weightFunc, canRepeat));
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the integer returned from <paramref name="dynamicWeightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="dynamicWeightFunc">The delegate function that calculates an integer weight for an element to be chosen. This function can return a different weight depending on the list of currently chosen elements</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, List<T>, int> dynamicWeightFunc, bool canRepeat = false) {
        List<T> result = new List<T>();
        List<int> possibilities = CollectionUtils.NaturalsList(list.Count);
        while (result.Count < quantity) {
            int chosenIndex = possibilities.RandomIndex(i => dynamicWeightFunc.Invoke(list[i], result));
            int chosen = possibilities[chosenIndex];
            if (!canRepeat) possibilities.RemoveAt(chosenIndex);
            result.Add(list[chosen]);
        }
        return result;
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the floating points returned from <paramref name="dynamicWeightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="dynamicWeightFunc">The delegate function that calculates an floating points weight for an element to be chosen. This function can return a different weight depending on the list of currently chosen elements</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<T> RandomElements<T>(this IList<T> list, int quantity, Func<T, List<T>, float> dynamicWeightFunc, bool canRepeat = false) {
        List<T> result = new List<T>();
        List<int> possibilities = CollectionUtils.NaturalsList(list.Count);
        while (result.Count < quantity) {
            int chosenIndex = possibilities.RandomIndex(i => dynamicWeightFunc.Invoke(list[i], result));
            int chosen = possibilities[chosenIndex];
            if (!canRepeat) possibilities.RemoveAt(chosenIndex);
            result.Add(list[chosen]);
        }
        return result;
    }
    #endregion

    #region Multiple Indices
    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random indices from <paramref name="list"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the indices are taken</param>
    /// <param name="quantity">The number of random indices to return</param>
    /// <param name="canRepeat">Defines whether or not a single index from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random indices from <paramref name="list"/></returns>
    public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, bool canRepeat = false) {
        List<int> result = new List<int>();
        List<int> possibilities = CollectionUtils.NaturalsList(list.Count);

        while (result.Count < quantity) {
            int chosenIndex = possibilities.RandomIndex();
            int chosen = possibilities[chosenIndex];
            if (!canRepeat) possibilities.RemoveAt(chosenIndex);
            result.Add(chosen);
        }

        return result;
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random indices from <paramref name="list"/> with chances determined by the integer <paramref name="weights"/> of each index 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the indices are taken</param>
    /// <param name="quantity">The number of random indices to return</param>
    /// <param name="weights">The integer IList that determines the chance of each index being picked up</param>
    /// <param name="canRepeat">Defines whether or not a single index from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random indices from <paramref name="list"/></returns>
    public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, IList<int> weights, bool canRepeat = false) {
        List<int> result = new List<int>();
        List<int> possibilities = CollectionUtils.NaturalsList(list.Count);
        while (result.Count < quantity) {
            int chosenIndex = possibilities.RandomIndex(weights);
            int chosen = possibilities[chosenIndex];
            if (!canRepeat) possibilities.RemoveAt(chosenIndex);
            result.Add(chosen);
        }
        return result;
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random elements from <paramref name="list"/> with chances determined by the floating point <paramref name="weights"/> of each index 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the elements are taken</param>
    /// <param name="quantity">The number of random elements to return</param>
    /// <param name="weights">The floating point IList that determines the chance of each element being picked up</param>
    /// <param name="canRepeat">Defines whether or not a single element from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random elements from <paramref name="list"/></returns>
    public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, IList<float> weights, bool canRepeat = false) {
        List<int> result = new List<int>();
        List<int> possibilities = CollectionUtils.NaturalsList(list.Count);

        while (result.Count < quantity) {
            int chosenIndex = possibilities.RandomIndex(weights);
            int chosen = possibilities[chosenIndex];
            if (!canRepeat) possibilities.RemoveAt(chosenIndex);
            result.Add(chosen);
        }

        return result;
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random indices from <paramref name="list"/> with chances determined by the integer returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the indices are taken</param>
    /// <param name="quantity">The number of random indices to return</param>
    /// <param name="weightFunc">The delegate function that calculates an integer weight for an index to be chosen</param>
    /// <param name="canRepeat">Defines whether or not a single index from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random indices from <paramref name="list"/></returns>
    public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, Func<T, int> weightFunc, bool canRepeat = false) {
        List<int> weights = new List<int>();
        foreach (T element in list) {
            weights.Add(weightFunc.Invoke(element));
        }
        return list.RandomIndices(quantity, weights, canRepeat);
    }

    /// <summary>
    /// Gets a list filled with <paramref name="quantity"/> random indices from <paramref name="list"/> with chances determined by the floating point returned from <paramref name="weightFunc"/> for each element
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the indices are taken</param>
    /// <param name="quantity">The number of random indices to return</param>
    /// <param name="weightFunc">The delegate function that calculates an floating point weight for an index to be chosen</param>
    /// <param name="canRepeat">Defines whether or not a single index from <paramref name="list"/> can be chosen more than once</param>
    /// <returns>A list of random indices from <paramref name="list"/></returns>
    public static List<int> RandomIndices<T>(this ICollection<T> list, int quantity, Func<T, float> weightFunc, bool canRepeat = false) {
        List<float> weights = new List<float>();
        foreach (T element in list) {
            weights.Add(weightFunc.Invoke(element));
        }
        return list.RandomIndices(quantity, weights, canRepeat);
    }
    #endregion
    #endregion

    #region Math
    #region Min/Max
    #region Value
    #region Specific
    /// <summary>
    /// Returns the smallest of all the floating point values in <paramref name="list"/>
    /// </summary>
    /// <param name="list">The IEnumerable containing all the of floating point values</param>
    /// <returns>The smallest of all the values in <paramref name="list"/></returns>
    public static float Min(this IEnumerable<float> list) {
        return Mathf.Min(list.ToArray());
    }

    /// <summary>
    /// Returns the largest of all the floating point values in <paramref name="list"/>
    /// </summary>
    /// <param name="list">The IEnumerable containing all the of floating point values</param>
    /// <returns>The largest of all the values in <paramref name="list"/></returns>
    public static float Max(this IEnumerable<float> list) {
        return Mathf.Max(list.ToArray());
    }

    /// <summary>
    /// Returns the smallest of all the values in <paramref name="list"/>
    /// </summary>
    /// <param name="list">The IEnumerable containing all the of integer values</param>
    /// <returns>The smallest of all the values in <paramref name="list"/></returns>
    public static int Min(this IEnumerable<int> list) {
        return list.Reduce<int, int>((a, b) => Mathf.Min(a, b), int.MaxValue);
    }

    /// <summary>
    /// Returns the largest of all the integer values in <paramref name="list"/>
    /// </summary>
    /// <param name="list">The IEnumerable containing all the of integer values</param>
    /// <returns>The largest of all the values in <paramref name="list"/></returns>
    public static int Max(this IEnumerable<int> list) {
        return list.Reduce<int, int>((a, b) => Mathf.Max(a, b), int.MinValue);
    }
    #endregion

    #region Generic
    /// <summary>
    /// Returns the smallest of all the floating point values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the generic type elements</param>
    /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
    /// <returns>The smallest of all the values found</returns>
    public static float Min<T>(this IEnumerable<T> list, Func<T, float> func) {
        return list.Reduce((a, b) => Mathf.Min(func.Invoke(a), b), float.MaxValue);
    }

    /// <summary>
    /// Returns the largest of all the floating point values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the generic type elements</param>
    /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
    /// <returns>The largest of all the values found</returns>
    public static float Max<T>(this IEnumerable<T> list, Func<T, float> func) {
        return list.Reduce((a, b) => Mathf.Max(func.Invoke(a), b), float.MinValue);
    }

    /// <summary>
    /// Returns the smallest of all the integer values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the generic type elements</param>
    /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
    /// <returns>The smallest of all the values found</returns>
    public static int Min<T>(this IEnumerable<T> list, Func<T, int> func) {
        return list.Reduce((a, b) => Mathf.Min(func.Invoke(a), b), int.MaxValue);
    }

    /// <summary>
    /// Returns the largest of all the integer values returned by <paramref name="func"/> applied on the elements of <paramref name="list"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the generic type elements</param>
    /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
    /// <returns>The largest of all the values found</returns>
    public static int Max<T>(this IEnumerable<T> list, Func<T, int> func) {
        return list.Reduce((a, b) => Mathf.Max(func.Invoke(a), b), int.MinValue);
    }
    #endregion
    #endregion

    #region Element
    /// <summary>
    /// Returns the <paramref name="list"/> element that results on the smallest floating point value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
    /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
    /// <returns>The element that results on the smallest value returned by <paramref name="func"/></returns>
    public static T MinElement<T>(this IEnumerable<T> list, Func<T, float> func) {
        T result = default;
        float min = float.MaxValue;
        foreach (T item in list) {
            float cur = func.Invoke(item);
            if (min > cur) {
                min = cur;
                result = item;
            }
        }
        return result;
    }

    /// <summary>
    /// Returns the <paramref name="list"/> element that results on the largest floating point value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
    /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
    /// <returns>The element that results on the largest value returned by <paramref name="func"/></returns>
    public static T MaxElement<T>(this IEnumerable<T> list, Func<T, float> func) {
        T result = default;
        float max = float.MinValue;
        foreach (T item in list) {
            float cur = func.Invoke(item);
            if (max < cur) {
                max = cur;
                result = item;
            }
        }
        return result;
    }

    /// <summary>
    /// Returns the <paramref name="list"/> element that results on the smallest integer value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
    /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
    /// <returns>The element that results on the smallest value returned by <paramref name="func"/></returns>
    public static T MinElement<T>(this IEnumerable<T> list, Func<T, int> func) {
        T result = default;
        int min = int.MaxValue;
        foreach (T item in list) {
            int cur = func.Invoke(item);
            if (min > cur) {
                min = cur;
                result = item;
            }
        }
        return result;
    }

    /// <summary>
    /// Returns the <paramref name="list"/> element that results on the largest integer value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containing all the elements to be evauated</param>
    /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
    /// <returns>The element that results on the largest value returned by <paramref name="func"/></returns>
    public static T MaxElement<T>(this IEnumerable<T> list, Func<T, int> func) {
        T result = default;
        int max = int.MinValue;
        foreach (T item in list) {
            int cur = func.Invoke(item);
            if (max < cur) {
                max = cur;
                result = item;
            }
        }
        return result;
    }
    #endregion

    #region Value, Element and Index
    /// <summary>
    /// Represents a comparable element of a list. Stores the element, it's index in the list and it's comparable value
    /// </summary>
    /// <typeparam name="E">The type of the elements of the list</typeparam>
    /// <typeparam name="V">The type of the values that the elements represent</typeparam>
    public struct ListElement<E, V> where V : IComparable {
        public E element;
        public int index;
        public V value;
    }

    /// <summary>
    /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest floating point value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
    /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest floating point value returned by <paramref name="func"/></returns>
    public static ListElement<T, float> FindMinimum<T>(this IList<T> list, Func<T, float> func) {
        ListElement<T, float> e = new ListElement<T, float>();
        e.value = float.MaxValue;
        e.index = -1;
        e.element = default(T);
        for (int i = 0; i < list.Count; i++) {
            float curValue = func(list[i]);
            if (e.value > curValue) {
                e.value = curValue;
                e.element = list[i];
                e.index = i;
            }
        }
        return e;
    }

    /// <summary>
    /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest floating point value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="func">The delegate function that calculates the floating point value for an element of <paramref name="list"/></param>
    /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest floating point value returned by <paramref name="func"/></returns>
    public static ListElement<T, float> FindMaximum<T>(this IList<T> list, Func<T, float> func) {
        ListElement<T, float> e = new ListElement<T, float>();
        e.value = float.MinValue;
        e.index = -1;
        e.element = default(T);
        for (int i = 0; i < list.Count; i++) {
            float curValue = func(list[i]);
            if (e.value < curValue) {
                e.value = curValue;
                e.element = list[i];
                e.index = i;
            }
        }
        return e;
    }

    /// <summary>
    /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest integer value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
    /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the smallest value returned by <paramref name="func"/></returns>
    public static ListElement<T, int> FindMinimum<T>(this IList<T> list, Func<T, int> func) {
        ListElement<T, int> e = new ListElement<T, int>();
        e.value = int.MaxValue;
        e.index = -1;
        e.element = default(T);
        for (int i = 0; i < list.Count; i++) {
            int curValue = func(list[i]);
            if (e.value > curValue) {
                e.value = curValue;
                e.element = list[i];
                e.index = i;
            }
        }
        return e;
    }

    /// <summary>
    /// Returns a <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest integer value returned by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/></typeparam>
    /// <param name="list">The IList from which the element is taken</param>
    /// <param name="func">The delegate function that calculates the integer value for an element of <paramref name="list"/></param>
    /// <returns>A <c>ListElement</c> based on the <paramref name="list"/> element that results on the largest value returned by <paramref name="func"/></returns>
    public static ListElement<T, int> FindMaximum<T>(this IList<T> list, Func<T, int> func) {
        ListElement<T, int> e = new ListElement<T, int>();
        e.value = int.MinValue;
        e.index = -1;
        e.element = default(T);
        for (int i = 0; i < list.Count; i++) {
            int curValue = func(list[i]);
            if (e.value < curValue) {
                e.value = curValue;
                e.element = list[i];
                e.index = i;
            }
        }
        return e;
    }
    #endregion
    #endregion

    #region Sum
    /// <summary>
    /// Returns the sum of all elements in <paramref name="list"/> after being converted to integer by <paramref name="converter"/>
    /// </summary>
    /// <typeparam name="T">Type of the elements in/ <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containig the elements to be summed</param>
    /// <param name="converter">The delegate function that converts a <typeparamref name="T"/> element into an integer</param>
    /// <returns>The sum of all elements in <paramref name="list"/> after being converted to integer</returns>
    public static int Sum<T>(this IEnumerable<T> list, Func<T, int> converter) {
        return list.ConvertAll(converter).Sum();
    }

    /// <summary>
    /// Returns the sum of all elements in <paramref name="list"/> after being converted to floating point by <paramref name="converter"/>
    /// </summary>
    /// <typeparam name="T">Type of the elements in/ <paramref name="list"/></typeparam>
    /// <param name="list">The IEnumerable containig the elements to be summed</param>
    /// <param name="converter">The delegate function that converts a <typeparamref name="T"/> element into a floating point</param>
    /// <returns>The sum of all elements in <paramref name="list"/> after being converted to floating point</returns>
    public static float Sum<T>(this IEnumerable<T> list, Func<T, float> converter) {
        return list.ConvertAll(converter).Sum();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="E"></typeparam>
    /// <param name="list"></param>
    /// <param name="addFunc"></param>
    /// <param name="ciclic"></param>
    /// <param name="jumpUsedElement"></param>
    /// <returns></returns>
    public static V SumPairs<V, E>(this IList<E> list, Func<V, E, E, V> addFunc, bool ciclic = false, bool jumpUsedElement = false) {
        V result = default(V);
        list.ForEachConsecutivePair((f, s) => result = addFunc(result, f, s), ciclic, jumpUsedElement);
        return result;
    }

    public static int SumPairs<T>(this IList<T> list, Func<T, T, int> converter, bool ciclic = false, bool jumpUsedElement = false) {
        int result = 0;
        list.ForEachConsecutivePair((e1, e2) => result += converter.Invoke(e1, e2), ciclic, jumpUsedElement);
        return result;
    }

    public static float SumPairs<T>(this IList<T> list, Func<T, T, float> converter, bool ciclic = false, bool jumpUsedElement = false) {
        float result = 0f;
        list.ForEachConsecutivePair((e1, e2) => result += converter.Invoke(e1, e2), ciclic, jumpUsedElement);
        return result;
    }

    public static float SumTrios<T>(this IList<T> list, Func<T, T, T, float> converter, bool ciclic = false, bool jumpUsedElement = false) {
        float result = 0f;
        list.ForEachConsecutiveTrio((e1, e2, e3) => result += converter.Invoke(e1, e2, e3), ciclic, jumpUsedElement);
        return result;
    }
    #endregion

    #region Means
    public static float Mean(this ICollection<int> list) {
        return list.Mean<float, int>((v1, v2) => v1 + v2, (v, c) => v / c);
    }
    public static float Mean(this ICollection<float> list) {
        return list.Mean<float, float>((v1, v2) => v1 + v2, (v, c) => v / c);
    }
    public static Vector2 Mean(this ICollection<Vector2> list) {
        return list.Mean<Vector2, Vector2>((v1, v2) => v1 + v2, (v, c) => v / c);
    }
    public static Vector3 Mean(this ICollection<Vector3> list) {
        return list.Mean<Vector3, Vector3>((v1, v2) => v1 + v2, (v, c) => v / c);
    }
    public static Vector2 Mean(this ICollection<Vector2Int> list) {
        return list.Mean<Vector2, Vector2Int>((v1, v2) => v1 + v2, (v, c) => v / c);
    }
    public static Vector3 Mean(this ICollection<Vector3Int> list) {
        return list.Mean<Vector3, Vector3Int>((v1, v2) => v1 + v2, (v, c) => v / c);
    }
    public static float Mean<T>(this ICollection<T> list, Func<T, float> converter) {
        return list.ConvertAll(converter).Mean();
    }
    public static V Mean<V, E>(this ICollection<E> list, Func<E, V, V> addFunc, Func<V, int, V> divFunc) {
        return divFunc(list.Reduce(addFunc), list.Count);
    }
    public static V Median<V, E>(this IEnumerable<E> list, Comparison<E> comparison, Func<E, E, V> meanFunc) {
        List<E> temp = new List<E>(list);
        temp.Sort(comparison);
        int n = temp.Count;
        if (n % 2 == 0) {
            return meanFunc.Invoke(temp[(n / 2) - 1], temp[n / 2]);
        }
        else {
            return meanFunc.Invoke(temp[n / 2], temp[n / 2]);
        }
    }
    public static V Median<V, E>(this IEnumerable<E> list, IComparer<E> comparer, Func<E, E, V> meanFunc) {
        List<E> temp = new List<E>(list);
        temp.Sort(comparer);
        int n = temp.Count;
        if (n % 2 == 0) {
            return meanFunc.Invoke(temp[(n / 2) - 1], temp[n / 2]);
        }
        else {
            return meanFunc.Invoke(temp[n / 2], temp[n / 2]);
        }
    }
    public static E Mode<E>(this IEnumerable<E> list) {
        return list.Mode((e1, e2) => e1.Equals(e2));
    }
    public static E Mode<E>(this IEnumerable<E> list, Func<E, E, bool> equalsFunc) {
        return list.CountOccurrences().MaxElement(o => o.Value).Key;
    }
    #endregion

    #region Set
    public static List<T> IntersectedWith<T>(this IEnumerable<T> list, ICollection<T> other) {
        List<T> result = new List<T>();
        foreach (T e in list) {
            if (other.Contains(e)) {
                result.Add(e);
            }
        }
        return result;
    }
    public static List<T> UnitedWith<T>(this IEnumerable<T> list, IEnumerable<T> other) {
        if (list == null) {
            return new List<T>(other);
        }
        if (other == null) {
            return new List<T>(list);
        }
        List<T> result = new List<T>(list);
        result.AddRange(other);
        return result;
    }
    #endregion
    #endregion

    #region Iterations
    #region Action
    public static void ForEach<T>(this IEnumerable<T> list, Action action) {
        foreach (T e in list) {
            action.Invoke();
        }
    }
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) {
        foreach (T e in list) {
            action.Invoke(e);
        }
    }
    public static void ForEach<T>(this IList<T> list, Action<T, int> action) {
        for (int i = 0; i < list.Count; i++) {
            action.Invoke(list[i], i);
        }
    }

    public static void ForEachBreak<T>(this IEnumerable<T> list, Action action, Predicate<T> breakCondition, bool breakBeforeAction = false) {
        foreach (T e in list) {
            if (breakBeforeAction && breakCondition.Invoke(e)) {
                break;
            }
            action.Invoke();
            if (!breakBeforeAction && breakCondition.Invoke(e)) {
                break;
            }
        }
    }
    public static void ForEachBreak<T>(this IEnumerable<T> list, Action<T> action, Predicate<T> breakCondition, bool breakBeforeAction = false) {
        foreach (T e in list) {
            if (breakBeforeAction && breakCondition.Invoke(e)) {
                break;
            }
            action.Invoke(e);
            if (!breakBeforeAction && breakCondition.Invoke(e)) {
                break;
            }
        }
    }
    public static void ForEachBreak<T>(this IList<T> list, Action<T, int> action, Predicate<T> breakCondition, bool breakBeforeAction = false) {
        for (int i = 0; i < list.Count; i++) {
            if (breakBeforeAction && breakCondition.Invoke(list[i])) {
                break;
            }
            action.Invoke(list[i], i);
            if (!breakBeforeAction && breakCondition.Invoke(list[i])) {
                break;
            }
        }
    }

    public static void ForEachConditional<T>(this IEnumerable<T> list, Action action, Predicate<T> applyCondition) {
        foreach (T e in list) {
            if (applyCondition.Invoke(e))
                action.Invoke();
        }
    }
    public static void ForEachConditional<T>(this IEnumerable<T> list, Action<T> action, Predicate<T> applyCondition) {
        foreach (T e in list) {
            if (applyCondition.Invoke(e))
                action.Invoke(e);
        }
    }
    public static void ForEachConditional<T>(this IList<T> list, Action<T, int> action, Predicate<T> applyCondition) {
        for (int i = 0; i < list.Count; i++) {
            if (applyCondition.Invoke(list[i]))
                action.Invoke(list[i], i);
        }
    }

    public static void ForEachConsecutivePair<T>(this IList<T> list, Action<T, T> action, bool ciclic = false, bool jumpUsedElement = false) {
        list.ForEachConsecutiveN(2, l => action.Invoke(l[0], l[1]), ciclic, jumpUsedElement);
    }
    public static void ForEachConsecutiveTrio<T>(this IList<T> list, Action<T, T, T> action, bool ciclic = false, bool jumpUsedElements = false) {
        list.ForEachConsecutiveN(3, l => action.Invoke(l[0], l[1], l[2]), ciclic, jumpUsedElements);
    }
    public static void ForEachConsecutiveN<T>(this IList<T> list, int N, Action<IList<T>> action, bool ciclic = false, bool jumpUsedElements = false) {
        int n = list.Count;
        if (n < N) {
            return;
        }
        int increment = (jumpUsedElements) ? N : 1;
        int size = (ciclic) ? n : n - 1;
        for (int i = 0; i < size; i += increment) {
            action.Invoke(list.GetRange(i, N, ciclic));
        }
    }
    #endregion

    #region Func
    public static void SetEach<T>(this IList<T> list, Func<T> func) {
        for (int i = 0; i < list.Count; i++) {
            list[i] = func.Invoke();
        }
    }
    public static void SetEachElement<T>(this IList<T> list, Func<T, T> func) {
        for (int i = 0; i < list.Count; i++) {
            list[i] = func.Invoke(list[i]);
        }
    }
    public static void SetEachIndex<T>(this IList<T> list, Func<int, T> func) {
        for (int i = 0; i < list.Count; i++) {
            list[i] = func.Invoke(i);
        }
    }
    public static void SetEachElementIndex<T>(this IList<T> list, Func<T, int, T> func) {
        for (int i = 0; i < list.Count; i++) {
            list[i] = func.Invoke(list[i], i);
        }
    }

    public static List<T> ThruFunc<T>(this List<T> list, Func<T> func) {
        List<T> result = new List<T>(list);
        result.SetEach(func);
        return result;
    }
    public static List<T> ThruFuncElement<T>(this List<T> list, Func<T, T> eFunc) {
        List<T> result = new List<T>(list);
        result.SetEachElement(eFunc);
        return result;
    }
    public static List<T> ThruFuncIndex<T>(this List<T> list, Func<int, T> iFunc) {
        List<T> result = new List<T>(list);
        result.SetEachIndex(iFunc);
        return result;
    }
    public static List<T> ThruFuncElementIndex<T>(this List<T> list, Func<T, int, T> eiFunc) {
        List<T> result = new List<T>(list);
        result.SetEachElementIndex(eiFunc);
        return result;
    }

    #region Reduce
    /// <summary>
    /// Reduces an IEnumerable to a single value
    /// </summary>
    /// <typeparam name="E">The type of the elements in the <paramref name="list"/></typeparam>
    /// <typeparam name="V">The type of the value to which the <paramref name="list"/> is being reduced</typeparam>
    /// <param name="list">The IEnumerable that is being reduced</param>
    /// <param name="func">The reducing delegate function, it takes an element of the <paramref name="list"/> and the current reduced value and returns the newly reduced value</param>
    /// <param name="initial">The initial value to start the iteration. This value is sent as the parameter of <paramref name="func"/> for the first element on <paramref name="list"/></param>
    /// <returns>The resulting value after reducing each element from <paramref name="list"/></returns>
    public static V Reduce<E, V>(this IEnumerable<E> list, Func<E, V, V> func, V initial = default(V)) {
        V result = initial;
        list.ForEach(e => result = func(e, result));
        return result;
    }
    /// <summary>
    /// Reduces an IEnumerable to a single value
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/> and the type of the return value</typeparam>
    /// <param name="list">The IEnumerable that is being reduced</param>
    /// <param name="func">The reducing delegate function, it takes an element of the <paramref name="list"/> and the current reduced value and returns the newly reduced value</param>
    /// <param name="initial">The initial value to start the iteration. This value is sent as the parameter of <paramref name="func"/> for the first element on <paramref name="list"/></param>
    /// <returns>The resulting value after reducing each element from <paramref name="list"/></returns>
    public static T Reduce<T>(this IEnumerable<T> list, Func<T, T, T> func, T initial = default(T)) {
        return list.Reduce<T, T>(func, initial);
    }
    /// <summary>
    /// Reduces an IEnumerable to a single value
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <paramref name="list"/> and the type of the return value</typeparam>
    /// <param name="list">The IEnumerable that is being reduced</param>
    /// <param name="func">The reducing delegate function, it takes an element of the <paramref name="list"/> and the current reduced value and returns the newly reduced value</param>
    /// <param name="var">The variable from and to which the <paramref name="list"/> is reduced. In each iteration it is used as the second parameter of <paramref name="func"/> and it's result is applied to it</param>
    public static void Reduce<T>(this IEnumerable<T> list, Func<T, T, T> func, ref T var) {
        foreach (T e in list) {
            var = func.Invoke(e, var);
        }
    }

    public static V Reduce<E, V>(this IEnumerable<E> list, Func<E, V, V> func, Predicate<V> stopConditionBasedOnAccumulator, V initial = default(V), bool stopBeforeAction = false) {
        V result = initial;
        list.ForEachBreak(e => result = func(e, result), e => stopConditionBasedOnAccumulator.Invoke(result), stopBeforeAction);
        return result;
    }
    public static T Reduce<T>(this IEnumerable<T> list, Func<T, T, T> func, Predicate<T> stopConditionBasedOnAccumulator, T initial = default(T), bool stopBeforeAction = false) {
        T result = initial;
        list.ForEachBreak(e => result = func(e, result), e => stopConditionBasedOnAccumulator.Invoke(result), stopBeforeAction);
        return result;
    }

    public static List<T> Accumulated<T>(this IEnumerable<T> list, Func<T, T, T> func, T initial = default(T)) {
        List<T> result = new List<T>();
        T accumulator = initial;
        list.ForEach(e => {
            accumulator = func(e, accumulator);
            result.Add(accumulator);
            }
        );
        return result;
    }
    #endregion
    #endregion
    #endregion

    #region List Method Variants
    #region Find
    public static T Find<T>(this IEnumerable<T> list, Predicate<T> predicate) {
        foreach (T e in list) {
            if (predicate.Invoke(e)) {
                return e;
            }
        }
        return default(T);
    }
    public static int FindIndex<T>(this IList<T> list, Predicate<T> predicate) {
        int n = list.Count;
        for (int i = 0; i < n; i++) {
            if (predicate.Invoke(list[i])) {
                return i;
            }
        }
        return -1;
    }
    public static List<T> FindAll<T>(this IEnumerable<T> list, Predicate<T> predicate) {
        List<T> result = new List<T>();
        foreach (T e in list) {
            if (predicate.Invoke(e)) {
                result.Add(e);
            }
        }
        return result;
    }
    public static List<int> FindAllIndex<T>(this IList<T> list, Predicate<T> predicate) {
        List<int> result = new List<int>();
        int n = list.Count;
        for (int i = 0; i < n; i++) {
            if (predicate.Invoke(list[i])) {
                result.Add(i);
            }
        }
        return result;
    }
    public static T FindThruIndex<T>(this IList<T> list, Predicate<int> indexPredicate) {
        for (int i = 0; i < list.Count; i++) {
            if (indexPredicate(i)) {
                return list[i];
            }
        }
        return default(T);
    }
    public static int FindIndexThruIndex<T>(this ICollection<T> list, Predicate<int> indexPredicate) {
        for (int i = 0; i < list.Count; i++) {
            if (indexPredicate(i)) {
                return i;
            }
        }
        return -1;
    }
    public static List<T> FindAllThruIndex<T>(this IList<T> list, Predicate<int> indexPredicate) {
        List<T> result = new List<T>();
        for (int i = 0; i < list.Count; i++) {
            if (indexPredicate(i)) {
                result.Add(list[i]);
            }
        }
        return result;
    }
    public static List<int> FindAllIndexThruIndex<T>(this ICollection<T> list, Predicate<int> indexPredicate) {
        List<int> result = new List<int>();
        for (int i = 0; i < list.Count; i++) {
            if (indexPredicate(i)) {
                result.Add(i);
            }
        }
        return result;
    }

    public static int FindAccumulatedIndex(this IList<int> list, int targetIndex) {
        int sum = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += list[i];
            if (sum >= targetIndex) {
                return i;
            }
        }
        return -1;
    }
    public static int FindAccumulatedElement(this IList<int> list, int targetValue) {
        int sum = 0;
        foreach (int e in list) {
            sum += e;
            if (sum >= targetValue) {
                return e;
            }
        }
        return default;
    }
    public static int FindAccumulatedIndex(this IList<float> list, float targetValue) {
        float sum = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += list[i];
            if (sum >= targetValue) {
                return i;
            }
        }
        return -1;
    }
    public static float FindAccumulatedElement(this IList<float> list, float targetValue) {
        float sum = 0;
        foreach (int e in list) {
            sum += e;
            if (sum >= targetValue) {
                return e;
            }
        }
        return default;
    }
    public static int FindAccumulatedIndex<T>(this IList<T> list, int targetIndex, Func<T, int> elementToSize) {
        int sum = 0;
        for (int i = 0; i < list.Count; i++) {
            sum += elementToSize.Invoke(list[i]);
            if (sum >= targetIndex) {
                return i;
            }
        }
        return -1;
    }
    public static T FindAccumulatedElement<T>(this IList<T> list, int targetValue, Func<T, int> elementValue) {
        int sum = 0;
        foreach (T e in list) {
            sum += elementValue.Invoke(e);
            if (sum >= targetValue) {
                return e;
            }
        }
        return default;
    }
    public static int FindAccumulatedIndex<T>(this IList<T> list, float targetValue, Func<T, float> elementValue) {
        float sum = 0;
        for (int i=0; i<list.Count; i++) {
            sum += elementValue.Invoke(list[i]);
            if (sum >= targetValue) {
                return i;
            }
        }
        return -1;
    }
    public static T FindAccumulatedElement<T>(this IList<T> list, float targetValue, Func<T, float> elementValue) {
        float sum = 0;
        foreach (T e in list) {
            sum += elementValue.Invoke(e);
            if (sum >= targetValue) {
                return e;
            }
        }
        return default;
    }

    public static bool FindAccumulatedElementPair<T>(this IList<T> list, float target, Func<T, T, float> pairValue, out float localTargetValue, out T element1, out T element2) {
        List<float> pairValues = list.MergedInPairs(pairValue); 
        float value = target * pairValues.Sum();
        float sum = 0;
        localTargetValue = 0;
        element1 = default;
        element2 = default;
        for (int i=0; i< pairValues.Count; i++) {
            float v = pairValues[i];
            if (sum + v >= value) {
                localTargetValue = (value - sum) / v;
                element1 = list[i];
                element2 = list[i+1];
                return true;
            }
            sum += v;
        }
        return false;
    }
    #endregion

    #region Add
    public static void AddIfDoesntContain<T>(this ICollection<T> list, T element) {
        if (!list.Contains(element)) {
            list.Add(element);
        }
    }
    public static void AddRangeIfDoesntContain<T>(this ICollection<T> list, IEnumerable<T> elements) {
        foreach (T element in elements) {
            if (!list.Contains(element)) {
                list.Add(element);
            }
        }
    }

    public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) {
        if (dictionary.ContainsKey(key)) {
            dictionary[key] = value;
        }
        else {
            dictionary.Add(key, value);
        }
    }

    public static List<T> With<T>(this List<T> list, T element) {
        List<T> result = new List<T>(list);
        result.Add(element);
        return result;
    }
    public static List<T> With<T>(this List<T> list, int index, T element) {
        List<T> result = new List<T>(list);
        result.Insert(index ,element);
        return result;
    }
    #endregion

    #region Remove
    public static void RemoveAt<T>(this IList<T> list, IList<int> indices) {
        for (int i = 0; i < indices.Count; i++) {
            list.RemoveAt(indices[i] - i);
        }
    }
    public static void RemoveRange<T>(this IList<T> list, int start, int count) {
        for (int i = 0; i < count; i++) {
            list.RemoveAt(start);
        }
    }

    public static List<T> Without<T>(this ICollection<T> list, T elementToRemove) {
        List<T> result = new List<T>(list);
        result.Remove(elementToRemove);
        return result;
    }
    #endregion

    #region GetRange
    public static List<T> StartingAt<T>(this IList<T> list, int start) {
        return list.GetRange(start, list.Count - start);
    }
    public static List<T> StartingAt<T>(this IEnumerable<T> list, int start) {
        int i = 0;
        List<T> result = new List<T>();
        foreach (T item in list) {
            if (i >= start) {
                result.Add(item);
            }
        }
        return result;
    }
    public static List<T> GetRange<T>(this IList<T> list, int start, int size, bool ciclic = false) {
        List<T> result = new List<T>();
        int listCount = list.Count;
        if (ciclic) {
            for (int i = 0; i < size; i++) {
                int j = (start + i) % listCount;
                result.Add(list[j]);
            }
        }
        else {
            if (start + size <= listCount) {
                for (int i = 0; i < size; i++) {
                    result.Add(list[start + i]);
                }
            }
            else {
                return null;
            }
        }
        return result;
    }
    public static List<T> GetRange<T>(this IEnumerable<T> list, int start, int size, bool ciclic = false) {
        List<T> result = new List<T>();
        int i = 0;
        int count = 0;
        foreach (T e in list) {
            if (i < start) {
                if (count == size) {
                    break;
                }
                result.Add(e);
                count++;
            }
            i++;
        }
        if (ciclic) {
            while (count < size) {
                foreach (T e in list) {
                    if (count == size) {
                        break;
                    }
                    result.Add(e);
                    count++;
                }
            }
        }
        return result;
    }
    #endregion

    #region Count
    public static int Count<T>(this IEnumerable<T> list, Predicate<T> predicate) {
        int count = 0;
        foreach (T t in list) {
            if (predicate.Invoke(t)) {
                count++;
            }
        }
        return count;
    }
    public static Dictionary<T, int> CountOccurrences<T>(this IEnumerable<T> list) {
        Dictionary<T, int> result = new Dictionary<T, int>();
        foreach (T e in list) {
            if (result.ContainsKey(e)) {
                result[e]++;
            }
            else {
                result.Add(e, 1);
            }
        }
        return result;
    }
    public static List<ListElement<T, int>> CountOccurrences<T>(this IList<T> list, Func<T, T, bool> equalsFunc) {
        List<ListElement<T, int>> result = new List<ListElement<T, int>>();
        int n = list.Count;
        for (int i = 0; i < n; i++) {
            T e = list[i];
            int index = result.FindIndex(le => equalsFunc.Invoke(le.element, e));
            if (index >= 0) {
                result[index] = new ListElement<T, int>() { element = e, index = i, value = result[index].value + 1 };
            }
            else {
                result.Add(new ListElement<T, int>() { element = e, index = i, value = 1 });
            }
        }
        return result;
    }
    #endregion

    #region Contains
    public static bool ContainsAny<T>(this IEnumerable<T> list, ICollection<T> possibilities) {
        foreach (T e in list) {
            if (possibilities.Contains(e)) {
                return true;
            }
        }
        return false;
    }
    public static bool ContainsAll<T>(this IEnumerable<T> list, ICollection<T> other) {
        foreach (T e in other) {
            if (!list.Contains(e)) {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Equals
    public static bool EqualsAllOrdered<T>(this IList<T> list, IList<T> other) {
        if (list == null || other == null) {
            return (list == null && other == null);
        }
        if (list.Count != other.Count) {
            return false;
        }
        return list.SequenceEqual(other);
    }
    public static bool EqualsAll<T>(this IList<T> aListA, IList<T> aListB) {
        if (aListA == null || aListB == null || aListA.Count != aListB.Count)
            return false;
        if (aListA.Count == 0)
            return true;
        Dictionary<T, int> lookUp = new Dictionary<T, int>(aListA.Count);
        // create index for the first list
        for (int i = 0; i < aListA.Count; i++) {
            int count = 0;
            if (!lookUp.TryGetValue(aListA[i], out count)) {
                lookUp.Add(aListA[i], 1);
                continue;
            }
            lookUp[aListA[i]] = count + 1;
        }
        for (int i = 0; i < aListB.Count; i++) {
            int count = 0;
            if (!lookUp.TryGetValue(aListB[i], out count)) {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(aListB[i]);
            else
                lookUp[aListB[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }
    #endregion

    #region Sort
    public static List<T> SortedBy<T>(this IList<T> list, IList<int> indices) {
        List<T> result = new List<T>();
        for (int i = 0; i < indices.Count; i++) {
            result.Add(list[indices[i]]);
        }
        return result;
    }

    public static List<T> InvertedList<T>(this IEnumerable<T> list) {
        List<T> result = new List<T>();
        foreach (T e in list) {
            result.Insert(0, e);
        }
        return result;
    }
    public static void Invert<T>(this IList<T> list) {
        int n = list.Count;
        for (int i = 1; i < n; i++) {
            T e = list[i];
            list.RemoveAt(i);
            list.Insert(0, e);
        }
    }
    #endregion

    #region ConvertAll
    public static List<TOut> ConvertAll<TIn, TOut>(this IEnumerable<TIn> list, Func<TIn, TOut> converter) {
        List<TOut> result = new List<TOut>();
        foreach (TIn e in list) {
            result.Add(converter.Invoke(e));
        }
        return result;
    }
    public static List<TOut> ConvertAllThruIndex<TIn, TOut>(this IList<TIn> list, Func<int, TIn, TOut> converter) {
        List<TOut> result = new List<TOut>();
        for (int i = 0; i < list.Count; i++) {
            result.Add(converter.Invoke(i, list[i]));
        }
        return result;
    }
    #endregion

    #region TrueForAll
    public static bool TrueForAll<T>(this IEnumerable<T> list, Predicate<T> match) {
        foreach (T e in list) {
            if (!match.Invoke(e)) {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Exists
    public static bool Exists<T>(this IEnumerable<T> list, Predicate<T> match) {
        foreach (T e in list) {
            if (!match.Invoke(e)) {
                return true;
            }
        }
        return false;
    }
    public static bool ExistsConsecutivePair<T>(this IList<T> list, Func<T, T, bool> match, bool ciclic = false, bool jumpUsedElement = false) {
        bool exists = false;
        list.ForEachConsecutivePair((e0, e1) => {
            if (match.Invoke(e0, e1)) {
                exists = true;
            }
        }, ciclic, jumpUsedElement);
        return exists;
    }
    public static bool ExistsConsecutiveN<T>(this IList<T> list, int N, Predicate<IList<T>> match, bool ciclic = false, bool jumpUsedElement = false) {
        bool exists = false;
        list.ForEachConsecutiveN(N, l => {
            if (match.Invoke(l)) {
                exists = true;
            }
        }, ciclic, jumpUsedElement);
        return exists;
    }
    #endregion
    #endregion

    #region Doubles
    public static bool HasDoubles<T>(this IEnumerable<T> list) {
        Dictionary<T, int> check = new Dictionary<T, int>();
        foreach (T e in list) {
            if (check.ContainsKey(e)) {
                return true;
            }
            else {
                check.Add(e, 1);
            }
        }
        return false;
    }
    public static bool HasDoubles<T>(this IList<T> list, Func<T, T, bool> equalsFunc) {
        int n = list.Count;
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (i != j && equalsFunc.Invoke(list[i], list[j])) {
                    return true;
                }
            }
        }
        return false;
    }

    public static List<T> WithoutDoubles<T>(this IEnumerable<T> list) {
        List<T> result = new List<T>();
        list.ForEach(e => result.AddIfDoesntContain(e));
        return result;
    }
    #endregion

    #region Formating
    public enum SplitOptions {
        DiscardSplitPoint,
        PutSplitPointLeft,
        PutSplitPointRight
    }
    public static List<List<T>> Split<T>(this IList<T> list, IEnumerable<int> splitPoints, SplitOptions options) {
        List<List<T>> result = new List<List<T>>();
        List<T> curSection = new List<T>();
        for (int i = 0; i < list.Count; i++) {
            if (splitPoints.Contains(i)) {
                switch (options) {
                    case SplitOptions.DiscardSplitPoint:
                        result.Add(curSection);
                        curSection = new List<T>();
                        break;
                    case SplitOptions.PutSplitPointLeft:
                        curSection.Add(list[i]);
                        result.Add(curSection);
                        curSection = new List<T>();
                        break;
                    case SplitOptions.PutSplitPointRight:
                        result.Add(curSection);
                        curSection = new List<T> { list[i] };
                        break;
                    default:
                        result.Add(curSection);
                        curSection = new List<T>();
                        break;
                }
            }
            else {
                curSection.Add(list[i]);
            }
        }
        return result;
    }

    public static List<List<T>> Group<T>(this IEnumerable<T> list, Func<T, T, bool> classifier) {
        List<List<T>> result = new List<List<T>>();
        foreach (T item in list) {
            List<T> group = result.Find(g => classifier.Invoke(g.First(), item));
            if (group != null) {
                group.Add(item);
            }
            else {
                result.Add(new List<T>() { item });
            }
        }
        return result;
    }

    public static List<T> AsSegments<T>(this List<T> list, Func<T, T, bool> sameSegment) {
        List<T> result = new List<T>(list);
        int k;
        for (int i = 0; i < result.Count; i++) {
            k = i;
            for (int j = i + 1; j < result.Count; j++) {
                bool canRemoveBetween = sameSegment.Invoke(result[i], result[j]);
                if (canRemoveBetween) {
                    k = j;
                }
                else if (k != i) {
                    int removeCount = k - i - 1;
                    result.RemoveRange(i + 1, removeCount);
                    k -= removeCount;
                    break;
                }
            }
            if (k != i) {
                int removeCount = k - i - 1;
                result.RemoveRange(i + 1, removeCount);
            }
        }
        return result;
    }

    public static List<T> ToSingleList<C, T>(this IEnumerable<C> list, Func<C, List<T>> func) {
        List<T> result = new List<T>();
        foreach (C collection in list) {
            if (collection != null) {
                result.AddRange(func.Invoke(collection));
            }
        }
        return result;
    }
    public static List<T> ToSingleList<T>(this IEnumerable<List<T>> list) {
        List<T> result = new List<T>();
        foreach (IEnumerable<T> group in list) {
            result.AddRange(group);
        }
        return result;
    }
    public static List<T> ToSingleList<T>(this IEnumerable<T[]> list) {
        List<T> result = new List<T>();
        foreach (IEnumerable<T> group in list) {
            result.AddRange(group);
        }
        return result;
    }
    public static List<T> ToSingleList<T>(this T[,] matrix) {
        List<T> result = new List<T>();
        foreach (T element in matrix) {
            result.Add(element);
        }
        return result;
    }

    public static List<T> IntercalatedWith<T>(this IList<T> list, IList<T> other, bool startWithOther = false) {
        List<T> result = new List<T>();

        int N = list.Count;
        int M = other.Count;
        int i;
        for (i = 0; i < N && i < M; i++) {
            if (startWithOther) {
                result.Add(other[i]);
                result.Add(list[i]);
            }
            else {
                result.Add(list[i]);
                result.Add(other[i]);
            }
        }
        if (i < N) {
            result.AddRange(list.GetRange(i, N - i));
        }
        else if (i < M) {
            result.AddRange(other.GetRange(i, M - i));
        }

        return result;
    }

    public static T[,] BreakIntoMatrix<T>(this T[] array, int width) {
        int length = array.Length;
        int height = Mathf.CeilToInt(length / (float)width);
        T[,] matrix = new T[height, width];
        int x = 0;
        int y = 0;
        for (int i = 0; i < length; i++) {
            matrix[y, x] = array[i];
            x++;
            if (x == width) {
                x = 0;
                y++;
            }
        }
        return matrix;
    }

    public static List<TOut> MergedInPairs<T, TOut>(this IList<T> list, Func<T, T, TOut> merger, bool ciclic = false, bool jumpUsedElement = false) {
        List<TOut> result = new List<TOut>();
        list.ForEachConsecutivePair((x1, x2) => result.Add(merger.Invoke(x1, x2)), ciclic, jumpUsedElement);
        return result;
    }
    public static List<TOut> MergedInsTrios<T, TOut>(this IList<T> list, Func<T, T, T, TOut> merger, bool ciclic = false, bool jumpUsedElement = false) {
        List<TOut> result = new List<TOut>();
        list.ForEachConsecutiveTrio((x1, x2, x3) => result.Add(merger.Invoke(x1, x2, x3)), ciclic, jumpUsedElement);
        return result;
    }
    public static List<TOut> MergedInN<T, TOut>(this IList<T> list, int N, Func<IList<T>, TOut> merger, bool ciclic = false, bool jumpUsedElement = false) {
        List<TOut> result = new List<TOut>();
        list.ForEachConsecutiveN(N, x => result.Add(merger.Invoke(x)), ciclic, jumpUsedElement);
        return result;
    }

    public static List<T> AlteredClone<T>(this IList<T> list, params Action<List<T>>[] actions) {
        List<T> result = new List<T>(list);
        actions.ForEach(a => a.Invoke(result));
        return result;
    }
    #endregion

}

