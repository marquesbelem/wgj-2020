using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    private void Awake() {
        instance = this;
    }

    public CameraShot templeShot;
    [Serializable] public class Level {
        public List<Puzzle> puzzles;
    }
    public List<Level> levels;
    public List<GameObject> cristalsToActivate;
    public UnityEvent onEnteredAny;
    public UnityEvent onCompletedAll;

    private int levelIndex = 0;
    private int puzzleIndex = 0;

    private void Start() {
        StartNext();
    }

    public void CurPuzzleSolved() {
        if (levelIndex < levels.Count && puzzleIndex < levels[levelIndex].puzzles.Count) {
            puzzleIndex++;
            if (puzzleIndex == levels[levelIndex].puzzles.Count) {
                WinCurrentLevel();
            }
            else {
                StartNext();
            }
        }
    }
    public void WinCurrentLevel() {
        if (levelIndex < levels.Count) {
            puzzleIndex = 0;
            levelIndex++;
            BeginWin();
        }
    }
    private void BeginWin() {
        templeShot.enabled = true;
        levels.ForEach(l => l.puzzles.ForEach(p => p.Active = false));
        Invoke("ActivateCristal", 1f);
        Invoke("StartNext", 2f);
    }
    private void ActivateCristal() {
        cristalsToActivate[levelIndex - 1].SetActive(true);
    }
    private void StartNext() {
        templeShot.enabled = false;
        if (levelIndex < levels.Count) {
            levels[levelIndex].puzzles[puzzleIndex].Begin();
            onEnteredAny.Invoke();
        }
        else {
            onCompletedAll.Invoke();
        }
    }

}
