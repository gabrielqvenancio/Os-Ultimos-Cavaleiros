using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    internal static EnvironmentHandler instance;

    internal ScriptableLevel currentLevel;
    [SerializeField] private ScriptableLevel[] allLevels;
    [SerializeField] private GameObject[] grounds, horizons;

    [SerializeField] private Vector3 groundMovingVelocity, horizonMovingVelocity;

    void Start()
    {
        instance = this;
        currentLevel = allLevels[Random.Range(0, allLevels.Length)];

        ChangeLevelSprites(currentLevel);
        PerformanceHandler.instance.OnChangeLevel(currentLevel);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 resetPosition;

        foreach (GameObject ground in grounds)
        {
            ground.transform.Translate(-groundMovingVelocity * Time.fixedDeltaTime);
            if (ground.transform.position.x <= -30)
            {
                resetPosition = ground.transform.position;
                resetPosition.x = 30f;
                ground.transform.position = resetPosition;
            }
        }

        foreach (GameObject horizon in horizons)
        {
            horizon.transform.Translate(-horizonMovingVelocity * Time.fixedDeltaTime);
            if (horizon.transform.position.x <= -25)
            {
                resetPosition = horizon.transform.position;
                resetPosition.x = 25f;
                horizon.transform.position = resetPosition;
            }
        }
    }

    private void ChangeLevelSprites(ScriptableLevel levelToLoad)
    {
        currentLevel = levelToLoad;
        for(int i = 0; i < 2; i++)
        {
            grounds[i].GetComponent<SpriteRenderer>().sprite = currentLevel.ground;
            horizons[i].GetComponent<SpriteRenderer>().sprite = currentLevel.horizon;
        }
    }
}
