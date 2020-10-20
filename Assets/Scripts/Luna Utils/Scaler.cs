using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour {
    
    public void SetScale(float value) {
        transform.localScale = new Vector3(value, value, value);
    }

}
