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

    [Serializable] public class Level {
        public List<Puzzle> puzzles;
    }
    public List<Level> levels;
    public UnityEvent onEnteredAny;
    public UnityEvent onCompletedAll;

    private int levelIndex = 0;
    private int puzzleIndex = 0;

    private void Start() {
        InvokeEvent();
    }

    public void CurPuzzleSolved() {
        if (levelIndex < levels.Count && puzzleIndex < levels[levelIndex].puzzles.Count) {
            puzzleIndex++;
            if (puzzleIndex == levels[levelIndex].puzzles.Count) {
                WinCurrentLevel();
            }
            else {
                InvokeEvent();
            }
        }
    }
    public void WinCurrentLevel() {
        if (levelIndex < levels.Count) {
            puzzleIndex = 0;
            levelIndex++;
            InvokeEvent();
        }
    }
    public void InvokeEvent() {
        if (levelIndex < levels.Count) {
            levels.ForEach(l => l.puzzles.ForEach(p => p.Active = false));
            levels[levelIndex].puzzles[puzzleIndex].Begin();
            onEnteredAny.Invoke();
        }
        else {
            onCompletedAll.Invoke();
        }
    }

}
