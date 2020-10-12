using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


#region SingleTypedEvents
[Serializable] public class BooleanEvent : UnityEvent<bool> { }
[Serializable] public class IntegerEvent : UnityEvent<int> { }
[Serializable] public class FloatEvent : UnityEvent<float> { }
[Serializable] public class CharEvent : UnityEvent<char> { }
[Serializable] public class StringEvent : UnityEvent<string> { }
[Serializable] public class ColorEvent : UnityEvent<Color> { }
[Serializable] public class Vector2Event : UnityEvent<Vector2> { }
[Serializable] public class Vector3Event : UnityEvent<Vector3> { }
[Serializable] public class Vector2IntEvent : UnityEvent<Vector2Int> { }
[Serializable] public class Vector3IntEvent : UnityEvent<Vector3Int> { }
[Serializable] public class QuaternionEvent : UnityEvent<Quaternion> { }
[Serializable] public class RectEvent : UnityEvent<Rect> { }
[Serializable] public class SpriteEvent : UnityEvent<Sprite> { }
[Serializable] public class GameObjectEvent : UnityEvent<GameObject> { }
[Serializable] public class TransformEvent : UnityEvent<Transform> { }
[Serializable] public class MaterialEvent : UnityEvent<Material> { }
[Serializable] public class AudioClipEvent : UnityEvent<AudioClip> { }

#region ListEvents
[Serializable] public class BooleanListEvent : UnityEvent<List<bool>> { }
[Serializable] public class IntegerListEvent : UnityEvent<List<int>> { }
[Serializable] public class FloatListEvent : UnityEvent<List<float>> { }
[Serializable] public class CharListEvent : UnityEvent<List<char>> { }
[Serializable] public class StringListEvent : UnityEvent<List<string>> { }
[Serializable] public class ColorListEvent : UnityEvent<List<Color>> { }
[Serializable] public class Vector2ListEvent : UnityEvent<List<Vector2>> { }
[Serializable] public class Vector3ListEvent : UnityEvent<List<Vector3>> { }
[Serializable] public class QuaternionListEvent : UnityEvent<List<Quaternion>> { }
[Serializable] public class RectListEvent : UnityEvent<List<Rect>> { }
[Serializable] public class SpriteListEvent : UnityEvent<List<Sprite>> { }
[Serializable] public class GameObjectListEvent : UnityEvent<List<GameObject>> { }
[Serializable] public class TransformListEvent : UnityEvent<List<Transform>> { }
#endregion
#endregion


#region DoubleTypedEvents
[Serializable] public class DoubleBooleanEvent : UnityEvent<bool, bool> { }
[Serializable] public class IntegerBooleanEvent : UnityEvent<int, bool> { }
#endregion

#region LabeledEvent
[Serializable]
public class LabeledEvent<L> {
    public L label;
    public UnityEvent unityEvent;

    public void TryInvoke(L matcingLabel) {
        if (label.Equals(matcingLabel)) {
            unityEvent.Invoke();
        }
    }
}
[Serializable]
public class LabeledEvent<L, E, T> where E : UnityEvent<T> {
    public L label;
    public E unityEvent;

    public void TryInvoke(L matcingLabel, T value) {
        if (label.Equals(matcingLabel)) {
            unityEvent.Invoke(value);
        }
    }
}

#region IdentifierEvents
[Serializable] public class IdentifierEvent : LabeledEvent<string> { }
[Serializable] public class BooleanIdentifierEvent : LabeledEvent<string, BooleanEvent, bool> { }
[Serializable] public class IntegerIdentifierEvent : LabeledEvent<string, IntegerEvent, int> { }
[Serializable] public class FloatIdentifierEvent : LabeledEvent<string, FloatEvent, float> { }
[Serializable] public class CharIdentifierEvent : LabeledEvent<string, CharEvent, char> { }
[Serializable] public class StringIdentifierEvent : LabeledEvent<string, StringEvent, string> { }
#endregion
#endregion
