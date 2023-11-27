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
        healthBar.UpdateHealth(CurrentHealth);
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
        Velocity = - (attributes.baseVelocity + Greenie.instance.Attributes.baseVelocity + new Vector3(Random.Range(-velocityOffset, velocityOffset), 0, 0));
        CurrentHealth = attributes.health;
        Animator.enabled = true;
        healthBar.ApplyHealthRange(0, attributes.health);
        BoxCollider.enabled = true;
    }
    protected override void Move()
    {
        transform.Translate((Velocity + LocalHitVelocity + PhysicsHandler.instance.GlobalVelocity) * (baseVelocityFactor * Time.fixedDeltaTime * GameoverScript.instance.GameOverFactor));
        if(transform.position.x <= -5f)
        {
            SpawnHandler.instance.EnqueueEnemy(gameObject);
        }
    }

    protected override void OnElimination()
    {
        Inventory.instance.Money += enemyAttributes.moneyDrop;
        Money.instance.MoneyToApply += enemyAttributes.moneyDrop;

        Animator.SetTrigger("die");
        UIHandler.instance.EliminationScoreIncrease(enemyAttributes.scoreYield);
        Velocity = - Greenie.instance.Attributes.baseVelocity;
        LocalHitVelocity = Vector3.zero;
        BoxCollider.enabled = false;
        if(enemyAttributes.deathSounds.Length > 0)
        {
            SoundHandler.instance.PlaySoundEffect(GetComponent<AudioSource>(), enemyAttributes.deathSounds[Random.Range(0, enemyAttributes.deathSounds.Length)]);
            GetComponent<AudioSource>().Play();
        }
    }

    internal override void OnPush()
    {
        recovery = new Vector3(Attributes.resistance, 0, 0);
        Animator.SetTrigger("gotHit");
        Animator.SetBool("isPushed", true);
    }

    internal override void OnRecover()
    {
        Animator.SetBool("isPushed", false);
    }
}