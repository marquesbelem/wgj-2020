using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour {

    public static GoalPanel instance;
    private void Awake() {
        instance = this;
    }

    public Text textRenderer;
    public string fixedTextStart = "Goal: ";
    public bool IsTextShowing {
        get => textRenderer.gameObject.activeInHierarchy;
        set => textRenderer.gameObject.SetActive(value);
    }
    public string CurText {
        set => textRenderer.text = fixedTextStart + value;
    }
    public void ShowNewText(string newText) {
        CurText = newText;
        IsTextShowing = true;
    }
    public void HideText() {
        IsTextShowing = false;
    }

}
