using System;
using System.Collections.Generic;
using UnityEngine;
using GalloUtils;

public class AudioSourcePool : MonoBehaviour {

    public List<AudioSource> sourceList;

    private int index = 0;

    public int PlayCount {
        get {
            return sourceList.Count(s => s.isPlaying);
        }
    }
    public bool IsAnyPlaying {
        get {
            return sourceList.Exists(s => s.isPlaying);
        }
    }
    public bool IsAllPlaying {
        get {
            return sourceList.TrueForAll(s => s.isPlaying);
        }
    }
    public AudioSource CurrentSource {
        get {
            return sourceList[index];
        }
    }

    private void NextIndex() {
        index = (index + 1) % sourceList.Count;
    }
    public AudioSource Play() {
        AudioSource source = sourceList[index];
        source.Play();
        NextIndex();
        return source;
    }
    public AudioSource PlayOneShot(AudioClip clip) {
        if (clip == null) {
            return null;
        }
        AudioSource source = CurrentSource;
        source.PlayOneShot(clip);
        NextIndex();
        return source;
    }

}
