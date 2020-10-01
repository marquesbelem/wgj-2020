using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Collectable : MonoBehaviour {

    private Interactable _interactable = null;
    public Interactable InteractableRef {
        get {
            if (_interactable == null) {
                _interactable = GetComponent<Interactable>();
            }
            return _interactable;
        }
    }

    public int type;

    public bool CanInteractWith(PlayerInteractor interactor) {
        DeliverSpot.WishedResource wishedResource = DeliverSpot.FirstEnabled.wishedResources.Find(w => w.type == type);
        if (wishedResource != null) {
            return wishedResource.quantity > interactor.CountResourcesOfType(type);
        }
        return false;
    }
    public void InteractWith(PlayerInteractor interactor) {
        InteractableRef.enabled = false;
        interactor.BeginCollectResource(this);
    }

}
