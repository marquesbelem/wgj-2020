using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipRandomizer : MonoBehaviour {

    public List<AudioClip> clipList;
    public bool allowRepeat = false;
    public AudioClipEvent setRandomClip;

    private AudioClip lastPlayed = null;

    public void Randomize() {
        AudioClip chosen = null;
        if (clipList != null && clipList.Count > 0) {
            if (clipList.Count == 1) {
                chosen = clipList.First();
            }
            else {
                List<AudioClip> candidates = new List<AudioClip>(clipList);
                if (lastPlayed != null && !allowRepeat) {
                    candidates.Remove(lastPlayed);
                }
                chosen = candidates.RandomElement();
            }
        }
        lastPlayed = chosen;
        if (chosen != null) {
            setRandomClip.Invoke(chosen);
        }
    }

}
