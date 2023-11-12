using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class IOHandler
{
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

    internal static void SaveHighScore()
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

    internal static int[] LoadSoundVolume()
    {
        int[] volumes = new int[] { 4, 4 };
        string path = Application.persistentDataPath + "/soundVolume.save";
        if (File.Exists(path))
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.OpenOrCreate));
            volumes[0] = br.ReadInt32();
            volumes[1] = br.ReadInt32();
            br.Close();
        }
        return volumes;
    }

    internal static void SaveSoundVolume()
    {
        int soundEffectVolume = (int)InputHandler.instance.pauseOptions.transform.Find("Sound Effects").Find("Slider").GetComponent<Slider>().value;
        int musicVolume = (int)InputHandler.instance.pauseOptions.transform.Find("Music").Find("Slider").GetComponent<Slider>().value;
        string path = Application.persistentDataPath + "/soundVolume.save";

        BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate));
        bw.Write(soundEffectVolume);
        bw.Write(musicVolume);
        bw.Close();
    }
}
