using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    internal static Menu instance;
    [SerializeField] internal GameObject menuButtonsParent;
    [SerializeField] private AudioClip menuMusic;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InputHandler.instance.FirstSelected = true;
        menuButtonsParent.transform.Find("Play").GetComponent<Button>().Select();
        SoundHandler.instance.ChangeMusic(menuMusic);
        SoundHandler.instance.PlayMusic();
    }

    public void Play()
    {
        if(SceneHandler.instance.State != GameState.menu || FadeScreen.instance.CurrentFade != null)
        {
            return;
        }
        SceneHandler.instance.State = GameState.gameplay;
        SceneHandler.instance.ChangeSceneFade(Scenes.gameplay, Scenes.menu, GameState.gameplay, 0.5f);
    }

    public void Tutorial()
    {
        if (SceneHandler.instance.State != GameState.menu)
        {
            return;
        }
        TutorialScript.instance.OpenTutorial();
    }

    public void Credits()
    {
        if (SceneHandler.instance.State != GameState.menu)
        {
            return;
        }
        CreditsScript.instance.OpenCredits();
    }

    public void Options()
    {
        if (SceneHandler.instance.State != GameState.menu)
        {
            return;
        }
        InputHandler.instance.Options();
    }

    public void Exit()
    {
        if (SceneHandler.instance.State != GameState.menu)
        {
            return;
        }
        Application.Quit();
    }
}
