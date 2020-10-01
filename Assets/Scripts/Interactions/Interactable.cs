using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {

    public static Interactable aimed = null;
    public static List<Interactable> instanceList = new List<Interactable>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }
    public static List<Interactable> AllThatDetects(Collider collider) {
        return instanceList.FindAll(i => i.IsDetectedBySensor(collider));
    }
    public static void TryInteractAimedWith(PlayerInteractor interactor) {
        if (aimed != null) {
            if (aimed.CanInteractWith(interactor)) {
                aimed.InteractWith(interactor);
            }
            else {
                aimed.BlockedInteractWith(interactor);
            }
        }
    }

    public Sensor sensor;
    public DayTime dayTime;

    public Collectable collectableRef;
    public DeliverSpot deliverSlotRef;

    public UnityEvent onInteract;
    public UnityEvent onBlockedInteract;
    public UnityEvent onAimedTrue;
    public UnityEvent onAimedFalse;

    private bool wasAimed = false;
    public bool IsAimed => aimed == this;

    public bool IsDetectedBySensor(Collider collider) {
        return sensor.collidersInside.Contains(collider);
    }
    public bool CanInteractWith(PlayerInteractor interactor) {
        bool result = dayTime == interactor.playerType;
        if (collectableRef != null) {
            result = result && collectableRef.CanInteractWith(interactor);
        }
        if (deliverSlotRef != null) {
            result = result && deliverSlotRef.CanInteractWith(interactor);
        }
        return result;
    }

    public void InteractWith(PlayerInteractor interactor) {
        onInteract.Invoke();
        if (collectableRef != null) {
            collectableRef.InteractWith(interactor);
        }
        if (deliverSlotRef != null) {
            deliverSlotRef.InteractWith(interactor);
        }
    }
    public void BlockedInteractWith(PlayerInteractor interactor) {
        onBlockedInteract.Invoke();
    }

    private void Update() {
        if (!wasAimed && IsAimed) {
            onAimedTrue.Invoke();
            wasAimed = IsAimed;
        }
        else if (wasAimed && !IsAimed) {
            onAimedFalse.Invoke();
            wasAimed = IsAimed;
        }
    }

}
