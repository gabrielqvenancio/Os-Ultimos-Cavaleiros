using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    internal static InputHandler instance;

    void Start()
    {
        instance = this;
    }
}
