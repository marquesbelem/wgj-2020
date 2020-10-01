using System;
using UnityEngine;

namespace GalloUtils {
    [Serializable]
    public class FloatTimer {

        [SerializeField] private float time;

        public bool isCounting = true;
        public bool loop = true;
        public bool inversedDirection = false;
        public float duration = 1f;
        public Action onTimerEnded;
        public Action<float> updateTimeElapsed;
        public Action<float> updateProgress;

        public float Time {
            get {
                return time;
            }
            set {
                bool changed = (time != value);
                time = value;
                if (changed) {
                    if (inversedDirection) {
                        if (loop) {
                            while (time <= 0f) {
                                time += duration;
                                onTimerEnded?.Invoke();
                            }
                        }
                        else {
                            if (time <= 0f) {
                                time = 0f;
                                Pause();
                                onTimerEnded?.Invoke();
                            }
                        }
                    }
                    else {
                        if (loop) {
                            while (time >= duration) {
                                time -= duration;
                                onTimerEnded?.Invoke();
                            }
                        }
                        else {
                            if (time >= duration) {
                                time = duration;
                                Pause();
                                onTimerEnded?.Invoke();
                            }
                        }
                    }
                    InvokeUpdateValueActions();
                }
            }
        }
        private void InvokeUpdateValueActions() {
            updateTimeElapsed?.Invoke(Time);
            updateProgress?.Invoke(Progress);
        }
        public float Progress {
            get {
                return Time / duration;
            }
        }

        public void ToggleDirection() {
            inversedDirection = !inversedDirection;
        }


        public void ResetTime() {
            if (inversedDirection) {
                Time = duration;
            }
            else {
                Time = 0f;
            }
        }
        public void AddTime(float value) {
            Time += value;
        }
        public void SubtractTime(float value) {
            Time -= value;
        }

        public void Replay() {
            isCounting = true;
            ResetTime();
        }
        public void Play() {
            isCounting = true;
        }
        public void Pause() {
            isCounting = false;
        }
        public void Stop() {
            isCounting = false;
            ResetTime();
        }
        public void Update(float deltaTime) {
            if (isCounting) {
                if (inversedDirection) {
                    SubtractTime(deltaTime);
                }
                else {
                    AddTime(deltaTime);
                }
            }
        }

    }

}
