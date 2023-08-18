using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceHandler : MonoBehaviour
{
    internal static PerformanceHandler instance;
    internal List<Queue<GameObject>> enemiesQueue;
    [SerializeField] private int spawnIntervalFrames;
    private int framesCount;
    [SerializeField] private Vector3 spawnPoint;

    private void Awake()
    {
        enemiesQueue = new List<Queue<GameObject>>();
    }

    private void Update()
    {
        SpawnTimer();
    }

    private void Start()
    {
        instance = this;
        framesCount = 0;
    }

    internal void SpawnTimer()
    {
        framesCount++;

        if(framesCount >= spawnIntervalFrames)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = ChooseRandomEnemy();
        Instantiate(enemy, spawnPoint, Quaternion.identity);
    }

    private GameObject ChooseRandomEnemy()
    {
        GameObject enemyChoosen = null;
        framesCount = 0;
        int spawnEnemyHelper = 100;
        int randEnemySpawn = UnityEngine.Random.Range(1, 101);

        foreach (GameObject enemy in EnvironmentHandler.instance.currentLevel.enemiesToSpawn)
        {
            ScriptableEnemy enemyAttributes = enemy.GetComponent<Enemy>().attributes;
            spawnEnemyHelper -= enemyAttributes.rarity;
            if (randEnemySpawn > spawnEnemyHelper)
            {
                enemyChoosen = (enemiesQueue[enemyAttributes.mapId].Count > 0 ? DequeueEnemy(enemyAttributes.mapId) : enemyAttributes.prefab);
                break;
            }
        }
        return enemyChoosen;
    }

    internal void EnqueueEnemy(GameObject enemy)
    {
        enemiesQueue[enemy.GetComponent<Enemy>().attributes.mapId].Enqueue(enemy);
        enemy.SetActive(false);
    }

    internal GameObject DequeueEnemy(int enemyMapId)
    {
        GameObject enemy = enemiesQueue[enemyMapId].Dequeue();
        enemy.SetActive(true);
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
}
