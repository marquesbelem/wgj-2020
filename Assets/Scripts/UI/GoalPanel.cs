using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour {

    public static GoalPanel instance;
    private void Awake() {
        instance = this;
    }
    public static void ShowNewText(string newText) {
        instance.CurText = newText;
        instance.IsTextShowing = true;
    }
    public static void HideText() {
        instance.IsTextShowing = false;
    }

    public Text textRenderer;
    public bool IsTextShowing {
        get => textRenderer.gameObject.activeInHierarchy;
        set => textRenderer.gameObject.SetActive(value);
    }
    public string CurText {
        get => textRenderer.text;
        set => textRenderer.text = value;
    }

}
