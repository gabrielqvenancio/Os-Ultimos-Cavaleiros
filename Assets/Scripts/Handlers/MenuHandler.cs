using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    internal static MenuHandler instance;

    private void Awake()
    {
        instance = this;
    }

    public void GameOver()
    {
        int highScore = 0;
        string path = Application.persistentDataPath + "/highscore.save";

        if (File.Exists(path))
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate));
            highScore = br.ReadInt32();
            br.Close();
        }

        if(UIHandler.instance.Score > highScore)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate));
            bw.Write(UIHandler.instance.Score);
            bw.Close();
        }

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
