using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnFinishFade();

public enum FadeScreenOptions
{
    FadeIn,
    FadeOut
}

public class FadeScreen : MonoBehaviour
{
    internal static FadeScreen instance;
    [SerializeField] private Image sprite;
    internal Image Sprite { get; set; }
    private const float baseWaitTime = 0.00392f;

    private void Awake()
    {
        instance = this;
    }

    internal IEnumerator FadeIn(float duration, OnFinishFade onFinishFade, AsyncOperation[] loadingOperations)
    {
        foreach (AsyncOperation operation in loadingOperations)
        {
            while (!operation.isDone)
            {
                yield return null;
            }
        }
        
        sprite.color = Color.black;
        sprite.gameObject.SetActive(true);

        while (sprite.color.a > 0f)
        {
            Color32 color = sprite.color;
            color.a -= 1;
            sprite.color = color;
            yield return new WaitForSeconds(baseWaitTime * duration);
        }

        onFinishFade?.Invoke();
        sprite.gameObject.SetActive(false);
    }

    internal IEnumerator FadeOut(float duration, OnFinishFade onFinishFade, AsyncOperation[] loadingOperations)
    {
        foreach (AsyncOperation operation in loadingOperations)
        {
            while (!operation.isDone)
            {
                yield return null;
            }
        }

        Color32 color = Color.black;
        color.a = 0;
        sprite.color = color;
        sprite.gameObject.SetActive(true);

        while(sprite.color.a < 1f)
        {
            color = sprite.color;
            color.a += 1;
            sprite.color = color;
            yield return new WaitForSeconds(baseWaitTime * duration);
        }

        onFinishFade?.Invoke();
        sprite.gameObject.SetActive(false);
    }
}
