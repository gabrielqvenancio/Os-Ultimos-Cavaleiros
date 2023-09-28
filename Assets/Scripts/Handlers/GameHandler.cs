using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHandler : MonoBehaviour
{
    internal static GameHandler instance;

    [SerializeField] private Vector3 enemiesSpawnPoint;
    [SerializeField] private GameObject enemiesParent;
    private List<Queue<GameObject>> enemiesQueue;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float scoreIncreaseTimeGap;
    private int score;

    [SerializeField] private int spawnWaitTime, spawnMaxWaitTime, spawnMinWaitTime;
    private int spawnPopulationModifier;
    private float levelStartTime;

    public Vector3 GlobalHitVelocity { get; private set; }


    private void Awake()
    {
        score = 0;
        enemiesQueue = new List<Queue<GameObject>>();
        instance = this;
        spawnPopulationModifier = 0;
        GlobalHitVelocity = Vector3.zero;
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

            foreach (GameObject enemy in EnvironmentHandler.instance.currentLevel.enemiesToSpawn)
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
        ScriptableEnemy enemyAttributes = enemy.GetComponent<Enemy>().GetAttributes();
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
        enemiesQueue[enemy.GetComponent<Enemy>().GetAttributes().mapId].Enqueue(enemy);
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
        if (GlobalHitVelocity.x > 0)
        {
            GlobalHitVelocity -= Greenie.instance.GetAttributes().pushRecovery * Time.fixedDeltaTime;
            if (GlobalHitVelocity.x < 0)
            {
                GlobalHitVelocity = Vector3.zero;
                Greenie.instance.Animator.SetBool("isPushed", false);
            }
        }
    }

    internal void ApplyAccelerationOnHit(Enemy enemyHit)
    {
        enemyHit.EnemyPush(Greenie.instance.GetAttributes().pushAcceleration);
        GlobalHitVelocity = enemyHit.GetAttributes().pushAcceleration;
    }
}