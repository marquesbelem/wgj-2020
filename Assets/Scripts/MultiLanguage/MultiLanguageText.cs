using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageText : MonoBehaviour {

    public Text textRenderer;
    public string termIdentifier;

    private void OnEnable() {
        MultiLanguageManager.selectedLanguageChanged += ApplyText; 
    }
    private void OnDisable() {
        MultiLanguageManager.selectedLanguageChanged -= ApplyText;
    }

    public void ApplyText() {
        Language language = MultiLanguageManager.SelectedLanguage;
        if (language != null) {
            textRenderer.text = language[termIdentifier];
        }
    }

}
