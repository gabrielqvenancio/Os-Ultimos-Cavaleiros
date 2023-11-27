using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public enum Scenes
{
    persistent = 0,
    menu = 1,
    gameplay = 2,
    shop = 3
}

public enum GameState
{
    init,
    gameplay,
    paused,
    options,
    menu,
    tutorial,
    credits,
    loading,
    gameover,
    shop
}

public class SceneHandler : MonoBehaviour
{
    internal static SceneHandler instance;
    internal GameState State { get; set; }
    internal Scenes Scene { get; set; }

    private void Awake()
    {
        instance = this;
    }

    internal AsyncOperation LoadScene(Scenes sceneToLoad)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive);
        return asyncOperation;
    }

    internal AsyncOperation UnloadScene(Scenes sceneToUnload)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync((int)sceneToUnload);
        return asyncOperation;
    }
    internal void ChangeScene(Scenes sceneToLoad, GameState stateAfterLoad)
    {
        State = GameState.loading;
        StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, stateAfterLoad));
    }

    internal void ChangeSceneFade(Scenes sceneToLoad, GameState stateAfterLoad, bool unloadCurrentScene, float duration, OnFinishFade onFinishFade = null)
    {
        if(unloadCurrentScene)
        {
            State = GameState.loading;
            StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, stateAfterLoad, duration));
        }
        else
        {
            AsyncOperation loadingSceneOperation = LoadScene(sceneToLoad);
            loadingSceneOperation.allowSceneActivation = true;
            Scene = sceneToLoad;
            State = stateAfterLoad;

            FadeScreen.instance.CurrentFade = StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[] { loadingSceneOperation }));
        }
    }
}
