using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningHandler : MonoBehaviour
{
    internal static LightningHandler instance;
    void Start()
    {
        instance = this;
    }
}
