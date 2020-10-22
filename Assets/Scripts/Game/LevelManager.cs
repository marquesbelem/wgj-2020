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
        Debug.Log("Solved puzzle " + puzzleIndex + "from level " + levelIndex);
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
            Debug.Log("Going to puzzle " + puzzleIndex + "from level " + levelIndex);
            BeginWin();
        }
    }
    private void BeginWin() {
        Debug.Log("BeginWin");
        templeShot.enabled = true;
        levels.ForEach(l => l.puzzles.ForEach(p => p.Active = false));
        Invoke("ActivateCristal", 2f);
        Invoke("StartNext", 3f);
    }
    private void ActivateCristal() {
        Debug.Log("ActivateCristal");
        cristalsToActivate[levelIndex - 1].SetActive(true);
    }
    private void StartNext() {
        Debug.Log("StartNext");
        templeShot.enabled = false;
        levels.ForEach(l => l.puzzles.ForEach(p => p.Active = false));
        if (levelIndex < levels.Count) {
            levels[levelIndex].puzzles[puzzleIndex].Begin();
            onEnteredAny.Invoke();
        }
        else {
            onCompletedAll.Invoke();
        }
    }

}
