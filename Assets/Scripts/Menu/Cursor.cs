using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private AudioSource cursorSource;

    public void OnPointerEnter(PointerEventData evt)
    {
        GetComponent<Button>().Select();
        Menu.instance.cursor.SetParent(transform.GetChild(0), false);
        if(Menu.instance.firstChoice)
        {
            Menu.instance.firstChoice = false;
            return;
        }
        cursorSource.Play();
    }

    public void OnSelect(BaseEventData evt)
    {
        Menu.instance.cursor.SetParent(transform.GetChild(0), false);
        if (Menu.instance.firstChoice)
        {
            Menu.instance.firstChoice = false;
            return;
        }
        cursorSource.Play();
    }
}
