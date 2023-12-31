using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    internal static UIHandler instance;

    [SerializeField] private TextMeshProUGUI scoreText, highScoreText, moneyText;
    [SerializeField] private float scoreIncreaseTimeGap;
    [SerializeField] private GameObject skillSlidersParent;
    [SerializeField] private Button pauseButton;
    internal int Score { get; private set; }
    internal int HighScore { get; private set; }
    private bool highScoreReached;

    private void Awake()
    {
        instance = this;

        Score = 0;
        HighScore = IOHandler.LoadHighScore();
        highScoreReached = false;
    }

    private void Start()
    {
        InitializePassiveScore();
        if (HighScore == 0)
        {
            Color color = highScoreText.color;
            color.a = 0;
            highScoreText.color = color;
        }
        else
        {
            UpdateScoreText(highScoreText, HighScore);
        }
    }

    internal void InitializePassiveScore()
    {
        InvokeRepeating(nameof(PassiveScoreIncrease), scoreIncreaseTimeGap, scoreIncreaseTimeGap);
    }

    public void PauseButton()
    {
        InputHandler.instance.Pause();
    }

    private void Update()
    {
        UpdateScoreText(scoreText, Score);
        if (Score > HighScore && !highScoreReached)
        {
            highScoreReached = true;
            StartCoroutine(ReduceHighScoreOpacity(0.5f));
        }
    }

    internal void ReduceSkillCooldownUI(int buttonNumber, float value)
    {
        Slider slider = skillSlidersParent.transform.Find("Slider " + buttonNumber).GetComponent<Slider>();
        slider.value = value;
    }

    private void UpdateScoreText(TextMeshProUGUI text, int score)
    {
        const int amountOfDigits = 9;
        text.text = "";

        int significantDigits = 0;
        int scoreAux = score;

        do
        {
            scoreAux /= 10;
            significantDigits++;
        }
        while (scoreAux > 0);

        for(int j = 0; j < (amountOfDigits - significantDigits); j++)
        {
            text.text += '0';
        }

        text.text += score.ToString();
    }

    private void PassiveScoreIncrease()
    {
        Score += 1;
    }

    internal void EliminationScoreIncrease(int scoreYield)
    {
        Score += scoreYield;
    }

    private IEnumerator ReduceHighScoreOpacity(float duration)
    {
        while(highScoreText.color.a > 0)
        {
            Color color = highScoreText.color;
            color.a -= Time.deltaTime / duration;
            highScoreText.color = color;
            yield return null;
        }
    }
}