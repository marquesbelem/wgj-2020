using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicalNote : MonoBehaviour {

    public int id = 0;
    public UnityEvent onSuccess;
    public UnityEvent onError;

    public void TriggerNote() {
        if (MusicalSequence.instance != null) {
            if (MusicalSequence.instance.TryProgress(id)) {
                onSuccess.Invoke();
            }
            else {
                onError.Invoke();
            }
        }
    }

}
