using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected HealthBar healthBar;
    protected int currentHealth, currentArmor;
    protected Vector3 velocity;

    internal BoxCollider2D BoxCollider { get; private protected set; }
    internal Animator Animator { get; private protected set; }

    internal void TakeDamage(int dealtDamage)
    {
        int healthBeforeDamage = currentHealth;
        if (currentArmor > 0)
        {
            currentArmor -= dealtDamage;
            if (currentArmor < 0)
            {
                dealtDamage -= currentArmor;
                currentArmor = 0;
            }
        }

        currentHealth -= dealtDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnElimination();
        }
        else
        {
            healthBar.ReduceHealthUI(currentHealth, healthBeforeDamage);
        }
    }

    protected abstract void OnElimination();
}
