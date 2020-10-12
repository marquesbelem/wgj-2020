using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseSaver : MonoBehaviour {

    public bool saveOnStart;

    private Pose savedPose;
    private bool hasAlreadySaved = false;

    private void Start() {
        if (saveOnStart) {
            SaveCurrentPose();
        }
    }

    public void SaveCurrentPose() {
        savedPose = new Pose(transform.position, transform.rotation);
        hasAlreadySaved = true;
    }
    public void LoadSavedPose() {
        if (!hasAlreadySaved) {
            SaveCurrentPose();
        }
        else {
            transform.position = savedPose.position;
            transform.rotation = savedPose.rotation;
        }
    }

}
