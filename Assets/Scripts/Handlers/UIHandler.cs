using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    internal static UIHandler instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float scoreIncreaseTimeGap;
    [SerializeField] private GameObject skillSlidersParent;
    internal int Score { get; private set; }

    private void Awake()
    {
        Score = 0;
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

    internal void ReduceSkillCooldownUI(int buttonNumber, float value)
    {
        Slider slider = skillSlidersParent.transform.Find("Slider " + buttonNumber).GetComponent<Slider>();
        slider.value = value;
    }

    private void UpdateScoreText()
    {
        const int amountOfDigits = 7;
        scoreText.text = "";

        int significantDigits = 0;
        int scoreAux = Score;

        do
        {
            scoreAux /= 10;
            significantDigits++;
        }while (scoreAux > 0);

        for(int j = 0; j < (amountOfDigits - significantDigits); j++)
        {
            scoreText.text += '0';
        }

        scoreText.text += Score.ToString();
    }

    private void PassiveScoreIncrease()
    {
        Score += 1;
    }

    internal void EliminationScoreIncrease(int scoreYield)
    {
        Score += scoreYield;
    }
}