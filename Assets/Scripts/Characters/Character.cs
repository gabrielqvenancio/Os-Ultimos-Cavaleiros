using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int CurrentHealth { get; protected set; }
    public int CurrentArmor { get; protected set; }
    public Vector3 Velocity { get; protected set; }
    public BoxCollider2D BoxCollider { get; protected set; }
    public Animator Animator { get; protected set; }
    public HealthBar healthBar { get; protected set; }

    internal void TakeDamage(int dealtDamage)
    {
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
            OnElimination();
        }
    }

    protected abstract void OnElimination();
}
