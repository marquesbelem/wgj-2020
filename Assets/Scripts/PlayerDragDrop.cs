using UnityEngine;

public class PlayerDragDrop : MonoBehaviour
{
    public enum Player { Solar, Night };
    public Player PlayerType;
    public float DistanceForResource;

    private GameObject _Resource;
    void Start()
    {

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
                    if (Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position) < DistanceForResource)
                    {
                        if (PlayerType == Player.Solar)
                        {
                            if (hit.collider.gameObject.CompareTag("SolarResource"))
                            {
                                DragResource(hit.collider.gameObject, gameObject.transform);
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
                                DragResource(hit.collider.gameObject, gameObject.transform);
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

    void DragResource(GameObject game, Transform parent)
    {
        _Resource = game;
        //fazer as animações de pegar o recurso

        //posição correta será arrumadar depois que tiver os assets
        var position = new Vector3(.2f, .1f, -0.7f);
        SetParentResource(parent, position);
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

