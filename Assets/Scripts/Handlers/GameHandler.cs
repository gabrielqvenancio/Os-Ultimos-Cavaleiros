using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    internal static GameHandler instance;
    internal List<Queue<GameObject>> enemiesQueue;
    [SerializeField] private int spawnIntervalFrames;
    private int framesCount;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private Greenie greenie;

    private void Awake()
    {
        enemiesQueue = new List<Queue<GameObject>>();
    }

    private void Start()
    {
        instance = this;
        framesCount = 0;
    }

    private void Update()
    {
        SpawnTimer();
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
        GameObject enemyChoosen;
        framesCount = 0;
        int spawnEnemyHelper = 100;
        int randEnemySpawn = UnityEngine.Random.Range(1, 101);

        foreach (GameObject enemy in EnvironmentHandler.instance.currentLevel.enemiesToSpawn)
        {
            ScriptableEnemy enemyAttributes = enemy.GetComponent<Enemy>().attributes;
            spawnEnemyHelper -= enemyAttributes.rarity;
            if (randEnemySpawn > spawnEnemyHelper)
            {
                if(enemiesQueue[enemyAttributes.mapId].Count > 0)
                {
                    enemyChoosen = DequeueEnemy(enemyAttributes.mapId);
                    enemyChoosen.transform.position = spawnPoint;
                }
                else
                {
                    enemyChoosen = enemyAttributes.prefab;
                    Instantiate(enemy, spawnPoint, Quaternion.identity, enemiesParent.transform);
                }
                break;
            }
        }
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

    internal void OnEnemyHit(Enemy enemyHit)
    {
        enemyHit.EnemyPush(greenie.pushAcceleration);
        EnvironmentHandler.instance.globalHitVelocity = enemyHit.attributes.pushAcceleration;
    }
}
