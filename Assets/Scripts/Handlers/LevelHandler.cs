using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    internal static LevelHandler instance;

    [SerializeField] private ScriptableLevel[] allLevels;
    [SerializeField] private float groundReset, horizonReset, groundLimit, horizonLimit;
    [SerializeField] private GameObject environmentObjectsParent;

    internal Transform EnvironmentObjectsParent { get { return environmentObjectsParent.transform; } }
    internal ScriptableLevel CurrentLevel { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeLevel();
    }

    internal void Move(Transform environmentObjectsParent)
    {
        foreach (Transform environmentObject in environmentObjectsParent)
        {
            if (environmentObject.childCount > 0)
            {
                Move(environmentObject);
            }
            else
            {
                int sortingOrderPosition = environmentObject.GetComponent<SpriteRenderer>().sortingOrder;
                if (sortingOrderPosition >= 0)
                {
                    float parallaxProportion = 1f / Mathf.Pow((CurrentLevel.amountOfLayers - sortingOrderPosition), 2f);
                    environmentObject.Translate((-Greenie.instance.Velocity + PhysicsHandler.instance.GlobalVelocity) * (parallaxProportion * Time.fixedDeltaTime));

                    if (sortingOrderPosition == CurrentLevel.amountOfLayers - 1)
                        CheckMapLimit(environmentObject, groundLimit, groundReset);
                    else
                        CheckMapLimit(environmentObject, horizonLimit, horizonReset);
                }
            }
        }
    }

    private void CheckMapLimit(Transform environmentObject, float limit, float reset)
    {
        if (Mathf.Abs(environmentObject.position.x) > Mathf.Abs(limit))
        {
            Vector3 resetPosition = environmentObject.position;
            resetPosition.x = reset + environmentObject.position.x - (environmentObject.position.x > 0 ? 1 : -1) * limit;
            environmentObject.position = resetPosition;
        }
    }

    private void ChangeLevel()
    {
        ScriptableLevel level = allLevels[Random.Range(0, allLevels.Length)];
        SpawnHandler.instance.OnChangeLevel(level);
        CurrentLevel = level;

        for (int i = 0; i < 2; i++)
        {
            /*
             * Vai ter que mudar bastante, porque agora vai ter quantidade de layers variada no horizonte do cenario, provavelmente
             * botar uns prefab de horizonte arruma
             * 
            grounds[i].GetComponent<SpriteRenderer>().sprite = currentLevel.ground;
            horizons[i].GetComponent<SpriteRenderer>().sprite = currentLevel.horizon;
            */
        }
    }
}
