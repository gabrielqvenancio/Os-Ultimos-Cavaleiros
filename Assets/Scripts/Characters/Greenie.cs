using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenie : MonoBehaviour
{
    private Animator animator;
    private int score;

    void Start()
    {
        animator = GetComponent<Animator>();
        score = 0;
    }

    internal void ScoreFrameIncrementation()
    {
        score++;
    }

    internal void ScoreEnemyIncrementation(int scoreYield)
    {
        score += scoreYield;
    }
}
