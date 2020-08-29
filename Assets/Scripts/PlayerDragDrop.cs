using System.Collections;
using UnityEngine;

public class PlayerDragDrop : MonoBehaviour
{
    public enum Player { Solar, Night };
    public Player PlayerType;
    public float DistanceForResource;

    private GameObject _Resource;
    private Animator _Animator;
    public AudioSource Drag;
    public AudioSource Drop;
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
                    if (gameObject.transform.position.x <= (hit.collider.gameObject.transform.position.x + DistanceForResource) ||
                        gameObject.transform.position.z <= (hit.collider.gameObject.transform.position.z + DistanceForResource))
                    {
                        if (PlayerType == Player.Solar)
                        {
                            if (hit.collider.gameObject.CompareTag("SolarResource"))
                            {
                                StartCoroutine(DragResource(hit.collider.gameObject, gameObject.transform));
                            }
                            if (hit.collider.gameObject.CompareTag("TempleSolar"))
                            {
                                StartCoroutine(DropResource(hit.collider.gameObject, hit.collider.gameObject.transform));
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
                                StartCoroutine(DropResource(hit.collider.gameObject, hit.collider.gameObject.transform));
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator DragResource(GameObject game, Transform parent)
    {
        if (gameObject.transform.childCount > 4)
            yield break;

        Debug.Log("drag");
        _Resource = game;
        _Animator.SetBool("Squats", true);

        if (!Drag.isPlaying)
            Drag.Play();
        yield return new WaitForSeconds(2.3f);

        var position = new Vector3(0.28f, -0.28f, 0.24f);
        SetParentResource(parent, position);
        _Animator.SetBool("Squats", false);
    }

    void SetParentResource(Transform parent, Vector3 position)
    {
        _Resource.transform.localPosition = Vector3.zero;
        _Resource.transform.SetParent(parent);
        _Resource.transform.localPosition = position;
    }

    IEnumerator DropResource(GameObject game, Transform parent)
    {
        if (_Resource == null)
            yield break;

        //fazer as animações de soltar o recurso
        _Animator.SetBool("Drop", true);

        if (!Drop.isPlaying)
            Drop.Play();

        yield return new WaitForSeconds(2.3f);

        var position = new Vector3(0, 0, 0.00409f);
        SetParentResource(parent, position);
        _Animator.SetBool("Drop", false);

        yield return new WaitForSeconds(1.3f);
        _Resource.gameObject.SetActive(false);

        Temple temple = parent.gameObject.GetComponent<Temple>();
        temple.UptadeCountResources();

        _Resource = null;
    }
}

