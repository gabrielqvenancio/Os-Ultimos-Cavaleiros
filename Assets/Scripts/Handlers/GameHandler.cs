using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameHandler : MonoBehaviour
{
    internal static GameHandler instance;

    [SerializeField] private Vector3 enemiesSpawnPoint;
    [SerializeField] private GameObject enemiesParent;
    private List<Queue<GameObject>> enemiesQueue;

    [SerializeField] private int scorePerFrame;
    private int score;

    [SerializeField] private int spawnWaitTime, spawnMaxWaitTime, spawnMinWaitTime;
    private int spawnPopulationModifier;
    private float levelStartTime;

    public Vector3 globalHitVelocity { get; private set; }

    private void Awake()
    {
        score = 0;
        enemiesQueue = new List<Queue<GameObject>>();
        instance = this;
        spawnPopulationModifier = 0;
        globalHitVelocity = Vector3.zero;
    }

    private void Start()
    {
        levelStartTime = Time.time;
        StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckGlobalVelocity();
    }

    internal void ScoreIncrementation(int scoreYield)
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
        foreach (GameObject enemyPrefab in levelToLoad.enemiesToSpawn)
        {
            enemiesQueue.Add(new Queue<GameObject>());
        }
    }

    private void CheckGlobalVelocity()
    {
        if (globalHitVelocity.x > 0)
        {
            globalHitVelocity -= Greenie.instance.GetAttributes().pushRecovery * Time.fixedDeltaTime;
            if (globalHitVelocity.x < 0)
            {
                globalHitVelocity = Vector3.zero;
                Greenie.instance.Animator.enabled = true;
            }
        }
    }

    internal void ApplyAccelerationOnHit(Enemy enemyHit)
    {
        enemyHit.EnemyPush(Greenie.instance.GetAttributes().pushAcceleration);
        globalHitVelocity = enemyHit.GetAttributes().pushAcceleration;
    }
}
