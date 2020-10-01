using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GalloUtils {

    public class ValueStateMachine<T, A, E, S> : MonoBehaviour where A : TransitionAnimation<T>  where E : UnityEvent<T> where S : ValueState<T> {
    
        public List<S> states;
        [SerializeField] private int currentStateIndex = 0;
        public bool allowChangeDuringTransition = true;
        public bool allowIndirectTargetState = true;
        public bool useDistOnStatePathfinding = true;
        public A transitionAnimation;
        public IntegerEvent onEnteredState;
        public E updateValue;

        private int targetStateIndex = -1;

        public S CurrentState {
            get {
                if (!states.IsValidIndex(currentStateIndex)) {
                    return null;
                }
                return states[currentStateIndex];
            }
        }
        public T CurrentStateValue {
            get {
                if (!states.IsValidIndex(currentStateIndex)) {
                    return default;
                }
                return CurrentState.value;
            }
        }
        public List<int> CurrentStateTransitionIndices {
            get {
                if (!states.IsValidIndex(currentStateIndex)) {
                    return null;
                }
                return CurrentState.transitionIndices;
            }
        }
        public bool IsTransitioning {
            get {
                return states.IsValidIndex(targetStateIndex);
            }
        }

        public void Start() {
            transitionAnimation.SetStartingValue(CurrentStateValue);
            transitionAnimation.onFinished = TransitionFinished;
        }
        public void Update() {
            transitionAnimation.Update(Time.deltaTime);
            InvokeUpdate();
        }
        public void TryGoToState(int stateIndex) {
            if (!IsTransitioning || allowChangeDuringTransition) {
                if (CurrentStateTransitionIndices.Contains(stateIndex)) {
                    targetStateIndex = stateIndex;
                    transitionAnimation.NewTargetValue(states[targetStateIndex].value);
                }
                else if (allowIndirectTargetState) {
                    List<int> statePath = null;
                    if (useDistOnStatePathfinding) {
                        statePath = GraphSolver.AStar_PathFinding(i => states[i].transitionIndices, (i1, i2) => DistFunc.Invoke(states[i1].value, states[i2].value), i => DistFunc.Invoke(states[i].value, states[stateIndex].value), PathfindingStartingState, stateIndex);
                    }
                    else {
                        statePath = GraphSolver.BFS_PathFinding(i => states[i].transitionIndices, PathfindingStartingState, stateIndex);
                    }
                    if (statePath != null) {
                        statePath.RemoveFirst();
                        targetStateIndex = stateIndex;
                        transitionAnimation.NewTargetValues(statePath.ConvertAll(i => states[i].value));
                    }
                }
            }
        }
        public void InvokeUpdate() {
            updateValue?.Invoke(transitionAnimation.CurrentValue);
        }

        private void TransitionFinished() {
            currentStateIndex = targetStateIndex;
            targetStateIndex = -1;
            onEnteredState.Invoke(currentStateIndex);
        }
        private int PathfindingStartingState {
            get {
                if (states.IsValidIndex(targetStateIndex)) {
                    return targetStateIndex;
                }
                return currentStateIndex;
            }
        }
        private Func<T, T, float> DistFunc {
            get {
                return transitionAnimation.distFunc;
            }
        }

    }

    [Serializable] public class RectStateMachine : ValueStateMachine<Rect, RectTransitionAnimation, RectEvent, RectValueState> { }


    [Serializable]
    public class ValueState<T> {
        public T value;
        public List<int> transitionIndices;
    }

    [Serializable] public class FloatValueState : ValueState<float> { }
    [Serializable] public class Vector2ValueState : ValueState<Vector2> { }
    [Serializable] public class Vector3ValueState : ValueState<Vector3> { }
    [Serializable] public class QuaternionValueState : ValueState<Quaternion> { }
    [Serializable] public class ColorValueState : ValueState<Color> { }
    [Serializable] public class RectValueState : ValueState<Rect> { }

}
