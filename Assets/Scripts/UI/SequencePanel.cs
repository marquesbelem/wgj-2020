using GalloUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencePanel : MonoBehaviour {

    public GameObject rootObj;
    public List<Image> uiNotePrefabs;
    public Vector2 bigSize;
    public Vector2 smallSize;

    private List<int> lastSequence;

    void Update() {
        MusicalSequence sequence = MusicalSequence.FirstEnabled;
        if (sequence != null) {
            if (!lastSequence.EqualsAllOrdered(sequence.sequence)) {
                lastSequence = new List<int>(sequence.sequence);
                foreach (Transform child in rootObj.transform) {
                    Destroy(child.gameObject); 
                }
                for (int i = 0; i < lastSequence.Count; i++) {
                    Instantiate(uiNotePrefabs[lastSequence[i]], rootObj.transform);
                }
            }
            int selectedNote = sequence.curIndex;
            for (int i=0; i < rootObj.transform.childCount; i++) {
                RectTransform rectTransform = (RectTransform)rootObj.transform.GetChild(i);
                if (rectTransform) {
                    rectTransform.sizeDelta = (selectedNote == i)? bigSize:smallSize;
                }
            }
            rootObj.SetActive(true);
        }
        else {
            rootObj.SetActive(false);
        }
    }

}
