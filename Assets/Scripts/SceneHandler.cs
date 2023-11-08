using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    persistent = 0,
    menu = 1,
    gameplay = 2
}

public enum GameState
{
    gameplay,
    paused,
    options,
    menu,
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

    private void Start()
    {
        ChangeScene(Scenes.menu, GameState.menu, false, FadeScreenOptions.FadeIn, 2f); //********lEMBRA DE DFESCOMENTAR
    }

    public void GameOver()
    {
        IOHandler.instance.SaveHighScore();
        ChangeScene(Scenes.menu, Scenes.gameplay, GameState.menu, true, FadeScreenOptions.FadeIn, 1.5f);
    }

    internal void ChangeActionMap(string map)
    {
        InputHandler.instance.playerInput.SwitchCurrentActionMap(map);
        switch (map)
        {
            case "Player":
            {
                InputHandler.instance.playerInput.uiInputModule.leftClick.Set(InputHandler.instance.playerInput.currentActionMap.FindAction("Click"));
                break;
            }
            case "UI":
            {
                InputHandler.instance.playerInput.uiInputModule.leftClick.Set(InputHandler.instance.playerInput.currentActionMap.FindAction("Click"));
                break;
            }
        }
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

    internal void ChangeScene(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad, bool loadingScreen)
    {
        if (loadingScreen)
        {
            StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, sceneToUnload, stateAfterLoad));
        }
    }

    internal void ChangeScene(Scenes sceneToLoad, GameState stateAfterLoad, bool loadingScreen)
    {
        if (loadingScreen)
        {
            StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, stateAfterLoad));
        }
    }

    internal void ChangeScene(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad, bool loadingScreen, FadeScreenOptions fadeScreenOptions, float duration, OnFinishFade onFinishFade = null)
    {
        if (loadingScreen)
        {
            StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, sceneToUnload, stateAfterLoad, fadeScreenOptions, duration));
        }
        else
        {
            AsyncOperation loadingSceneOperation = LoadScene(sceneToLoad);
            AsyncOperation unloadingSceneOperation = UnloadScene(sceneToUnload);
            loadingSceneOperation.allowSceneActivation = true;
            Scene = sceneToLoad;
            switch (fadeScreenOptions)
            {
                case FadeScreenOptions.FadeIn:
                {
                    StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[] { loadingSceneOperation, unloadingSceneOperation }));
                    break;
                }
                case FadeScreenOptions.FadeOut:
                {
                    StartCoroutine(FadeScreen.instance.FadeOut(duration, onFinishFade, new AsyncOperation[] { loadingSceneOperation, unloadingSceneOperation }));
                    break;
                }
            }
        }
    }

    internal void ChangeScene(Scenes sceneToLoad, GameState stateAfterLoad, bool loadingScreen, FadeScreenOptions fadeScreenOptions, float duration, OnFinishFade onFinishFade = null)
    {
        if (loadingScreen)
        {
            StartCoroutine(LoadingScreen.instance.CallLoadingScreen(sceneToLoad, stateAfterLoad, fadeScreenOptions, duration));
        }
        else
        {
            AsyncOperation loadingSceneOperation = LoadScene(sceneToLoad);
            loadingSceneOperation.allowSceneActivation = true;
            Scene = sceneToLoad;
            switch (fadeScreenOptions)
            {
                case FadeScreenOptions.FadeIn:
                {
                    StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[] { loadingSceneOperation }));
                    break;
                }
                case FadeScreenOptions.FadeOut:
                {
                    StartCoroutine(FadeScreen.instance.FadeOut(duration, onFinishFade, new AsyncOperation[] { loadingSceneOperation }));
                    break;
                }
            }
        }
    }
}
