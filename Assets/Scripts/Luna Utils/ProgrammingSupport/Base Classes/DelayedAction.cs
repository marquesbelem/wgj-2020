using System;
using System.Collections;
using UnityEngine;

namespace GalloUtils {
    public class DelayedAction {

        public float delay;
        public Action action;

        public DelayedAction(Action action, float delay) {
            this.delay = delay;
            this.action = action;
        }
        public IEnumerator Start() {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        public static DelayedAction DoAfter(Action action, float delay) {
            DelayedAction result = new DelayedAction(action, delay);
            result.Start();
            return result;
        }

    }

}
