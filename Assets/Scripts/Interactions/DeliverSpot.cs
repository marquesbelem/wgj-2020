using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]
public class DeliverSpot : MonoBehaviour {

    public static List<DeliverSpot> instanceList = new List<DeliverSpot>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }
    public static DeliverSpot FirstEnabled {
        get {
            if (instanceList.Count == 0) {
                return null;
            }
            else {
                return instanceList[0];
            }
        }
    }

    private Interactable _interactable = null;
    public Interactable InteractableRef {
        get {
            if (_interactable == null) {
                _interactable = GetComponent<Interactable>();
            }
            return _interactable;
        }
    }

    public List<Transform> slotPoints;
    [Serializable] public class WishedResource {
        public int type;
        public int quantity;
    }
    public List<WishedResource> wishedResources;
    public UnityEvent onCompleted;

    private readonly List<Collectable> collected = new List<Collectable>();

    public bool CanInteractWith(PlayerInteractor interactor) => interactor.HasAnyResources;
    public void InteractWith(PlayerInteractor interactor) => interactor.BeginDeliverAllTo(this);

    public void Deliver(List<Collectable> collectables) {
        collected.AddRange(collectables);
        UpdateCollectedPositions();
        if (wishedResources.TrueForAll(w => collected.Count(c => c.type == w.type) >= w.quantity)) {
            onCompleted.Invoke();
        }
    }
    private void UpdateCollectedPositions() {
        for (int i = 0; i < collected.Count; i++) {
            collected[i].gameObject.SetActive(true);
            collected[i].transform.parent = slotPoints[i];
            collected[i].transform.localPosition = Vector3.zero;
            collected[i].transform.localRotation = Quaternion.identity;
        }
    }

}
