using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int currentHealth, currentArmor;
    internal Vector3 velocity;
    private Vector3 localHitVelocity;
    private BoxCollider2D greenieCollider, ownCollider;
    private Animator animator;
    [SerializeField] internal ScriptableEnemy attributes;

    private void Awake()
    {
        
    }

    void Start()
    {
        ResetEnemy();

        localHitVelocity = Vector3.zero;
        
        greenieCollider = GameObject.Find("Greenie").GetComponent<BoxCollider2D>();
        ownCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ResetEnemy();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        AccelerationCheck();

        transform.Translate((velocity + localHitVelocity + EnvironmentHandler.instance.globalHitVelocity) * Time.fixedDeltaTime);
        if(transform.position.x <= -15f)
        {
            GameHandler.instance.EnqueueEnemy(gameObject);
        }
    }

    private void AccelerationCheck()
    {
        if(localHitVelocity.x > 0) 
        {
            localHitVelocity -= attributes.pushRecovery * Time.fixedDeltaTime;
            if(localHitVelocity.x <= 0)
            {
                localHitVelocity = Vector3.zero;
                animator.enabled = true;
            }
        }
    }

    internal void EnemyPush(Vector3 push)
    {
        localHitVelocity += push;
        animator.enabled = false;
    }

    private void ResetEnemy()
    {
        float velocityOffset = attributes.baseSpeed * 0.66f;
        velocity = new Vector3(-attributes.baseSpeed + Random.Range(-velocityOffset, velocityOffset), 0, 0) - EnvironmentHandler.instance.mapVelocity;
        currentHealth = attributes.health;
        currentArmor = attributes.armor;
    }
}
