using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int currentHealth, currentArmor;
    private Vector3 velocity, totalAcceleration;
    private float velocityOffset;
    [SerializeField] internal ScriptableEnemy attributes;

    void Start()
    {
        currentHealth = attributes.health;
        velocityOffset = attributes.baseSpeed / 5f;
        velocity = new Vector3(-attributes.baseSpeed + Random.Range(-velocityOffset, velocityOffset), 0, 0);
        totalAcceleration = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(velocity * Time.fixedDeltaTime + totalAcceleration * Mathf.Pow(Time.fixedDeltaTime, 2) / 2);
        if(transform.position.x <= -15f)
        {
            PerformanceHandler.instance.EnqueueEnemy(gameObject);
        }
    }

    private void OnEnable()
    {
        velocity = new Vector3(-attributes.baseSpeed + Random.Range(-velocityOffset, velocityOffset), 0, 0);
        currentHealth = attributes.health;
        currentArmor = attributes.armor;
    }
}
