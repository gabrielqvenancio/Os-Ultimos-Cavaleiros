using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider baseSlider, healthLostSlider;

    private void Awake()
    {
        
    }

    internal void ReduceHealthUI(int newHealth, int oldHealth)
    {
        baseSlider.value = newHealth;
        StartCoroutine(ApplyHealthLostDecay(newHealth, oldHealth));
    }

    private IEnumerator ApplyHealthLostDecay(int newHealth, int oldHealth)
    {
        /*
        Vector3 healthLostSliderPosition = healthLostSlider.GetComponent<RectTransform>().localPosition;
        healthLostSliderPosition.x = newHealth / baseSlider.GetComponent<RectTransform>().
        healthLostSlider.GetComponent<RectTransform>().localPosition
        */

        yield return null;
    }

    internal void ApplyHealthRange(float min, float max)
    {
        baseSlider.minValue = min;
        baseSlider.maxValue = max;
        baseSlider.value = max;
    }
}