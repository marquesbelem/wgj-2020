using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    public static partial class GameObjectExtension {

        public static void SetAllActive(this List<GameObject> list, bool value) {
            list.ForEach(go => go.SetActive(value));
        }

        public static List<TComponent> GetAllComponents<TComponent>(this List<GameObject> list) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            foreach (GameObject obj in list) {
                TComponent component = obj.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }
        public static List<TComponent> GetAllComponents<TComponent>(this List<Transform> list) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            foreach (Transform t in list) {
                TComponent component = t.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }
        public static List<T2> GetAllComponents<T1, T2>(this List<T1> list) where T1 : Component where T2 : Component {
            List<T2> result = new List<T2>();
            foreach (T1 c in list) {
                T2 component = c.GetComponent<T2>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }

        public static List<GameObject> GetChildrenFamilies(this GameObject self) {
            return self.transform.GetChildrenFamilies().ConvertAll(t => t.gameObject);
        }
        public static List<Transform> GetChildrenFamilies(this Transform self) {
            List<Transform> result = new List<Transform>();
            foreach (Transform child in self) {
                result.Add(child);
                result.AddRange(child.GetChildrenFamilies());
            }
            return result;
        }
        public static List<TComponent> GetChildrenFamilies<TComponent>(this TComponent self) where TComponent : Component {
            List<TComponent> result = new List<TComponent>();
            List<GameObject> objList = self.gameObject.GetChildrenFamilies();
            foreach (GameObject obj in objList) {
                TComponent component = obj.GetComponent<TComponent>();
                if (component != null) {
                    result.Add(component);
                }
            }
            return result;
        }

        #region LineRenderer
        public static List<Vector3> GetPositionList(this LineRenderer line) {
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);
            return new List<Vector3>(positions);
        }
        public static void AddPosition(this LineRenderer line, Vector3 pos) {
            int count = line.positionCount;
            line.positionCount = count + 1;
            line.SetPosition(count, pos);
        }
        public static float GetLength(this LineRenderer line) {
            return line.GetPositionList().PathLength();
        }
        #endregion

        #region KeyFrame
        public static Keyframe WithValue(this Keyframe k, float newValue) {
            return new Keyframe(k.time, newValue, k.inTangent, k.outTangent, k.inWeight, k.outWeight);
        }
        public static Keyframe WithValue(this Keyframe k, Func<float, float> valueFunc) {
            return k.WithValue(valueFunc.Invoke(k.value));
        }

        public static Keyframe WithTime(this Keyframe k, float newTime) {
            return new Keyframe(newTime, k.value, k.inTangent, k.outTangent, k.inWeight, k.outWeight);
        }
        public static Keyframe WithTime(this Keyframe k, Func<float, float> timeFunc) {
            return k.WithTime(timeFunc.Invoke(k.time));
        }

        public static Keyframe ScaledBy(this Keyframe k, Vector2 vector) {
            return new Keyframe(
                k.time * vector.x,
                k.value * vector.y,
                k.inTangent * vector.y / vector.x,
                k.outTangent * vector.y / vector.x
            );
        }

        public static List<Keyframe> InvertedCurve(this List<Keyframe> list) {
            List<Keyframe> result = list.InvertedList();
            result.SetEachElement(k => k.ScaledBy(new Vector2(-1f, 1f)).WithTime(t => t + 1f));
            return result;
        }
        public static void InvertCurve(this List<Keyframe> list) {
            list.Invert();
            list.SetEachElement(k => k.ScaledBy(new Vector2(-1f, 1f)).WithTime(t => t + 1f));
        }
        #endregion

    }

}
