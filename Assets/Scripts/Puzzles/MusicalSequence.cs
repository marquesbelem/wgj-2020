using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicalSequence : MonoBehaviour {

    public static MusicalSequence instance;
    private void OnEnable() {
        instance = this;
    }

    public List<int> sequence;
    public UnityEvent onSequenceSuccess;
    public UnityEvent onNoteError;

    private int curIndex = 0;

    public bool TryProgress(int noteId) {
        if (curIndex < sequence.Count) {
            if (sequence[curIndex] == noteId) {
                curIndex++;
                if (curIndex == sequence.Count) {
                    onSequenceSuccess.Invoke();
                }
                return true;
            }
            else {
                onNoteError.Invoke();
                return false;
            }
        }
        return false;
    }

}
