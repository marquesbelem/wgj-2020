using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GalloUtils {
    namespace Lib {

        [Serializable] public abstract class SerializableCondition<T> : MonoBehaviour {
            public abstract bool CheckCondition(T value);
        }
        [Serializable] public abstract class SerializableCondition<T1, T2> {
            public abstract bool CheckCondition(T1 value1, T2 value2);
        }

        [Serializable] public class MatchCondition<T> : SerializableCondition<T> {
            public T matchValue;

            public override bool CheckCondition(T value) {
                return value.Equals(matchValue);
            }
        }

    }
}
