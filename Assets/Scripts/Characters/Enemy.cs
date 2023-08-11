using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 standardVelocity, totalAcceleration;
    [SerializeField] internal ScriptableEnemy attributes;

    private void FixedLoopMethod()
    {
        Move();
    }

    void Start()
    {
        standardVelocity = new Vector3(-0.75f + Random.Range(-0.25f, 0.25f), 0, 0);
        totalAcceleration = Vector3.zero;

        PerformanceHandler.loopsDelegate += FixedLoopMethod;
    }

    private void Move()
    {
        transform.Translate(standardVelocity * Time.fixedDeltaTime + (totalAcceleration * Mathf.Pow(Time.fixedDeltaTime, 2)) / 2);
        if(transform.position.x <= -15f)
        {
            gameObject.SetActive(false);
        }
    }
}
