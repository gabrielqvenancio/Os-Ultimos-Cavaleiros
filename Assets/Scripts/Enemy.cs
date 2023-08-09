using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 standardVelocity;
    private Vector3 totalAcceleration;
    [SerializeField] private ScriptableObject attributes;

    private void FixedLoopMethod()
    {
        Move();
    }

    void Start()
    {
        standardVelocity = new Vector3(-2f, 0, 0);
        totalAcceleration = Vector3.zero;

        PerformanceHandler.loopsAgregator += FixedLoopMethod;
    }

    private void Move()
    {
        transform.Translate((standardVelocity + totalAcceleration) * Time.fixedDeltaTime);
    }
}
