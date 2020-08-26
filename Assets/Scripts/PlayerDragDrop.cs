using System.Collections;
using UnityEngine;

public class PlayerDragDrop : MonoBehaviour
{
    public enum Player { Solar, Night };
    public Player PlayerType;
    public float DistanceForResource;

    private GameObject _Resource;
    private Animator _Animator;
    public GameObject Rig;
    void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    if (Vector3.Distance(Rig.transform.position, hit.collider.gameObject.transform.position) <= DistanceForResource)
                    {
                        if (PlayerType == Player.Solar)
                        {
                            if (hit.collider.gameObject.CompareTag("SolarResource"))
                            {
                                StartCoroutine(DragResource(hit.collider.gameObject, gameObject.transform));
                            }
                            if (hit.collider.gameObject.CompareTag("TempleSolar"))
                            {
                                DropResource(hit.collider.gameObject, hit.collider.gameObject.transform);
                            }
                        }

                        if (PlayerType == Player.Night)
                        {
                            if (hit.collider.gameObject.CompareTag("NightResource"))
                            {
                                StartCoroutine(DragResource(hit.collider.gameObject, gameObject.transform));
                            }
                            if (hit.collider.gameObject.CompareTag("TempleNight"))
                            {
                                Debug.Log("TempleNight");
                                DropResource(hit.collider.gameObject, hit.collider.gameObject.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator DragResource(GameObject game, Transform parent)
    {
        _Resource = game;
        _Animator.SetBool("Squats", true);
        yield return new WaitForSeconds(2.3f);
        //posição correta será arrumadar depois que tiver os assets
        var position = new Vector3(0.28f, -0.28f, 0.24f);
        SetParentResource(parent, position);
        _Animator.SetBool("Squats", false);
    }

    void SetParentResource(Transform parent, Vector3 position)
    {
        _Resource.transform.SetParent(parent);
        _Resource.transform.localPosition = position;
    }

    void DropResource(GameObject game, Transform parent)
    {
        if (_Resource == null)
            return;

        //fazer as animações de soltar o recurso

        //posição correta será arrumadar depois que tiver os assets
        var position = new Vector3(0, 1f, 0);
        SetParentResource(parent, position);

        Temple temple = parent.gameObject.GetComponent<Temple>();
        temple.UptadeCountResources();

        _Resource = null;
    }
}

