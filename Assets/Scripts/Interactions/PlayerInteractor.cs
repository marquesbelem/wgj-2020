using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour {

    public DayTime playerType;
    public Animator animatorRef;
    public Collider colliderRef;
    public AudioSource collectSound;
    public AudioSource deliverSound;
    public string squatAnimTrigger = "Squat";
    public float squatAnimTime = 1f;
    public string dropAnimTrigger = "Drop";
    public float dropAnimTime = 1f;

    private DeliverSpot curDeliverSpot;
    private readonly List<Collectable> collectedResources = new List<Collectable>();
    public bool HasAnyResources => collectedResources.Count > 0;
    public int CountResourcesOfType(int type) => collectedResources.Count(c => c.type == type);

    void Update() {
        Interactable.aimed = GalloUtils.CollectionExtension.MaxElement(Interactable.AllThatDetects(colliderRef), e => Vector3.Dot((e.transform.position - transform.position).normalized, Camera.main.transform.forward));
        if (Input.GetMouseButtonDown(0)) {
            Interactable.TryInteractAimedWith(this);
        }
    }

    public void BeginCollectResource(Collectable collectable) {
        collectedResources.Add(collectable);
        if (collectSound != null) {
            collectSound.Play();
        }
        animatorRef.SetTrigger("Squat");
        Invoke("EndCollectResource", squatAnimTime);
        PlayerMovement.allowed = false;
    }
    public void EndCollectResource() {
        collectedResources.Last().gameObject.SetActive(false);
        PlayerMovement.allowed = true;
    }
    public void BeginDeliverAllTo(DeliverSpot deliverSpot) {
        if (deliverSound != null) {
            deliverSound.Play();
        }
        collectedResources.RemoveAt(0);
        curDeliverSpot = deliverSpot;
        animatorRef.SetTrigger("Drop");
        Invoke("EndDeliverAllTo", dropAnimTime);
        PlayerMovement.allowed = false;
    }
    public void EndDeliverAllTo() {
        curDeliverSpot.Deliver(collectedResources);
        PlayerMovement.allowed = true;
    }

}


