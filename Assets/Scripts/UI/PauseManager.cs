using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour {
    public static bool Paused { get; private set; } = false;

    public GameObject pauseOverlay;
    public UnityEvent onPaused;
    public UnityEvent onUnpaused;

    private CursorLockMode lastMode;

    public void TogglePaused() {
        SetPaused(!Paused);
    }
    public void SetPaused(bool newValue) {
        Paused = newValue;
        pauseOverlay.SetActive(Paused);
        Time.timeScale = Paused ? 0f : 1f;
        CameraController.allowInput = !Paused;
        PointerRegionManager.SetInputBlocker(1, Paused);
        PlayerInteractor.isInputAllowed = !Paused;
        if (Paused) {
            lastMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
            onPaused.Invoke();
        }
        else {
            Cursor.lockState = lastMode;
            onUnpaused.Invoke();
        }
    }

}
