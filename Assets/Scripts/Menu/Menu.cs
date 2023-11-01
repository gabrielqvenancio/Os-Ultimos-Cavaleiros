using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class Menu : MonoBehaviour
{
    internal static Menu instance;
    internal bool firstChoice = true;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] internal RectTransform cursor;
    [SerializeField] private TextMeshProUGUI highScore;

    private void Awake()
    {
        instance = this;
        highScore.text = "High Score: " + GetHighScore();
    }

    public void Play()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private int GetHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.save";
        if (File.Exists(path))
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate));
            int highScore = br.ReadInt32();
            br.Close();
            return highScore;
        }
        return 0;
    }
}
