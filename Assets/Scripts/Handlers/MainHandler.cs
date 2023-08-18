using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandler : MonoBehaviour
{
    internal static MainHandler instance;

    void Start()
    {
        instance = this;
    }
}
