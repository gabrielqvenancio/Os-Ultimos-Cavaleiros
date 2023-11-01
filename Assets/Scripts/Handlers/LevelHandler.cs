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
        SpawnHandler.instance.OnChangeLevel(CurrentLevel);
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
                    {
                        CheckMapLimit(environmentObject, groundLimit, groundReset);
                    }
                    else
                    {
                        CheckMapLimit(environmentObject, horizonLimit, horizonReset);
                    }
                }
            }
        }
    }

    private void CheckMapLimit(Transform environmentObject, float limit, float reset)
    {
        int signalFactor = environmentObject.position.x >= 0 ? 1 : -1;
        if ((signalFactor == -1 && environmentObject.position.x < - limit) || (signalFactor == 1 && environmentObject.position.x > limit))
        {
            Vector3 resetPosition = environmentObject.position;
            resetPosition.x = - signalFactor * (reset - (Mathf.Abs(environmentObject.position.x) - limit));
            environmentObject.position = resetPosition;
        }
    }

    private void ChangeLevel()
    {
        for(int i = 0; i < environmentObjectsParent.transform.childCount; i++)
        {
            Destroy(environmentObjectsParent.transform.GetChild(i).gameObject);
        }

        ScriptableLevel level = allLevels[Random.Range(0, allLevels.Length)];
        Instantiate(level.mapPrefab, environmentObjectsParent.transform);
        CurrentLevel = level;

        SoundHandler.instance.ChangeMusic(level.music);
    }
}
