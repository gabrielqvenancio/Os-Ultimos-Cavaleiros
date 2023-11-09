using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    internal static LoadingScreen instance;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject pressAnyButtonImage;
    internal bool ready { get; private set; }
    internal bool closeLoadingScreen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        closeLoadingScreen = false;
        ready = false;
    }

    private IEnumerator ChangeBackgroundSprite(float fadeDuration)
    {
        int spriteNumber = Random.Range(0, sprites.Length);
        backGroundImage.sprite = sprites[spriteNumber];
        while (true)
        {
            yield return new WaitForSeconds(6f);
            while (backGroundImage.color.a > 0)
            {
                Color32 color = backGroundImage.color;
                color.a -= 1;
                backGroundImage.color = color;
                yield return new WaitForSeconds(FadeScreen.baseWaitTime * fadeDuration);
            }

            spriteNumber++;
            if (spriteNumber >= sprites.Length)
            {
                spriteNumber = 0;
            }
            backGroundImage.sprite = sprites[spriteNumber];
            yield return new WaitForSeconds(1f);

            while (backGroundImage.color.a < 1)
            {
                Color32 color = backGroundImage.color;
                color.a += 1;
                backGroundImage.color = color;
                yield return new WaitForSeconds(FadeScreen.baseWaitTime * fadeDuration);
            }
        }
    }

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad)
    {
        Coroutine backGroundAnimation = StartCoroutine(ChangeBackgroundSprite(2f));

        SceneHandler.instance.State = GameState.loading;
        SoundHandler.instance.PauseMusic();

        transform.Find("Loading Screen").gameObject.SetActive(true);
        progressText.text = "0%";

        AsyncOperation operation = SceneHandler.instance.UnloadScene(sceneToUnload);

        while (!operation.isDone)
        {
            progressText.text = ((int)(operation.progress * 50f)).ToString() + "%";
            yield return null;
        }

        operation = SceneHandler.instance.LoadScene(sceneToLoad);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            progressText.text = ((int)(50f + (operation.progress * 50f / 0.9f))).ToString() + "%";
            yield return null;
        }

        progressText.text = "100%";

        pressAnyButtonImage.SetActive(true);
        ready = true;
        while (!closeLoadingScreen)
        {
            yield return null;
        }

        closeLoadingScreen = false;
        ready = false;

        operation.allowSceneActivation = true;
        while(!operation.isDone)
        {
            yield return null;
        }

        StopCoroutine(backGroundAnimation);
        pressAnyButtonImage.SetActive(false);
        transform.Find("Loading Screen").gameObject.SetActive(false);
        SceneHandler.instance.State = stateAfterLoad;
        SceneHandler.instance.Scene = sceneToLoad;
        SoundHandler.instance.PlayMusic();
    }

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, GameState stateAfterLoad)
    {
        SceneHandler.instance.State = GameState.loading;
        Time.timeScale = 0;

        transform.Find("Loading Screen").gameObject.SetActive(true);
        progressText.text = "0%";

        AsyncOperation operation = SceneHandler.instance.LoadScene(sceneToLoad);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            progressText.text = ((int)(operation.progress / 0.9f * 100f)).ToString() + "%";
            yield return null;
        }

        progressText.text = "100%";

        ready = true;
        while (!closeLoadingScreen)
        {
            yield return null;
        }

        closeLoadingScreen = false;
        ready = false;

        operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            yield return null;
        }

        Time.timeScale = 1;
        transform.Find("Loading Screen").gameObject.SetActive(false);
        SceneHandler.instance.State = stateAfterLoad;
        SceneHandler.instance.Scene = sceneToLoad;
        SoundHandler.instance.PlayMusic();
    }

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad, FadeScreenOptions fadeScreenOptions, float duration, OnFinishFade onFinishFade = null)
    {
        yield return StartCoroutine(CallLoadingScreen(sceneToLoad, sceneToUnload, stateAfterLoad));
        switch (fadeScreenOptions)
        {
            case FadeScreenOptions.FadeIn:
            {
                StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[0]));
                break;
            }
            case FadeScreenOptions.FadeOut:
            {
                StartCoroutine(FadeScreen.instance.FadeOut(duration, onFinishFade, new AsyncOperation[0]));
                break;
            }
        }
    }

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, GameState stateAfterLoad, FadeScreenOptions fadeScreenOptions, float duration, OnFinishFade onFinishFade = null)
    {
        yield return StartCoroutine(CallLoadingScreen(sceneToLoad, stateAfterLoad));
        switch (fadeScreenOptions)
        {
            case FadeScreenOptions.FadeIn:
            {
                StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[0]));
                break;
            }
            case FadeScreenOptions.FadeOut:
            {
                StartCoroutine(FadeScreen.instance.FadeOut(duration, onFinishFade, new AsyncOperation[0]));
                break;
            }
        }
    }
}