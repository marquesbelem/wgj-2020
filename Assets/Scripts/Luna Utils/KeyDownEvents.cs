using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyDownEvents : MonoBehaviour {
    
    [Serializable] public class KeyEvent {
        public KeyCode keyCode;
        public UnityEvent unityEvent;
    }
    public List<KeyEvent> keyEvents;

    private void Update() {
        foreach (KeyEvent ke in keyEvents) {
            if (Input.GetKeyDown(ke.keyCode)) {
                ke.unityEvent.Invoke();
            }
        }
    }

}
