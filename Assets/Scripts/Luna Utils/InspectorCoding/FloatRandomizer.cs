using System.Collections.Generic;
using UnityEngine;

public class FloatRandomizer : MonoBehaviour {

    public enum Type {
        List,
        Range
    }
    public Type type;
    public bool IsList {
        get {
            return type == Type.List;
        }
    }
    public bool IsRange {
        get {
            return type == Type.Range;
        }
    }

    public List<float> valueList;
    public float rangeMin;
    public float rangeMax;
    public FloatEvent setRandomFloat;

    public void Randomize() {
        float value = default;
        switch (type) {
            case Type.List:
                value = valueList.RandomElement();
                break;
            case Type.Range:
                value = Random.Range(rangeMin, rangeMax);
                break;
        }
        setRandomFloat.Invoke(value);
    }

}
