using System.Collections;
using UnityEngine;

public class PlayerDragDrop : MonoBehaviour
{
    private bool DropInTemple;
    public enum Player { Solar, Night };

    public Player PlayerType;

    public float DistanceForResource;

    private GameObject _Resource;

    private Animator _Animator;

    public AudioSource Drag;

    public AudioSource Drop;

    public Texture2D MouseDefault;

    public Texture2D MouseDrag;

    public Texture2D MouseDrop;

    private void Start()
    {
        SetStartCursor();
        _Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                Debug.Log("distance: " + Vector3.Distance(transform.position, hit.collider.gameObject.transform.position));
                if (Vector3.Distance(transform.position, hit.collider.gameObject.transform.position) < DistanceForResource)
                {
                    if (PlayerType == Player.Solar)
                    {
                        Action(hit, "SolarResource");
                    }

                    if (PlayerType == Player.Night)
                    {
                        Action(hit, "NightResource");
                    }
                }
                else if (DropInTemple)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(DropResource(hit.collider.gameObject, hit.collider.gameObject.transform));
                    }
                }
                else
                {
                    Cursor.SetCursor(MouseDefault, Vector2.zero, CursorMode.ForceSoftware);
                }
            }
        }
    }

    private IEnumerator DragResource(GameObject game, Transform parent)
    {
        if (gameObject.transform.childCount > 4)
            yield break;

        _Resource = game;
        _Animator.SetBool("Squats", true);

        if (!Drag.isPlaying)
            Drag.Play();
        yield return new WaitForSeconds(2.3f);

        var position = new Vector3(0.28f, -0.28f, 0.24f);
        SetParentResource(parent, position);
        _Animator.SetBool("Squats", false);
    }

    private void SetParentResource(Transform parent, Vector3 position)
    {
        _Resource.transform.localPosition = Vector3.zero;
        _Resource.transform.SetParent(parent);
        _Resource.transform.localPosition = position;
    }

    private IEnumerator DropResource(GameObject game, Transform parent)
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
        DropInTemple = false;
    }

    private void SetStartCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.SetCursor(MouseDefault, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Action(RaycastHit hit, string tagResource)
    {
        if (hit.collider.gameObject.CompareTag(tagResource))
        {
            Cursor.SetCursor(MouseDrag, Vector2.zero, CursorMode.ForceSoftware);

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(DragResource(hit.collider.gameObject, gameObject.transform));
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("TempleNight") || coll.CompareTag("TempleSolar"))
        {
            Cursor.SetCursor(MouseDrop, Vector2.zero, CursorMode.ForceSoftware);
            DropInTemple = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("TempleNight") || coll.CompareTag("TempleSolar"))
        {
            Cursor.SetCursor(MouseDefault, Vector2.zero, CursorMode.ForceSoftware);
            DropInTemple = false;
        }
    }
}

