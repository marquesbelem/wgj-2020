using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour {

    public static List<PlayerInteractor> instanceList = new List<PlayerInteractor>();
    private void OnEnable() {
        instanceList.Add(this);
    }
    private void OnDisable() {
        instanceList.Remove(this);
    }
    public static PlayerInteractor FirstEnabled {
        get {
            if (instanceList.Count == 0) {
                return null;
            }
            else {
                return instanceList[0];
            }
        }
    }
    public static bool isInputAllowed = true;

    public DayTime playerType;
    public Animator animatorRef;
    public Collider colliderRef;
    public AudioSource collectSound;
    public AudioSource deliverSound;
    public string squatAnimTrigger = "Squat";
    public float squatAnimTime = 1f;
    public string dropAnimTrigger = "Drop";
    public float dropAnimTime = 1f;
    public string collectedAllNewGoalText = "Deliver all resources to the goddess";

    private DeliverSpot curDeliverSpot;
    private List<Collectable> collectedResources = new List<Collectable>();
    public bool HasAnyResources => collectedResources.Count > 0;
    public int CountResourcesOfType(int type) => collectedResources.Count(c => c.type == type);

    void Update() {
        if (isInputAllowed) {
            Interactable.aimed = Interactable.AllThatDetects(colliderRef).MaxElement(e => Vector3.Dot((e.transform.position - transform.position).normalized, Camera.main.transform.forward));
            if (Input.GetMouseButtonDown(0)) {
                Interactable.TryInteractAimedWith(this);
            }
        }
    }

    public void BeginCollectResource(Collectable collectable) {
        collectedResources.Add(collectable);
        if (DeliverSpot.FirstEnabled.WouldCompleteWith(collectedResources)) {
            GoalPanel.instance.ShowNewText(collectedAllNewGoalText);
        }
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
        curDeliverSpot = deliverSpot;
        animatorRef.SetTrigger("Drop");
        Invoke("EndDeliverAllTo", dropAnimTime);
        PlayerMovement.allowed = false;
    }
    public void EndDeliverAllTo() {
        curDeliverSpot.Deliver(collectedResources);
        collectedResources.Clear();
        PlayerMovement.allowed = true;
    }

}


