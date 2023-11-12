using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public enum Scenes
{
    persistent = 0,
    menu = 1,
    gameplay = 2
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
    loading
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

    public void GameOver()
    {
        IOHandler.SaveHighScore();
        ChangeSceneFade(Scenes.menu, Scenes.gameplay, GameState.menu, 0.5f);
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
    internal void ChangeScene(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad)
    {
        State = GameState.loading;
        StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, sceneToUnload, stateAfterLoad));
    }

    internal void ChangeSceneFade(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad, float duration, OnFinishFade onFinishFade = null)
    {
        State = GameState.loading;
        StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, sceneToUnload, stateAfterLoad, duration));
    }
    
    internal void ChangeSceneFade(Scenes sceneToLoad, GameState stateAfterLoad, float duration, OnFinishFade onFinishFade = null)
    {
        AsyncOperation loadingSceneOperation = LoadScene(sceneToLoad);
        loadingSceneOperation.allowSceneActivation = true;
        Scene = sceneToLoad;
        State = stateAfterLoad;

        StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[] { loadingSceneOperation }));
    }
}
