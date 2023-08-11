using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeLevel(ScriptableLevel levelToLoad);

public class InputHandler : MonoBehaviour
{
    internal static InputHandler instance;
    internal ChangeLevel changeLevelDelegate;
    void Start()
    {
        instance = this;
    }
}
