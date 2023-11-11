using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    internal static TutorialScript instance;
    [SerializeField] private GameObject tutorialParent;
    [SerializeField] private Sprite[] tutorialSprites;
    private int currentSpriteIndex;


    private void Start()
    {
        instance = this;
    }

    internal void OpenTutorial()
    {
        currentSpriteIndex = 0;
        tutorialParent.transform.Find("Image").GetComponent<Image>().sprite = tutorialSprites[currentSpriteIndex];
        SceneHandler.instance.State = GameState.tutorial;
        tutorialParent.SetActive(true);
    }

    internal void CloseTutorial()
    {
        SceneHandler.instance.State = GameState.menu;
        tutorialParent.SetActive(false);
    }

    public void Next()
    {
        if(currentSpriteIndex == tutorialSprites.Length - 1)
        {
            CloseTutorial();
            return;
        }
        currentSpriteIndex++;
        tutorialParent.transform.Find("Image").GetComponent<Image>().sprite = tutorialSprites[currentSpriteIndex];
    }

    public void Back()
    {
        if(currentSpriteIndex == 0)
        {
            CloseTutorial();
            return;
        }
        currentSpriteIndex--;
        tutorialParent.transform.Find("Image").GetComponent<Image>().sprite = tutorialSprites[currentSpriteIndex];
    }
}
