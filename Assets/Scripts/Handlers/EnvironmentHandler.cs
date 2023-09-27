using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    internal static EnvironmentHandler instance;

    [SerializeField] private ScriptableLevel[] allLevels;
    [SerializeField] private GameObject[] grounds, horizons;
    [SerializeField] private float parallaxProportion;
    [SerializeField] private float groundReset, horizonReset, groundLimit, horizonLimit;
    internal ScriptableLevel currentLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeLevel();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        foreach (GameObject ground in grounds)
        {
            ground.transform.Translate((-Greenie.instance.GetAttributes().baseVelocity + GameHandler.instance.GlobalHitVelocity) * Time.fixedDeltaTime);
            CheckMapLimit(ground, groundLimit, groundReset);
        }

        foreach (GameObject horizon in horizons)
        {
            horizon.transform.Translate((-Greenie.instance.GetAttributes().baseVelocity + GameHandler.instance.GlobalHitVelocity) * (parallaxProportion * Time.fixedDeltaTime));
            CheckMapLimit(horizon, horizonLimit, horizonReset);
        }
    }

    private void CheckMapLimit(GameObject mapPart, float limit, float reset)
    {
        if (Mathf.Abs(mapPart.transform.position.x) > Mathf.Abs(limit))
        {
            Vector3 resetPosition = mapPart.transform.position;
            resetPosition.x = reset + mapPart.transform.position.x - (mapPart.transform.position.x > 0 ? 1 : -1) * limit;
            mapPart.transform.position = resetPosition;
        }
    }

    private void ChangeLevel()
    {
        ScriptableLevel level = allLevels[UnityEngine.Random.Range(0, allLevels.Length)];
        GameHandler.instance.OnChangeLevel(level);
        currentLevel = level;

        for(int i = 0; i < 2; i++)
        {
            grounds[i].GetComponent<SpriteRenderer>().sprite = currentLevel.ground;
            horizons[i].GetComponent<SpriteRenderer>().sprite = currentLevel.horizon;
        }
    }
}
