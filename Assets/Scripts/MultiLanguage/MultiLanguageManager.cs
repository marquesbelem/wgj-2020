using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiLanguageManager : MonoBehaviour {

    public List<LanguagePack> languageAssets;

    private static string selectedLanguageID = "";
    public static string SelectedLanguageID {
        get {
            if (string.IsNullOrEmpty(selectedLanguageID) && loadedLanguages != null && loadedLanguages.Count > 0) {
                selectedLanguageID = loadedLanguages[0].languageID;
            }
            return selectedLanguageID;
        }
        set {
            if (loadedLanguages != null && loadedLanguages != null) {
                selectedLanguageID = loadedLanguages.Find(l => l.languageID == value).languageID;
            }
            else {
                selectedLanguageID = "";
            }
            PlayerPrefs.SetString("SelectedLanguage", selectedLanguageID);
            PlayerPrefs.Save();
            selectedLanguageChanged.Invoke();
        }
    }
    public static LanguagePack SelectedLanguage => loadedLanguages.Find(l => SelectedLanguageID == l.languageID);
    public static int SelectedLanguageIndex { 
        get => loadedLanguages.FindIndex(l => SelectedLanguageID == l.languageID);
        set => SelectedLanguageID = loadedLanguages[value].languageID;
    }
    public static List<LanguagePack> loadedLanguages = null;
    public static UnityAction selectedLanguageChanged;

    private void Start() {
        loadedLanguages = languageAssets;
        SelectedLanguageID = PlayerPrefs.GetString("SelectedLanguage", SelectedLanguageID);
    }

}
