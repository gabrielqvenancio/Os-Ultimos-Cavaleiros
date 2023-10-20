using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    internal static UIHandler instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float scoreIncreaseTimeGap;
    private int score;

    private void Awake()
    {
        score = 0;
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating(nameof(PassiveScoreIncrease), scoreIncreaseTimeGap, scoreIncreaseTimeGap);
    }

    private void Update()
    {
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        const int amountOfDigits = 7;
        scoreText.text = "";

        int significantDigits = 0;
        int scoreAux = score;

        do
        {
            scoreAux /= 10;
            significantDigits++;
        }while (scoreAux > 0);

        for(int j = 0; j < (amountOfDigits - significantDigits); j++)
        {
            scoreText.text += '0';
        }

        scoreText.text += score.ToString();
    }

    private void PassiveScoreIncrease()
    {
        score += 1;
    }

    internal void EliminationScoreIncrease(int scoreYield)
    {
        score += scoreYield;
    }
}