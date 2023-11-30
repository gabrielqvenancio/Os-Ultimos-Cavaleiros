using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMenu : MonoBehaviour
{
    internal static PreMenu instance;

    [SerializeField] private int numberOfAnimations;
    [SerializeField] private SpriteRenderer preMenuRenderer;
    private int currentAnimation;
    private Coroutine preMenuCoroutine;

    private void Awake()
    {
        instance = this;
        currentAnimation = 1;
    }

    private void Start()
    {
        Color color = preMenuRenderer.color;
        color.a = 0;
        preMenuRenderer.color = color;
        preMenuCoroutine = StartCoroutine(ShowPreMenuScreen(currentAnimation));
    }

    internal void SkipScreen()
    {
        currentAnimation++;
        StopCoroutine(preMenuCoroutine);

        if(currentAnimation <= numberOfAnimations)
        {
            preMenuCoroutine = StartCoroutine(ShowPreMenuScreen(currentAnimation));
        }
        else
        {
            SceneHandler.instance.ChangeSceneFade(Scenes.menu, GameState.menu, false, 1.5f);
            Destroy(gameObject);
        }
    }

    private IEnumerator ShowPreMenuScreen(int animationToPlay)
    {
        if(animationToPlay == 1)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GetComponent<Animator>().SetTrigger("" + animationToPlay);

        Color color = preMenuRenderer.color;
        color.a = 0f;
        preMenuRenderer.color = color;

        while(preMenuRenderer.color.a < 1f)
        {
            color.a += Time.deltaTime;
            preMenuRenderer.color = color;
            yield return null;
        }
        color.a = 1f;

        if(animationToPlay < numberOfAnimations)
        {
            yield return new WaitForSeconds(3f);

            while (preMenuRenderer.color.a > 0f)
            {
                color.a -= Time.deltaTime;
                preMenuRenderer.color = color;
                yield return null;
            }

            SkipScreen();
        }
    }
}
