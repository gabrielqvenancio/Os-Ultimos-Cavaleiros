using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionState
{
    startingLevel,
    waiting,
    endingLevel
}

public class TransitionHandler : MonoBehaviour
{
    internal static TransitionHandler instance;

    [SerializeField] private int baseScoreLimit;
    internal TransitionState State { get; private set; }
    private int scoreLimit;
    private bool allowGreenieMovement;
    internal int CurrentGameCycle { get; private set; }

    private void Awake()
    {
        instance = this;

        State = TransitionState.startingLevel;
        scoreLimit = baseScoreLimit;
        allowGreenieMovement = true;
        CurrentGameCycle = 0;
    }

    private void Update()
    {
        CheckScoreLimit();
    }

    private void FixedUpdate()
    {
        if(allowGreenieMovement)
        {
            Greenie.instance.transform.Translate(Greenie.instance.Velocity * (Character.baseVelocityFactor * Time.fixedDeltaTime));
            switch(State)
            {
                case TransitionState.startingLevel:
                {
                    if(Greenie.instance.transform.position.x >= -1.25)
                    {
                        allowGreenieMovement = false;
                        State = TransitionState.waiting;
                        PhysicsHandler.instance.CameraIsMoving = true;
                        PhysicsHandler.instance.ContinueMovement = true;
                    }
                    break;
                }
                case TransitionState.endingLevel:
                {
                    break;
                }
            }
        }
    }

    private void CheckScoreLimit()
    {
        if(UIHandler.instance.Score >= scoreLimit)
        {
            StartTransition();
        }
    }

    private void StartTransition()
    {
        State = TransitionState.endingLevel;
        LevelHandler.instance.EnvironmentObjectsParent.GetChild(0).Find("Sign").gameObject.SetActive(true);
        scoreLimit += baseScoreLimit;

        StopLevel();
    }

    private void StopLevel()
    {
        UIHandler.instance.CancelInvoke();
        SpawnHandler.instance.StopEnemySpawning();
        SpawnHandler.instance.EliminateAllEnemies();
    }

    internal void SignReachedLimitPoint()
    {
        SoundHandler.instance.MusicFadeOut();
        PhysicsHandler.instance.CameraIsMoving = false;
        PhysicsHandler.instance.ContinueMovement = false;
        allowGreenieMovement = true;
    }

    internal void ChangeLevel()
    {
        StartCoroutine(ChangeLevelCoroutine());
    }

    private IEnumerator ChangeLevelCoroutine()
    {
        yield return StartCoroutine(FadeScreen.instance.FadeOut(1f, null, new AsyncOperation[0]));
        if(LevelHandler.instance.CurrentLevel.levelName == "Caverna")
        {
            CurrentGameCycle++;
        }
        LevelHandler.instance.OnChangeLevel(LevelHandler.instance.CurrentLevel.nextLevel);
        SpawnHandler.instance.DeleteAllEnemyGameObjects();
        SkillHandler.instance.ResetSkillsCooldown();

        State = TransitionState.startingLevel;
        StartCoroutine(FadeScreen.instance.FadeIn(0.5f, null, new AsyncOperation[0]));
        PhysicsHandler.instance.ResetGlobalVelocity();
        Greenie.instance.transform.position = Greenie.instance.initialPosition;
        UIHandler.instance.InitializePassiveScore();
        SpawnHandler.instance.OnChangeLevel(LevelHandler.instance.CurrentLevel);
    }
}
