using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : Character
{
    internal static Greenie instance;
    [SerializeField] private ScriptableCharacter attributes;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
        instance = this;
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

    }

    internal ScriptableCharacter GetAttributes()
    {
        return attributes;
    }
}
