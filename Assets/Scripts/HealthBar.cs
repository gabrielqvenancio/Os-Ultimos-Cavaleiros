using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    internal void ReduceHealthUI(float newSliderValue)
    {
        slider.value = newSliderValue;
        StartCoroutine(ApplyHealthLostDecay());
    }

    private IEnumerator ApplyHealthLostDecay()
    {
        //Aqui vai ter uma barra de vida "extra" que vai substituir a diferen�a entre a vida antes e depois do dano, e vai decair ao longo do tempo
        //s� para ficar bonitinho mesmo
        /*
        Vector3 newHealthLostScale = healthLost.rectTransform.localScale;
        newHealthLostScale.x = CurrentHealth - damage;
        healthLost.rectTransform.localScale = newHealthLostScale;
         */
        yield return null;
    }

    internal void ApplyHealthRange(float min, float max)
    {
        if(!slider)
        {
            Debug.Log("fewsopdimjifrro");
        }
        else
        {

        slider.minValue = min;
        slider.maxValue = max;
        slider.value = max;
        }
    }
}