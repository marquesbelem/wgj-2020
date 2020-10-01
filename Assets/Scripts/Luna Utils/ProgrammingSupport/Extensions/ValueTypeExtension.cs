using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class StringExtension {

        public static List<char> SelectChars(this string s, List<int> indices) {
            List<char> result = new List<char>();
            indices.ForEach(i => result.Add(s[i]));
            return result;
        }

        public static int FindIndex(this string s, char c) {
            int n = s.Length;
            for (int i = 0; i < n; i++) {
                if (s[i] == c) {
                    return i;
                }
            }
            return -1;
        }
        public static List<int> FindAllIndex(this string s, Predicate<char> predicate) {
            List<int> result = new List<int>();
            int n = s.Length;
            for (int i = 0; i < n; i++) {
                if (predicate.Invoke(s[i])) {
                    result.Add(i);
                }
            }
            return result;
        }

        public static string RemoveFirstIf(this string s, Predicate<char> predicate) {
            if (s.IsNullOrEmpty()) {
                return s;
            }
            if (predicate.Invoke(s[0])) {
                return s.Substring(1, s.Length - 1);
            }
            return s;
        }
        public static string RemoveLastIf(this string s, Predicate<char> predicate) {
            if (s.IsNullOrEmpty()) {
                return s;
            }
            if (predicate.Invoke(s[s.Length - 1])) {
                return s.Substring(0, s.Length - 1);
            }
            return s;
        }

        public static string Cleaned(this string s, char charToRemove) {
            string result = "";
            for (int i = 0; i < s.Length; i++) {
                if (charToRemove != s[i]) {
                    result += s[i];
                }
            }
            return result;
        }
        public static string Cleaned(this string s, ICollection<char> charsToRemove) {
            string result = "";
            for (int i = 0; i < s.Length; i++) {
                if (!charsToRemove.Contains(s[i])) {
                    result += s[i];
                }
            }
            return result;
        }

        public static string RemoveDirectlyNestedParenthesis(this string s, char open = '(', char close = ')') {
            for (int i = 0; i < s.Length; i++) {
                if (s[i] == open) {
                    int? closeIndex = s.Find0DepthCloseIndex(i, open, close);
                    if (closeIndex != null) {
                        int j = i + 1;
                        int k = closeIndex.Value - 1;
                        if (j < k && s[j] == open && s[k] == close) {
                            s = s.Split(new List<int>() { j, k }, StringSplitOptions.RemoveEmptyEntries).Merge();
                            i--;
                        }
                    }
                }
            }
            return s;
        }
        public static string RemoveBorderParenthesis(this string s, char open = '(', char close = ')') {
            if (s.IsNullOrEmpty()) {
                return s;
            }
            if (s[0] == open) {
                int? closeIndex = s.Find0DepthCloseIndex(0, open, close);
                if (closeIndex != null && closeIndex.Value == s.Length - 1) {
                    return s.Substring(1, s.Length - 2);
                }
            }
            return s;
        }
        public static int ParenthesisDepthAt(this string s, int index, char open = '(', char close = ')') {
            int parenthesisDepth = 0;
            for (int i = 0; i < index; i++) {
                if (s[i] == open) {
                    parenthesisDepth++;
                }
                else if (s[i] == close) {
                    parenthesisDepth--;
                }
            }
            return parenthesisDepth;
        }
        public static int? Find0DepthCloseIndex(this string s, int openIndex, char open = '(', char close = ')') {
            int parenthesisDepth = 0;
            for (int i = openIndex + 1; i < s.Length; i++) {
                if (s[i] == open) {
                    parenthesisDepth++;
                }
                else if (s[i] == close) {
                    if (parenthesisDepth == 0) {
                        return i;
                    }
                    parenthesisDepth--;
                }
            }
            return null;
        }

        public static List<string> Split(this string s, ICollection<int> splitPoints, StringSplitOptions options = StringSplitOptions.None) {
            List<string> result = new List<string>();
            string curSection = "";
            for (int i = 0; i < s.Length; i++) {
                if (splitPoints.Contains(i)) {
                    if ((options == StringSplitOptions.None) || (!curSection.IsNullOrEmpty())) {
                        result.Add(curSection);
                    }
                    curSection = "";
                }
                else {
                    curSection += s[i];
                }
            }
            if ((options == StringSplitOptions.None) || (!curSection.IsNullOrEmpty())) {
                result.Add(curSection);
            }
            return result;
        }
        public static string Merge(this IEnumerable<string> list) {
            return list.Reduce<string>((a, b) => b + a, "");
        }

        public static int CountAll(this string s, char charToCount) {
            int result = 0;
            foreach (char c in s) {
                if (c == charToCount) {
                    result++;
                }
            }
            return result;
        }

        public static bool IsNullOrEmpty(this string s) {
            return string.IsNullOrEmpty(s);
        }

    }

    public static partial class FloatExtension {

        public static float LerpedTo(this float v1, float v2, float t) {
            return Mathf.Lerp(v1, v2, t);
        }
        public static float DistanceTo(this float v, float other) {
            return FloatUtils.Distance(v, other);
        }

        public static Vector2 ToVector2(this float v) {
            return new Vector2(v, v);
        }
        public static Vector3 ToVector3(this float v) {
            return new Vector3(v, v, v);
        }

        public static int RoundToInt(this float v, RoundMethod roundMethod = RoundMethod.Round) {
            switch (roundMethod) {
                case RoundMethod.Round:
                    return Mathf.RoundToInt(v);
                case RoundMethod.Floor:
                    return Mathf.FloorToInt(v);
                case RoundMethod.Ceil:
                    return Mathf.CeilToInt(v);
                default:
                    return default;
            }
        }
        public static float Sign(this float v) {
            return Mathf.Sign(v);
        }
        public static float Abs(this float v) {
            return Mathf.Abs(v);
        }

    }

    public static partial class IntExtension {

        public static int RotatedInside(this int v, int d) {
            return IntUtils.RotateInside(v, d);
        }

        public static Vector2Int ToVector2(this int v) {
            return new Vector2Int(v, v);
        }
        public static Vector3Int ToVector3(this int v) {
            return new Vector3Int(v, v, v);
        }

        public static int Sign(this int v) {
            return Mathf.Sign(v).RoundToInt();
        }
        public static int Abs(this int v) {
            return Mathf.Abs(v);
        }

    }

}
