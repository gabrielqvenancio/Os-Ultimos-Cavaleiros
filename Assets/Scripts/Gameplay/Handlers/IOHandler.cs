using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IOHandler : MonoBehaviour
{
    internal static IOHandler instance;

    private void Awake()
    {
        instance = this;
    }

    internal static int LoadHighScore()
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

    internal void SaveHighScore()
    {
        int highScore = 0;
        string path = Application.persistentDataPath + "/highscore.save";

        if (File.Exists(path))
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate));
            highScore = br.ReadInt32();
            br.Close();
        }

        if (UIHandler.instance.Score > highScore)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate));
            bw.Write(UIHandler.instance.Score);
            bw.Close();
        }
    }
}
