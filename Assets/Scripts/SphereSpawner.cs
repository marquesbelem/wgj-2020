using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public float radius = 1;
    public int quantidade = 100;
    public List<GameObject> spawnable;
    // Start is called before the first frame update
    void Start()
    {
       
        for(int i = 0;i < quantidade; i++)
        {
            Vector3 direcao = Random.insideUnitSphere.normalized;
            Transform novo = Instantiate(spawnable[Random.Range(0, spawnable.Count)]).transform;
            novo.position = transform.position + direcao.normalized * radius;
            novo.up = direcao;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
