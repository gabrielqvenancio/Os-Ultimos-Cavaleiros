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
        InputHandler.instance.firstSelected = true;
        menuButtonsParent.transform.Find("Play").GetComponent<Button>().Select();
        SoundHandler.instance.ChangeMusic(menuMusic);
        SoundHandler.instance.PlayMusic();
    }

    public void Play()
    {
        SceneHandler.instance.State = GameState.gameplay;
        SceneHandler.instance.ChangeScene(Scenes.gameplay, Scenes.menu, GameState.gameplay, true);
    }

    public void Options()
    {
        InputHandler.instance.Options();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
