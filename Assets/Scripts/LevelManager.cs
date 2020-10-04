using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {
    
    public int index = 0;
    public UnityEvent onCompletedAll;
    public UnityEvent onEnteredAny;
    public List<UnityEvent> onEnterLevel;

    private void Start() {
        InvokeEvent();
    }

    public void WinCurrentLevel() {
        if (index < onEnterLevel.Count) {
            index++;
            InvokeEvent();
        }
    }
    public void InvokeEvent() {
        if (index < onEnterLevel.Count) {
            if (index < onEnterLevel.Count) {
                Debug.Log("Entered level " + index);
                onEnteredAny.Invoke();
                onEnterLevel[index].Invoke();
            }
            else {
                onCompletedAll.Invoke();
            }
        }
    }

}
