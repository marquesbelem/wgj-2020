using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFinal : MonoBehaviour
{
    public GameObject Canvas;
    void Start()
    {
        StartCoroutine(EnableCanvas());
    }

    IEnumerator EnableCanvas()
    {
        yield return new WaitForSeconds(16f);
        Canvas.SetActive(true);
        yield return new WaitForSeconds(16f);
        SceneManager.LoadScene("Menu");
    }
}
