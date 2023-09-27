using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : Character
{
    internal static Greenie instance;
    [SerializeField] private ScriptableCharacter attributes;
    private int totalHealth, totalArmor;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
        instance = this;
    }

    private void Start()
    {
        totalHealth = attributes.health;
        totalArmor = attributes.armor;
        currentHealth = totalHealth;
        currentArmor = totalArmor;

        healthBar.ApplyHealthRange(0, totalHealth);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyHit = collision.gameObject.GetComponent<Enemy>();

        enemyHit.TakeDamage(attributes.damage);
        TakeDamage(enemyHit.GetAttributes().damage);
        if(enemyHit.gameObject.activeSelf)
        {
            GameHandler.instance.ApplyAccelerationOnHit(enemyHit);
            Animator.enabled = false;
        }
    }

    protected override void OnElimination()
    {
        Application.Quit();
    }

    internal ScriptableCharacter GetAttributes()
    {
        return attributes;
    }
}
