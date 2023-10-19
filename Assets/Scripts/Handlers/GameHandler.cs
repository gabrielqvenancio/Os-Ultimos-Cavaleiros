using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    internal static GameHandler instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float scoreIncreaseTimeGap;
    [SerializeField] private Vector3 enemiesSpawnPoint;
    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private int spawnWaitTime, spawnMaxWaitTime, spawnMinWaitTime;
    private List<Queue<GameObject>> enemiesQueue;
    private int score;
    private int spawnPopulationModifier;
    private float levelStartTime;
    private List<Vector3> allGlobalVelocities, allGlobalRecoveries;

    internal Vector3 GlobalVelocity { get; private set; }


    private void Awake()
    {
        score = 0;
        enemiesQueue = new List<Queue<GameObject>>();
        instance = this;
        spawnPopulationModifier = 0;
        GlobalVelocity = Vector3.zero;
        allGlobalVelocities = new List<Vector3>();
        allGlobalRecoveries = new List<Vector3>();
    }

    private void Start()
    {
        levelStartTime = Time.time;
        StartCoroutine(SpawnEnemy());
        InvokeRepeating(nameof(PassiveScoreIncrease), scoreIncreaseTimeGap, scoreIncreaseTimeGap);
    }

    private void FixedUpdate()
    {
        CheckGlobalVelocity();
    }

    private void Update()
    {
        UpdateScoreText();
        
    }

    private void UpdateScoreText()
    {
        const int amountOfDigits = 7;
        scoreText.text = "";

        int significantDigits = 0;
        int scoreAux = score;

        do
        {
            scoreAux /= 10;
            significantDigits++;
        }while (scoreAux > 0);

        for(int j = 0; j < (amountOfDigits - significantDigits); j++)
        {
            scoreText.text += '0';
        }

        scoreText.text += score.ToString();
    }

    private void PassiveScoreIncrease()
    {
        score += 1;
    }

    internal void EliminationScoreIncrease(int scoreYield)
    {
        score += scoreYield;
    }

    private IEnumerator SpawnEnemy()
    {
        while(true)
        {
            int spawnEnemyHelper = 100;
            int drawnNumber = UnityEngine.Random.Range(1, 101);

            foreach (GameObject enemy in EnvironmentHandler.instance.CurrentLevel.enemiesToSpawn)
            {
                if (ChooseEnemy(enemy, ref spawnEnemyHelper, drawnNumber)) break;
            }

            spawnPopulationModifier += 2;

            float modifiers = spawnPopulationModifier - (Time.time - levelStartTime)/10f;

            yield return new WaitForSeconds(Mathf.Clamp(spawnWaitTime + modifiers,
                                                        spawnMinWaitTime, spawnMaxWaitTime));
        }
    }

    private bool ChooseEnemy(GameObject enemy, ref int spawnEnemyHelper, int drawnNumber)
    {
        ScriptableEnemy enemyAttributes = enemy.GetComponent<Enemy>().EnemyAttributes;
        spawnEnemyHelper -= enemyAttributes.rarity;
        if (drawnNumber > spawnEnemyHelper)
        {
            if (enemiesQueue[enemyAttributes.mapId].Count > 0)
            {
                GameObject enemyChoosen = DequeueEnemy(enemyAttributes.mapId);
                enemyChoosen.transform.position = enemiesSpawnPoint;
            }
            else
            {
                Instantiate(enemy, enemiesSpawnPoint, Quaternion.identity, enemiesParent.transform);
            }
            return true;
        }
        return false;
    }

    internal void EnqueueEnemy(GameObject enemy)
    {
        enemiesQueue[enemy.GetComponent<Enemy>().EnemyAttributes.mapId].Enqueue(enemy);
        enemy.SetActive(false);
        spawnPopulationModifier -= 2;
    }

    internal GameObject DequeueEnemy(int enemyMapId)
    {
        GameObject enemy = enemiesQueue[enemyMapId].Dequeue();
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().ResetEnemy();
        return enemy;
    }

    internal void OnChangeLevel(ScriptableLevel levelToLoad)
    {
        enemiesQueue.Clear();

        for (int i = 0; i < levelToLoad.enemiesToSpawn.Length; i++)
        {
            enemiesQueue.Add(new Queue<GameObject>());
        }
    }

    private void CheckGlobalVelocity()
    {
        GlobalVelocity = Greenie.instance.LocalHitVelocity;
        for(int i = 0; i < allGlobalVelocities.Count; i++)
        {
            int adjustSignal = (allGlobalVelocities[i].x > 0 ? 1 : -1);

            GlobalVelocity += allGlobalVelocities[i];
            allGlobalVelocities[i] -= allGlobalRecoveries[i] * Time.fixedDeltaTime;

            if (adjustSignal * allGlobalVelocities[i].x < 0)
            {
                allGlobalVelocities[i] = Vector3.zero;
                allGlobalVelocities.RemoveAt(i);
                allGlobalRecoveries.RemoveAt(i);
                i--;
            }
        }
    }

    internal void ResetGlobalVelocity()
    {
        allGlobalVelocities.Clear();
        allGlobalRecoveries.Clear();
        GlobalVelocity = Vector3.zero;
    }

    internal void AddGlobalVelocity(Vector3 velocity, Vector3 recovery)
    {
        allGlobalVelocities.Add(velocity);
        allGlobalRecoveries.Add(recovery);
    }

    internal void PushCharacter(Vector3 acceleration, Character character)
    {
        character.LocalHitVelocity += acceleration * Character.basePushForceFactor;
        character.OnPush();
    }
}