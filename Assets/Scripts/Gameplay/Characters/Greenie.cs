using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal delegate void GreenieHit();

public class Greenie : Character
{
    internal static Greenie instance;

    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] internal Vector3 initialPosition;

    private float reducedDamagePercentage;
    internal float ReducedDamagePercentage
    {
        get { return Mathf.Clamp(reducedDamagePercentage, 0, 100); }
        set { reducedDamagePercentage = value; }
    }
    internal event GreenieHit GreenieHitEvent;
    internal int Pushable { get; set; }
    internal int AdditionalDamage { get; set; }
    internal Vector3 AdditionalForce { get; set; }

    private void Awake()
    {
        instance = this;

        Pushable = 0;
        AdditionalDamage = 0;
        AdditionalForce = Vector3.zero;
        reducedDamagePercentage = 0;

        Initialize();
    }

    private void Start()
    {
        healthBar.ApplyHealthRange(0, Attributes.health);
    }

    private void FixedUpdate()
    {
        Move();
        Deceleration();
    }

    private void LateUpdate()
    {
        healthBar.UpdateHealth(CurrentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();
            Debug.Log(enemyHit.Attributes.damage + TransitionHandler.instance.CurrentGameCycle * enemyHit.EnemyAttributes.extraDamagePerCycle);
            TakeDamage((int)((100f - reducedDamagePercentage) / 100f) * (enemyHit.Attributes.damage + TransitionHandler.instance.CurrentGameCycle * enemyHit.EnemyAttributes.extraDamagePerCycle));
            if (CurrentHealth <= 0)
            {
                return;
            }
            enemyHit.TakeDamage(Attributes.damage + AdditionalDamage);

            if (enemyHit.CurrentHealth > 0)
            {
                PhysicsHandler.instance.PushCharacter(Attributes.pushForce + AdditionalForce, enemyHit);
                if (Pushable == 0)
                {
                    SoundHandler.instance.PlaySoundEffect(GetComponent<AudioSource>(), hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)]);
                    PhysicsHandler.instance.PushCharacter(enemyHit.Attributes.pushForce, this);
                }
            }

            OnGreenieHit();
            SoundHandler.instance.PlayHitSound();
        }
        else if(collision.CompareTag("Next Map"))
        {
            TransitionHandler.instance.ChangeLevel();
        }
    }

    protected override void Move()
    {
        LevelHandler.instance.Move(LevelHandler.instance.EnvironmentObjectsParent);
    }

    protected override void OnElimination()
    {
        IOHandler.SaveHighScore();
        IOHandler.SaveMoney();
        StartCoroutine(GameoverScript.instance.GameOver());
    }

    internal override void OnPush()
    {
        recovery = new Vector3(Attributes.resistance, 0, 0);
        Animator.SetBool("isPushed", true);
        Animator.SetTrigger("gotHit");
    }

    internal override void OnRecover()
    {
        Animator.SetBool("isPushed", false);
    }

    private void OnGreenieHit()
    {
        GreenieHitEvent?.Invoke();
    }
}