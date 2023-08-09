using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    private Animator animator;

    private void LoopMethod()
    {

    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
}
