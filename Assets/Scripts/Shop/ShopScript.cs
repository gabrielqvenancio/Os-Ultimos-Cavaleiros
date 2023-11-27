using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopScript : MonoBehaviour
{
    internal static ShopScript instance;

    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector3 mapLeftLimit, mapRightLimit;
    [SerializeField] private AudioClip shopMusic;
    [SerializeField] private AudioSource roxerSource;
    internal Merchant MerchantOnRange { get; set; }

    private void Start()
    {
        instance = this;
        SoundHandler.instance.ChangeMusic(shopMusic);
        SoundHandler.instance.PlayMusic();
    }

    private void FixedUpdate()
    {
        MoveGreenie(InputHandler.instance.playerInput.currentActionMap.FindAction("Move").ReadValue<Vector2>());
        MoveCamera();
    }

    private void Update()
    {
        PlayHammerSound();
    }

    private void PlayHammerSound()
    {
        if (roxerSource.gameObject.GetComponent<SpriteRenderer>().sprite.name == "5")
        {
            SoundHandler.instance.PlaySoundEffect(roxerSource, roxerSource.clip);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Character":
            {
                MerchantOnRange = collision.GetComponent<Merchant>();
                MerchantOnRange.ProximityChange(true);
                break;
            }
            case "Other":
            {
                if(collision.gameObject.name == "Play Collider")
                {
                    InputHandler.instance.playerInput.SwitchCurrentActionMap("Player");
                    IOHandler.SaveMoney();
                    IOHandler.SaveSkillsLevel();
                    SceneHandler.instance.ChangeSceneFade(Scenes.gameplay, GameState.gameplay, true, 1f);
                    enabled = false;
                }
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Character":
            {
                MerchantOnRange.ProximityChange(false);
                MerchantOnRange = null;
                break;
            }
        }
    }

    private void MoveGreenie(Vector2 input)
    {
        if(input.x != 0)
        {
            if(input.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else if(input.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }

            GetComponent<Animator>().SetBool("isWalking", true);
            Vector2 position = transform.position;
            position.x = Mathf.Clamp(position.x + (input.x * velocity.x * Time.fixedDeltaTime), mapLeftLimit.x + 0.12f, mapRightLimit.x - 0.12f);
            transform.position = position;
            return;
        }
        GetComponent<Animator>().SetBool("isWalking", false);
    }

    private void MoveCamera()
    {
        float halfCameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        Vector3 cameraPosition = Camera.main.transform.transform.position;
        cameraPosition.x = Mathf.Clamp(transform.position.x + 0.4f, mapLeftLimit.x + halfCameraWidth, mapRightLimit.x - halfCameraWidth);
        Camera.main.transform.transform.position = cameraPosition;
    }
}
