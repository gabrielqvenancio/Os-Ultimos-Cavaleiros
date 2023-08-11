using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.U2D;

public class EnvironmentHandler : MonoBehaviour
{
    private ScriptableLevel currentLevel;
    internal static EnvironmentHandler instance;
    [SerializeField] private GameObject[] grounds;
    [SerializeField] private GameObject[] horizons;
    private Vector3 envMovingVelocity;

    private void LoopMethod()
    {
        Move();
    }

    void Start()
    {
        instance = this;
        envMovingVelocity = new Vector3(-5f, 0, 0);

        PerformanceHandler.fixedLoopsDelegate += LoopMethod;
        InputHandler.instance.changeLevelDelegate += ChangeLevelSprites;
    }

    private void Move()
    {
        Vector3 resetPosition;

        foreach (GameObject ground in grounds)
        {
            ground.transform.Translate(envMovingVelocity * Time.fixedDeltaTime);
            if (ground.transform.position.x <= -30)
            {
                resetPosition = ground.transform.position;
                resetPosition.x = 30f;
                ground.transform.position = resetPosition;
            }
        }

        foreach (GameObject horizon in horizons)
        {
            horizon.transform.Translate(envMovingVelocity * Time.fixedDeltaTime);
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

    }
}
