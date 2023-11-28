using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    internal static LevelHandler instance;

    [SerializeField] private ScriptableLevel[] allLevels;
    [SerializeField] private GameObject environmentObjectsParent;

    internal Transform EnvironmentObjectsParent { get { return environmentObjectsParent.transform; } }
    internal ScriptableLevel CurrentLevel { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnChangeLevel(allLevels[0]);
        SpawnHandler.instance.OnChangeLevel(CurrentLevel);
        InvokeRepeating(nameof(RandomLayer), 5f, 5f);
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
                if (!environmentObject.gameObject.activeSelf || !environmentObject.GetComponent<SpriteRenderer>())
                {
                    return;
                }

                int sortingOrderPosition = environmentObject.GetComponent<SpriteRenderer>().sortingOrder;
                if (sortingOrderPosition >= 0)
                {
                    float parallaxProportion;
                    if (sortingOrderPosition >= CurrentLevel.amountOfLayers)
                    {
                        parallaxProportion = 1f / Mathf.Pow((float)CurrentLevel.amountOfLayers / (sortingOrderPosition + 2), 2f);
                    }
                    else
                    {
                        parallaxProportion = 1f / Mathf.Pow(CurrentLevel.amountOfLayers - sortingOrderPosition, 2f);
                    }
                    environmentObject.Translate((-Greenie.instance.Velocity + PhysicsHandler.instance.GlobalVelocity) * (Character.baseVelocityFactor * parallaxProportion * Time.fixedDeltaTime * (PhysicsHandler.instance.ContinueMovement ? 1 : 0)));

                    if (environmentObject.CompareTag("Ground"))
                    {
                        CheckMapLimit(environmentObject, CurrentLevel.groundLimitPoint, CurrentLevel.groundReturnPoint);
                    }
                    else if(environmentObject.CompareTag("Sign"))
                    {
                        if(environmentObject.transform.position.x <= 1.4f)
                        {
                            TransitionHandler.instance.SignReachedLimitPoint();
                        }
                    }
                    else
                    {
                        CheckMapLimit(environmentObject, CurrentLevel.othersLimitPoint, CurrentLevel.othersReturnPoint);
                    }
                }
            }
        }
    }

    private void CheckMapLimit(Transform environmentObject, float limitPoint, float returnPoint)
    {
        int signalFactor = environmentObject.position.x >= 0 ? 1 : -1;
        if ((signalFactor == -1 && environmentObject.position.x < -limitPoint) || (signalFactor == 1 && environmentObject.position.x > limitPoint))
        {
            if(environmentObject.CompareTag("Random Layer"))
            {
                Destroy(environmentObject.gameObject);
                return;
            }
            Vector3 resetPosition = environmentObject.position;
            resetPosition.x = - signalFactor * (returnPoint - (Mathf.Abs(environmentObject.position.x) - limitPoint));
            environmentObject.position = resetPosition;
        }
    }

    private void RandomLayer()
    {
        if (CurrentLevel.randomLayers.Length == 0)
        {
            return;
        }

        int rng = Random.Range(1, 101);
        if(rng >= 80)
        {
            Instantiate(CurrentLevel.randomLayers[Random.Range(0, CurrentLevel.randomLayers.Length)], new Vector3(CurrentLevel.othersReturnPoint, 0, 0), Quaternion.identity, EnvironmentObjectsParent.transform.GetChild(0).Find("Random"));
        }
    }

    internal void OnChangeLevel(ScriptableLevel level)
    {
        for(int i = 0; i < EnvironmentObjectsParent.transform.childCount; i++)
        {
            Destroy(EnvironmentObjectsParent.transform.GetChild(i).gameObject);
        }

        Instantiate(level.mapPrefab, EnvironmentObjectsParent.transform);
        CurrentLevel = level;

        SoundHandler.instance.ChangeMusic(level.music);
    }
}
