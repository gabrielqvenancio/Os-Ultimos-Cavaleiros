using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected int currentHealth, currentArmor;
    protected Vector3 velocity;
    [SerializeField] protected HealthBar healthBar;
    public BoxCollider2D BoxCollider { get; protected set; }
    public Animator Animator { get; internal set; }

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
