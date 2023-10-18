using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;

    [SerializeField] protected ScriptableCharacter attributes;
    internal ScriptableCharacter Attributes { get { return attributes; } }

    internal int CurrentHealth { get; private protected set; }
    internal int CurrentArmor { get; private protected set; }
    internal Vector3 Velocity { get; private protected set; }
    internal BoxCollider2D BoxCollider { get; private protected set; }
    internal Animator Animator { get; set; }
    internal Vector3 LocalHitVelocity { get; set; }

    internal void TakeDamage(int dealtDamage)
    {
        int healthBeforeDamage = CurrentHealth;
        if (CurrentArmor > 0)
        {
            CurrentArmor -= dealtDamage;
            if (CurrentArmor < 0)
            {
                dealtDamage -= CurrentArmor;
                CurrentArmor = 0;
            }
        }

        CurrentHealth -= dealtDamage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnElimination();
        }
        else
        {
            healthBar.ReduceHealthUI(CurrentHealth, healthBeforeDamage);
        }
    }

    protected void Deceleration()
    {
        if (LocalHitVelocity.x > 0)
        {
            LocalHitVelocity -= 2 * new Vector3(attributes.resistance, 0, 0) * Time.fixedDeltaTime;
            if (LocalHitVelocity.x <= 0)
            {
                LocalHitVelocity = Vector3.zero;
                OnRecover();
            }
        }
    }

    protected abstract void Move();
    protected abstract void OnElimination();
    internal abstract void OnRecover();
    internal abstract void OnPush();
}
