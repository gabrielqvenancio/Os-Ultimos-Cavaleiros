using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] internal ScriptableEnemy attributes;
    private Vector3 localHitVelocity;

    private void Awake()
    {
        localHitVelocity = Vector3.zero;
        BoxCollider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
    }

    void Start()
    {
        ResetEnemy();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        AccelerationCheck();

        transform.Translate((velocity + localHitVelocity + GameHandler.instance.GlobalVelocity) * Time.fixedDeltaTime);
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
        velocity = -attributes.baseVelocity + new Vector3(Random.Range(-velocityOffset, velocityOffset), 0, 0)
                   - Greenie.instance.attributes.baseVelocity;
        currentHealth = attributes.health;
        currentArmor = attributes.armor;
        Animator.enabled = true;
        healthBar.ApplyHealthRange(0, attributes.health);
    }

    protected override void OnElimination()
    {
        GameHandler.instance.EliminationScoreIncrease(attributes.scoreYield);
        GameHandler.instance.EnqueueEnemy(gameObject);
    }
}