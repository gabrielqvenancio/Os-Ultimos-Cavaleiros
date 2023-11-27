using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Money : MonoBehaviour
{
    internal static Money instance;

    internal int MoneyToApply { get; set; }
    internal TextMeshProUGUI moneyText;
    private int moneyApplied;
    private Coroutine moneyCoroutine;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            MoneyToApply = 0;
            moneyApplied = Inventory.instance.Money;
            moneyCoroutine = null;
            GameObject[] sceneMoneyText = GameObject.FindGameObjectsWithTag("Money");


            moneyText = sceneMoneyText.Length > 0 ? sceneMoneyText[0].GetComponent<TextMeshProUGUI>() : null;
        };
    }

    private void Update()
    {
        if(moneyText)
        {
            UpdateMoneyText();
        }
    }

    private void UpdateMoneyText()
    {
        if (MoneyToApply != 0 && moneyCoroutine == null)
        {
            moneyCoroutine = StartCoroutine(UpdateMoneyEffect());
        }
        moneyText.text = moneyApplied.ToString();
    }

    private IEnumerator UpdateMoneyEffect()
    {
        while (MoneyToApply != 0)
        {
            int aux = MoneyToApply > 0 ? -1 : 1;
            MoneyToApply += aux;
            moneyApplied -= aux;

            yield return new WaitForSeconds(0.001f);
        }

        moneyCoroutine = null;
    }
}
