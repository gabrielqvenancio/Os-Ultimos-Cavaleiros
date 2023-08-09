using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Loop();

public class PerformanceHandler : MonoBehaviour
{
    internal static Loop loopsAgregator;
    internal static Loop fixedLoopsAgregator;
    internal Queue<Enemy> enemiesQueue;

    private void Start()
    {
        enemiesQueue = new Queue<Enemy>();
    }

    private void Update()
    {
        loopsAgregator?.Invoke();
    }

    private void FixedUpdate()
    {
        fixedLoopsAgregator?.Invoke();
    }
}
