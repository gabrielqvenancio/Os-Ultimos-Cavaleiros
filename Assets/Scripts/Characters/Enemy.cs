using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private ScriptableEnemy enemyAttributes;
    internal ScriptableEnemy EnemyAttributes { get { return enemyAttributes; } }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        ResetEnemy();
    }

    private void Update()
    {
        HealthBar.UpdateHealth(CurrentHealth);
    }

    private void FixedUpdate()
    {
        Move();
        Deceleration();
    }

    internal void ResetEnemy()
    {
        LocalHitVelocity = Vector3.zero;
        float velocityOffset = attributes.baseVelocity.x * 0.66f;
        Velocity = -attributes.baseVelocity + new Vector3(Random.Range(-velocityOffset, velocityOffset), 0, 0)
                   - Greenie.instance.Attributes.baseVelocity;
        CurrentHealth = attributes.health;
        CurrentArmor = attributes.armor;
        Animator.enabled = true;
        healthBar.ApplyHealthRange(0, attributes.health);
    }
    protected override void Move()
    {
        transform.Translate((Velocity + LocalHitVelocity + PhysicsHandler.instance.GlobalVelocity) * Time.fixedDeltaTime);
        if(transform.position.x <= -15f)
        {
            SpawnHandler.instance.EnqueueEnemy(gameObject);
        }
    }

    protected override void OnElimination()
    {
        UIHandler.instance.EliminationScoreIncrease(enemyAttributes.scoreYield);
        SpawnHandler.instance.EnqueueEnemy(gameObject);
    }

    internal override void OnPush()
    {
        recovery = new Vector3(baseResistance + Attributes.resistance, 0, 0);
        Animator.enabled = false;
    }

    internal override void OnRecover()
    {
        Animator.enabled = true;
    }
}