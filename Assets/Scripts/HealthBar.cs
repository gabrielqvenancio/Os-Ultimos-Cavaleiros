using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image healthLost;
    private float healthLostBasePosition;

    private void Start()
    {
        healthLostBasePosition = healthLost.rectTransform.localPosition.x;
    }

    public void ReduceHealthUI(float newSliderValue)
    {
        slider.value = newSliderValue;

        StartCoroutine(ApplyHealthLostDecay());
    }

    public IEnumerator ApplyHealthLostDecay()
    {
        //Aqui vai ter uma barra de vida "extra" que vai substituir a diferença entre a vida antes e depois do dano, e vai decair ao longo do tempo
        //só para ficar bonitinho mesmo
        /*
        Vector3 newHealthLostScale = healthLost.rectTransform.localScale;
        newHealthLostScale.x = CurrentHealth - damage;
        healthLost.rectTransform.localScale = newHealthLostScale;
         */
        yield return null;
    }

    public void ApplyHealthRange(int min, int max)
    {
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = max;
    }
}
