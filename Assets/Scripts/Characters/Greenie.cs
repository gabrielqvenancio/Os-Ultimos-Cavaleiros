using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int scorePerFrame;
    private int score;
    [SerializeField] internal Vector3 pushAcceleration, pushRecovery;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        score = 0;
    }

    internal void ScoreIncrementation(int scoreYield)
    {
        score += scoreYield;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameHandler.instance.OnEnemyHit(collision.gameObject.GetComponent<Enemy>());
    }
}
