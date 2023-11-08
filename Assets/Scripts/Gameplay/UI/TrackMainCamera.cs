using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackMainCamera : MonoBehaviour
{
    internal static TrackMainCamera instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += Canvas;
    }

    internal void Canvas(Scene scene, LoadSceneMode mode)
    {
        GameObject[] trackedCanvas = GameObject.FindGameObjectsWithTag("Canvas");
        foreach(GameObject canvas in trackedCanvas)
        {
            canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}
