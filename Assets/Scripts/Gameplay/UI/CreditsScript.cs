using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    internal static CreditsScript instance;
    [SerializeField] internal GameObject creditsParent;
    [SerializeField] private Sprite[] creditsSprites;
    private int currentSpriteIndex;

    private void Start()
    {
        instance = this;
        currentSpriteIndex = 0;
    }

    internal void OpenCredits()
    {
        SceneHandler.instance.State = GameState.credits;
        currentSpriteIndex = 0;
        creditsParent.transform.Find("Image").GetComponent<Image>().sprite = creditsSprites[currentSpriteIndex];
        creditsParent.SetActive(true);
        creditsParent.transform.Find("Next").GetComponent<Button>().Select();
    }

    internal void CloseCredits()
    {
        SceneHandler.instance.State = GameState.menu;
        creditsParent.SetActive(false);

        InputHandler.instance.FirstSelected = true;
        Menu.instance.menuButtonsParent.transform.Find("Play").gameObject.GetComponent<Button>().Select();
    }

    public void Next()
    {
        if (currentSpriteIndex == creditsSprites.Length - 1)
        {
            CloseCredits();
            return;
        }
        currentSpriteIndex++;
        creditsParent.transform.Find("Image").GetComponent<Image>().sprite = creditsSprites[currentSpriteIndex];
    }

    public void Back()
    {
        if (currentSpriteIndex == 0)
        {
            CloseCredits();
            return;
        }
        currentSpriteIndex--;
        creditsParent.transform.Find("Image").GetComponent<Image>().sprite = creditsSprites[currentSpriteIndex];
    }
}
