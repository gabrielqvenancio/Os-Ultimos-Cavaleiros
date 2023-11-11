using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    internal static InputHandler instance;

    [SerializeField] internal PlayerInput playerInput;
    [SerializeField] private GameObject pause, pauseBase, pauseOptions;

    internal bool firstSelected;

    private void Awake()
    {
        instance = this;
        firstSelected = true;
    }

    private void Start()
    {
        SceneHandler.instance.State = GameState.init;
        SceneHandler.instance.Scene = Scenes.menu;
    }

    public void ChangeMusicVolume(Single volume)
    {
        SoundHandler.instance.ChangeMusicVolume(volume);
        SoundHandler.instance.PlayCursor();
    }

    public void ChangeSoundEffectsVolume(Single volume)
    {
        SoundHandler.instance.ChangeSoundEffectsVolume(volume);
        SoundHandler.instance.PlayCursor();
    }

    public void Pause()
    {
        SceneHandler.instance.State = GameState.paused;
        firstSelected = true;
        Time.timeScale = 0;

        pause.SetActive(true);
        SoundHandler.instance.PauseMusic();

        pauseBase.transform.Find("Continue").gameObject.GetComponent<Button>().Select();
    }

    public void Continue()
    {
        SceneHandler.instance.State = GameState.gameplay;
        Time.timeScale = 1;

        pause.SetActive(false);
        SoundHandler.instance.PlayMusic();
    }

    public void Quit()
    {
        Time.timeScale = 1;
        pause.SetActive(false);
        SceneHandler.instance.ChangeScene(Scenes.menu, Scenes.gameplay, GameState.menu, true, FadeScreenOptions.FadeIn, 1f);
    }

    public void Options()
    {
        if (SceneHandler.instance.State == GameState.menu)
        {
            Menu.instance.menuButtonsParent.SetActive(false);
        }

        SceneHandler.instance.State = GameState.options;
        firstSelected = true;
        
        pause.SetActive(true);
        pauseBase.SetActive(false);
        pauseOptions.SetActive(true);

        pauseOptions.transform.GetChild(0).Find("Slider").GetComponent<Slider>().Select();
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        switch (SceneHandler.instance.State)
        {
            case GameState.gameplay:
            {
                Pause();
                break;
            }
            case GameState.paused:
            {
                Continue();
                break;
            }
            case GameState.options:
            {
                CloseOptions();
                break;
            }
            case GameState.tutorial:
            {
                TutorialScript.instance.CloseTutorial();
                break;
            }
        }
    }

    public void CloseOptions()
    {
        switch (SceneHandler.instance.Scene)
        {
            case Scenes.gameplay:
            {
                SceneHandler.instance.State = GameState.paused;
                firstSelected = true;

                pauseBase.SetActive(true);
                pauseOptions.SetActive(false);

                pauseBase.transform.Find("Continue").gameObject.GetComponent<Button>().Select();
                break;
            }
            case Scenes.menu:
            {
                SceneHandler.instance.State = GameState.menu;
                firstSelected = true;

                pause.SetActive(false);
                pauseBase.SetActive(true);
                pauseOptions.SetActive(false);
                Menu.instance.menuButtonsParent.SetActive(true);

                Menu.instance.menuButtonsParent.transform.Find("Play").gameObject.GetComponent<Button>().Select();
                break;
            }
        }
    }

    public void AnyButton(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }

        switch(SceneHandler.instance.State)
        {
            case GameState.loading:
            {
                if(LoadingScreen.instance.Ready)
                {
                    LoadingScreen.instance.closeLoadingScreen = true;
                }
                break;
            }
            case GameState.init:
            {
                Destroy(GameObject.Find("Pre Menu"));
                SceneHandler.instance.ChangeScene(Scenes.menu, GameState.menu, false, FadeScreenOptions.FadeIn, 2f);
                break;
            }
        }
    }

    public void Skill(InputAction.CallbackContext context)
    {
        if (!context.performed || SceneHandler.instance.State != GameState.gameplay)
        {
            return;
        }

        int id = 0;
        switch(context.action.name)
        {
            case "Skill 1":
            {
                id = 0;
                break;
            }
            case "Skill 2":
            {
                id = 1;
                break;
            }
            case "Skill 3":
            {
                id = 2;
                break;
            }
        }

        Greenie.instance.ClickOnSkill(id);
    }
}
