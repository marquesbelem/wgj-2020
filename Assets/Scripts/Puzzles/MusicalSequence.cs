using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicalSequence : MonoBehaviour {

    public static List<MusicalSequence> instanceList = new List<MusicalSequence>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }
    public static MusicalSequence FirstEnabled {
        get {
            if (instanceList.Count == 0) {
                return null;
            }
            else {
                return instanceList[0];
            }
        }
    }


    public List<int> sequence;
    public UnityEvent onSequenceSuccess;
    public UnityEvent onNoteError;

    public int curIndex = 0;

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
                curIndex = 0;
                onNoteError.Invoke();
                return false;
            }
        }
        return false;
    }

}
