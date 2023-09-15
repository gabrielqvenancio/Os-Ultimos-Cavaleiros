using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private ScriptableEnemy attributes;
    private Vector3 localHitVelocity;

    private void Awake()
    {
        localHitVelocity = Vector3.zero;
    }

    void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
        ResetEnemy();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        AccelerationCheck();

        transform.Translate((Velocity + localHitVelocity + GameHandler.instance.globalHitVelocity) * Time.fixedDeltaTime);
        if(transform.position.x <= -15f)
        {
            GameHandler.instance.EnqueueEnemy(gameObject);
        }
    }

    private void AccelerationCheck()
    {
        if(localHitVelocity.x > 0) 
        {
            localHitVelocity -= attributes.pushRecovery * Time.fixedDeltaTime;
            if(localHitVelocity.x <= 0)
            {
                localHitVelocity = Vector3.zero;
                Animator.enabled = true;
            }
        }
    }

    internal void EnemyPush(Vector3 push)
    {
        localHitVelocity += push;
        Animator.enabled = false;
    }

    internal void ResetEnemy()
    {
        localHitVelocity = Vector3.zero;
        float velocityOffset = attributes.baseVelocity.x * 0.66f;
        Velocity = -attributes.baseVelocity + new Vector3(Random.Range(-velocityOffset, velocityOffset), 0, 0) - Greenie.instance.GetAttributes().baseVelocity;
        CurrentHealth = attributes.health;
        CurrentArmor = attributes.armor;
        Animator.enabled = true;
    }

    protected override void OnElimination()
    {
        GameHandler.instance.EnqueueEnemy(gameObject);
    }

    internal ScriptableEnemy GetAttributes()
    {
        return attributes;
    }
}