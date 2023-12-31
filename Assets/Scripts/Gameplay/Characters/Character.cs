using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected ScriptableCharacter attributes;
    internal ScriptableCharacter Attributes { get { return attributes; } }

    protected int currentHealth;
    internal int CurrentHealth 
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, Attributes.health); }
    }
    internal Vector3 Velocity { get; private protected set; }
    internal BoxCollider2D BoxCollider { get; private protected set; }
    internal Animator Animator { get; set; }
    internal Vector3 LocalHitVelocity { get; set; }
    protected Vector3 recovery;

    internal const float basePushForceFactor = 4f, baseRecoveryFactor = 9f, baseVelocityFactor = 0.3f;

    protected virtual void Initialize()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
        Velocity = Attributes.baseVelocity;
        CurrentHealth = attributes.health;
        recovery = Vector3.zero;
    }

    internal void TakeDamage(int dealtDamage)
    {
        CurrentHealth -= dealtDamage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnElimination();
        }
    }

    protected void Deceleration()
    {
        if (LocalHitVelocity.x != 0)
        {
            int signalFactor = LocalHitVelocity.x > 0 ? 1 : -1;
            LocalHitVelocity -= recovery * (Time.fixedDeltaTime * signalFactor);
            if ((signalFactor == 1 && LocalHitVelocity.x <= 0) || (signalFactor == -1 && LocalHitVelocity.x >= 0))
            {
                LocalHitVelocity = Vector3.zero;
                recovery = Vector3.zero;
                OnRecover();
            }

            recovery.x += Attributes.resistance * baseRecoveryFactor * Time.fixedDeltaTime;
        }
    }

    protected abstract void Move();
    protected abstract void OnElimination();
    internal abstract void OnRecover();
    internal abstract void OnPush();
}
