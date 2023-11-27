using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Selectable selectable;
    private bool selected = false;

    public void OnPointerEnter(PointerEventData evt)
    {
        selectable.Select();
    }

    public void OnDeselect(BaseEventData evt)
    {
        selected = false;
        if(cursor) cursor.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData evt)
    {
        if (cursor)
        {
            cursor.SetParent(transform.Find("Cursor Position"), false);
            cursor.gameObject.SetActive(true);
        }
        if (InputHandler.instance.FirstSelected)
        {
            InputHandler.instance.FirstSelected = false;
            return;
        }

        if(!selected) SoundHandler.instance.PlayCursor();
        selected = true;
    }
}
