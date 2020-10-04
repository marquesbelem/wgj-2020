using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {
    
    public int index = 0;
    [Serializable] public class ObjectList {
        public List<GameObject> list;
    }
    public List<ObjectList> onlyActiveOnLevel;
    public UnityEvent onEnteredAny;
    public List<UnityEvent> onEnterLevel;
    public UnityEvent onCompletedAll;

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
                for (int i = 0; i < onlyActiveOnLevel.Count; i++) {
                    onlyActiveOnLevel[i].list.ForEach(o => o.SetActive(i == index));
                }
                onEnteredAny.Invoke();
                onEnterLevel[index].Invoke();
            }
            else {
                onCompletedAll.Invoke();
            }
        }
    }

}
