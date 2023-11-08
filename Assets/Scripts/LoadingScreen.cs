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

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad)
    {
        SceneHandler.instance.State = GameState.loading;
        Time.timeScale = 0;
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

        Time.timeScale = 1;
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
