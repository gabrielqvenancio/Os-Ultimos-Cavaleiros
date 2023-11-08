using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    [SerializeField] private RectTransform cursor;

    public void OnPointerEnter(PointerEventData evt)
    {
        GetComponent<Button>().Select();
        cursor.SetParent(transform.GetChild(0), false);
        if (InputHandler.instance.firstSelected)
        {
            InputHandler.instance.firstSelected = false;
            return;
        }
        SoundHandler.instance.PlayCursor();
    }

    public void OnSelect(BaseEventData evt)
    {
        cursor.SetParent(transform.GetChild(0), false);
        if(InputHandler.instance.firstSelected)
        {
            InputHandler.instance.firstSelected = false;
            return;
        }
        SoundHandler.instance.PlayCursor();
    }
}
