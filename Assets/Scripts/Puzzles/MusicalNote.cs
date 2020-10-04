using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicalNote : MonoBehaviour {

    public int id = 0;
    public UnityEvent onSuccess;
    public UnityEvent onError;

    public void TriggerNote() {
        if (MusicalSequence.FirstEnabled != null) {
            if (MusicalSequence.FirstEnabled.TryProgress(id)) {
                onSuccess.Invoke();
            }
            else {
                Debug.Log("Wrong note played");
                onError.Invoke();
            }
        }
    }

}
