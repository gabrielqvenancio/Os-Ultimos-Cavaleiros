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
    internal bool Ready { get; private set; }
    internal bool closeLoadingScreen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        closeLoadingScreen = false;
        Ready = false;
    }

    private IEnumerator ChangeBackgroundSprite(float fadeDuration)
    {
        int spriteNumber = Random.Range(0, sprites.Length);
        backGroundImage.sprite = sprites[spriteNumber];
        while (true)
        {
            yield return new WaitForSecondsRealtime(6f);
            while (backGroundImage.color.a > 0)
            {
                Color color = backGroundImage.color;
                color.a -= Time.deltaTime / fadeDuration;
                backGroundImage.color = color;
                yield return null;
            }

            spriteNumber++;
            if (spriteNumber >= sprites.Length)
            {
                spriteNumber = 0;
            }
            backGroundImage.sprite = sprites[spriteNumber];
            yield return new WaitForSecondsRealtime(1f);

            while (backGroundImage.color.a < 1)
            {
                Color color = backGroundImage.color;
                color.a += Time.deltaTime / fadeDuration;
                backGroundImage.color = color;
                yield return null;
            }
        }
    }

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad)
    {
        Coroutine backGroundAnimation = StartCoroutine(ChangeBackgroundSprite(2f));

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
        Ready = true;
        while (!closeLoadingScreen)
        {
            yield return null;
        }

        closeLoadingScreen = false;
        Ready = false;

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
    }

    internal IEnumerator CallLoadingScreen(Scenes sceneToLoad, Scenes sceneToUnload, GameState stateAfterLoad, float duration, OnFinishFade onFinishFade = null)
    {
        yield return FadeScreen.instance.CurrentFade = StartCoroutine(FadeScreen.instance.FadeOut(duration, onFinishFade, new AsyncOperation[0]));
        Coroutine backGroundAnimation = StartCoroutine(ChangeBackgroundSprite(2f));

        SoundHandler.instance.PauseMusic();

        transform.Find("Loading Screen").gameObject.SetActive(true);
        progressText.text = "0%";

        yield return FadeScreen.instance.CurrentFade = StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[0]));

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
        Ready = true;
        while (!closeLoadingScreen)
        {
            yield return null;
        }

        yield return FadeScreen.instance.CurrentFade = StartCoroutine(FadeScreen.instance.FadeOut(duration, onFinishFade, new AsyncOperation[0]));

        closeLoadingScreen = false;
        Ready = false;

        operation.allowSceneActivation = true;
        while (!operation.isDone)
        {
            yield return null;
        }

        StopCoroutine(backGroundAnimation);
        pressAnyButtonImage.SetActive(false);
        transform.Find("Loading Screen").gameObject.SetActive(false);
        SceneHandler.instance.State = stateAfterLoad;
        SceneHandler.instance.Scene = sceneToLoad;

        FadeScreen.instance.CurrentFade = StartCoroutine(FadeScreen.instance.FadeIn(duration, onFinishFade, new AsyncOperation[0]));
    }
}
