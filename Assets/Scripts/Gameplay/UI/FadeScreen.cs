using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnFinishFade();

public class FadeScreen : MonoBehaviour
{
    internal static FadeScreen instance;
    [SerializeField] private Image sprite;
    internal Image Sprite { get; set; }
    internal Coroutine CurrentFade { get; set; }

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

        while (sprite.color.a > 0f)
        {
            Color color = sprite.color;

            color.a -= Time.deltaTime / duration;
            sprite.color = color;
            yield return null;
        }

        onFinishFade?.Invoke();
        sprite.gameObject.SetActive(false);
        CurrentFade = null;
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

        Color color = Color.black;
        color.a = 0;
        sprite.gameObject.SetActive(true);

        while(sprite.color.a < 1f)
        {
            color = sprite.color;
            color.a += Time.deltaTime / duration;
            sprite.color = color;
            yield return null;
        }

        onFinishFade?.Invoke();
        CurrentFade = null;
    }
}
