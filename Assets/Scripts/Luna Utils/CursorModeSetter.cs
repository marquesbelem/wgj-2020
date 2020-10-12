using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorModeSetter : MonoBehaviour {

    public void SetNone() {
        Cursor.lockState = CursorLockMode.None;
    }
    public void SetLocked() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void SetConfined() {
        Cursor.lockState = CursorLockMode.Confined;
    }

}
