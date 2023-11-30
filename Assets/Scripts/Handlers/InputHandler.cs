using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    internal static InputHandler instance;

    [SerializeField] internal PlayerInput playerInput;
    [SerializeField] internal GameObject pause, pauseBase, pauseOptions;
    [SerializeField] internal Selectable currentSelection;
    internal bool FirstSelected { get; set; }

    private void Awake()
    {
        instance = this;
        FirstSelected = true;
    }

    private void Start()
    {
        SceneHandler.instance.State = GameState.init;
        SceneHandler.instance.Scene = Scenes.menu;
    }

    public void ChangeMusicVolume(float volume)
    {
        SoundHandler.instance.ChangeMusicVolume((int)volume);
        SoundHandler.instance.PlayCursor();
    }

    public void ChangeSoundEffectsVolume(float volume)
    {
        SoundHandler.instance.ChangeSoundEffectsVolume((int)volume);
        SoundHandler.instance.PlayCursor();
    }

    public void Pause()
    {
        switch (SceneHandler.instance.State)
        {
            case GameState.gameplay:
            case GameState.shop:
            {
                SceneHandler.instance.State = GameState.paused;
                Time.timeScale = 0;
                FirstSelected = true;
                pause.SetActive(true);
                SoundHandler.instance.PauseMusic();

                pauseBase.transform.Find("Continue").gameObject.GetComponent<Button>().Select();
                break;
            }
            default:
            {
                return;
            }
        }
    }

    public void Continue()
    {
        if (SceneHandler.instance.State != GameState.paused)
        {
            return;
        }

        if(SceneHandler.instance.Scene == Scenes.gameplay)
        {
            SceneHandler.instance.State = GameState.gameplay;
        }
        else if(SceneHandler.instance.Scene == Scenes.shop)
        {
            SceneHandler.instance.State = GameState.shop;
        }
        
        Time.timeScale = 1;
        pause.SetActive(false);
        SoundHandler.instance.PlayMusic();
    }

    public void Quit()
    {
        if (SceneHandler.instance.State != GameState.paused || FadeScreen.instance.CurrentFade != null)
        {
            return;
        }

        Time.timeScale = 1;
        pause.SetActive(false);
        switch(SceneHandler.instance.Scene)
        {
            case Scenes.gameplay:
            {
                InputHandler.instance.playerInput.SwitchCurrentActionMap("Shop");
                SceneHandler.instance.ChangeSceneFade(Scenes.shop, GameState.shop, true, 0.5f);
                break;
            }
            case Scenes.shop:
            {
                SceneHandler.instance.ChangeSceneFade(Scenes.menu, GameState.menu, true, 0.5f);
                break;
            }
        }
        
    }

    public void Options()
    {
        pauseOptions.transform.Find("Sound Effects").Find("Slider").GetComponent<Slider>().value = SoundHandler.instance.SoundEffectsVolume;
        pauseOptions.transform.Find("Music").Find("Slider").GetComponent<Slider>().value = SoundHandler.instance.MusicVolume;

        if (SceneHandler.instance.State == GameState.menu)
        {
            Menu.instance.menuButtonsParent.SetActive(false);
        }

        SceneHandler.instance.State = GameState.options;
        FirstSelected = true;
        
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
            case GameState.shop:
            {
                if(ShopScript.instance.MerchantOnRange && ShopScript.instance.MerchantOnRange.MenuToggled)
                {
                    ShopScript.instance.MerchantOnRange.ToggleShopMenu(false);
                }
                else
                {
                    Pause();
                }
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
            case GameState.credits:
            {
                CreditsScript.instance.CloseCredits();
                break;
            }
        }
    }

    public void CloseOptions()
    {
        IOHandler.SaveSoundVolume();
        switch (SceneHandler.instance.Scene)
        {
            case Scenes.gameplay:
            case Scenes.shop:
            {
                SceneHandler.instance.State = GameState.paused;
                FirstSelected = true;

                pauseBase.SetActive(true);
                pauseOptions.SetActive(false);

                pauseBase.transform.Find("Continue").gameObject.GetComponent<Button>().Select();
                break;
            }
            case Scenes.menu:
            {
                SceneHandler.instance.State = GameState.menu;
                FirstSelected = true;

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
            case GameState.gameover:
            {
                if(GameoverScript.instance.WaitingForInput)
                {
                    GameoverScript.instance.WaitingForInput = false;
                }
                break;
            }
            case GameState.init:
            {
                PreMenu.instance.SkipScreen();
                break;
            }
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        switch(SceneHandler.instance.State)
        {
            case GameState.shop:
            {
                if (!ShopScript.instance.MerchantOnRange)
                {
                    return;
                }

                if(!ShopScript.instance.MerchantOnRange.MenuToggled)
                {
                    ShopScript.instance.MerchantOnRange.ToggleShopMenu(true);
                }
                else
                {
                    ShopScript.instance.MerchantOnRange.Upgrade();
                }

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

        SkillHandler.instance.ClickOnSkill(id);
    }
}