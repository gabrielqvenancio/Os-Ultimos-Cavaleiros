using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    internal static EnvironmentHandler instance;

    internal ScriptableLevel currentLevel;
    [SerializeField] private ScriptableLevel[] allLevels;
    [SerializeField] private GameObject[] grounds, horizons;

    [SerializeField] internal Vector3 mapVelocity;
    [SerializeField] private float parallaxProportion;
    [SerializeField] private float groundReset, horizonReset, groundLimit, horizonLimit;

    private Vector3 pushRecovery;
    internal Vector3 globalHitVelocity;

    private void Awake()
    {
        instance = this;
        globalHitVelocity = Vector3.zero;
    }

    private void Start()
    {
        pushRecovery = GameObject.Find("Greenie").GetComponent<Greenie>().pushRecovery;
        ChangeLevel();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        CheckAcceleration();
        
        foreach (GameObject ground in grounds)
        {
            int directionCorrection;
            ground.transform.Translate((-mapVelocity + globalHitVelocity) * Time.fixedDeltaTime);

            if (Mathf.Abs(ground.transform.position.x) < Mathf.Abs(groundLimit))
            {
                directionCorrection = ground.transform.position.x > 0 ? 1 : -1;

                Vector3 resetPosition = ground.transform.position;
                resetPosition.x = groundReset - Mathf.Abs(ground.transform.position.x - groundLimit);
                ground.transform.position = resetPosition;
            }
        }

        foreach (GameObject horizon in horizons)
        {
            horizon.transform.Translate((-mapVelocity + globalHitVelocity) * (parallaxProportion * Time.fixedDeltaTime));
            if (horizon.transform.position.x < -horizonLimit || horizon.transform.position.x > horizonLimit)
            {
                Vector3 resetPosition = horizon.transform.position;
                resetPosition.x = horizonReset - Mathf.Abs(horizon.transform.position.x - horizonLimit);
                horizon.transform.position = resetPosition;
            }
        }
    }

    private void ResetPosition(GameObject mapPart, int directionCorrection)
    {
        
    }

    private void CheckAcceleration()
    {
        if (globalHitVelocity.x > 0)
        {
            globalHitVelocity -= pushRecovery * Time.fixedDeltaTime;
            if(globalHitVelocity.x < 0)
            {
                globalHitVelocity = Vector3.zero;
            }
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
