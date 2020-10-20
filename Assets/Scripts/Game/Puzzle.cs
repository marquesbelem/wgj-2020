using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour {

    public int goalIndex = 0;
    public List<string> goalText;
    public List<GameObject> activableObjs;
    public UnityEvent onBegin;
    public BooleanEvent onChangeActive;
    public BooleanEvent onChangeInactive;

    public bool Active {
        set { 
            activableObjs.ForEach(o => o.SetActive(value));
            onChangeActive.Invoke(value);
            onChangeInactive.Invoke(!value);
        } 
    }
    public void Solve() {
        LevelManager.instance.CurPuzzleSolved();
    }
    public void Begin() {
        Active = true;
        DisplayGoal();
        onBegin.Invoke();
    }
    public void NextGoal() {
        goalIndex++;
        DisplayGoal();
    }

    private void DisplayGoal() {
        GoalPanel.instance.ShowNewText(goalText[0]);
    }

}
