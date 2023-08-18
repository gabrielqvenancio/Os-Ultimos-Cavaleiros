using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningHandler : MonoBehaviour
{
    internal static LightningHandler instance;
    private int framesCount;
    [SerializeField] private int dayDuration;
    [SerializeField] private Light2D sunLight;
    [SerializeField] private GameObject[] timeOfDayObjects;

    void Start()
    {
        instance = this;
        framesCount = 1500;
        dayDuration = 3000;
    }

    private void Update()
    {
        SimulateDay();
    }

    private void SimulateDay()
    {
        framesCount++;
        if(framesCount >= dayDuration)
        {
            framesCount = 0;
        }
    }

    private IEnumerator ChangeDayLight()
    {
        while()
        {
            yield return null;
        }
    }
}
