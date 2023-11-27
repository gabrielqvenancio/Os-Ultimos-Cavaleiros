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
        SceneManager.sceneLoaded += SetCameraPositionToOrigin;
    }

    internal void Canvas(Scene scene, LoadSceneMode mode)
    {
        GameObject[] trackedCanvas = GameObject.FindGameObjectsWithTag("Canvas");
        foreach(GameObject canvas in trackedCanvas)
        {
            canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    internal void SetCameraPositionToOrigin(Scene scene, LoadSceneMode mode)
    {
        Vector3 origin = Vector3.zero;
        origin.z = -10f;
        Camera.main.transform.position = origin;
    }
}
