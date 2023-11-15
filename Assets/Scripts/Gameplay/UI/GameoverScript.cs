using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScript : MonoBehaviour
{
    internal static GameoverScript instance;
    [SerializeField] private GameObject gameoverObject;
    [SerializeField] private GameObject enemiesParent;
    internal bool WaitingForInput { get; set; }
    internal float GameOverFactor { get; set; }
    

    private void Start()
    {
        instance = this;
        WaitingForInput = false;
        GameOverFactor = 1f;
    }

    internal IEnumerator GameOver()
    {
        SoundHandler.instance.StopMusic();

        for(int i = 0; i < enemiesParent.transform.childCount; i++)
        {
            if(enemiesParent.transform.GetChild(i).gameObject.activeSelf)
            {
                enemiesParent.transform.GetChild(i).GetComponent<Enemy>().Animator.enabled = false;
            }
        }

        SceneHandler.instance.State = GameState.gameover;
        gameoverObject.SetActive(true);
        Greenie.instance.GetComponent<SpriteRenderer>().enabled = false;
        Greenie.instance.BoxCollider.enabled = false;
        GameOverFactor = 0f;

        gameoverObject.transform.Find("You Lost").gameObject.SetActive(true);
        gameoverObject.transform.Find("Slash").gameObject.SetActive(true);
        SoundHandler.instance.PlaySoundEffect(gameoverObject.transform.Find("Slash").GetComponent<AudioSource>(), gameoverObject.transform.Find("Slash").GetComponent<AudioSource>().clip);
        gameoverObject.transform.Find("Greenie Death").gameObject.SetActive(true);

        yield return new WaitForSeconds(1.75f);
        yield return StartCoroutine(IncreaseOpacity(gameoverObject.transform.Find("Background").GetComponent<Image>(), 2f));
        yield return new WaitForSeconds(0.5f);

        gameoverObject.transform.Find("You Lost").gameObject.SetActive(true);
        yield return StartCoroutine(IncreaseOpacity(gameoverObject.transform.Find("You Lost").GetComponent<Image>(), 1.5f));
        gameoverObject.transform.Find("Press Any Button").gameObject.SetActive(true);

        WaitingForInput = true;

        while(WaitingForInput)
        {
            yield return null;
        }

        SceneHandler.instance.ChangeSceneFade(Scenes.menu, Scenes.gameplay, GameState.menu, 0.5f);
    }

    private IEnumerator IncreaseOpacity(Image image, float duration)
    {
        while (image.color.a < 1f)
        {
            Color color = image.color;
            color.a += Time.deltaTime / duration;
            image.color = color;
            yield return null;
        }
    }
}
