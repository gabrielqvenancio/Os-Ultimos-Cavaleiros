using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    internal static SpawnHandler instance;

    [SerializeField] private Vector3 enemiesSpawnPoint;
    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private int spawnWaitTime, spawnMaxWaitTime, spawnMinWaitTime;

    private List<Queue<GameObject>> enemiesQueue;
    private int spawnPopulationModifier;
    private float levelStartTime;

    private void Awake()
    {
        instance = this;

        levelStartTime = Time.time;
        enemiesQueue = new List<Queue<GameObject>>();
        spawnPopulationModifier = 0;
    }

    private void Start()
    {
        OnChangeLevel(LevelHandler.instance.CurrentLevel);
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            int spawnEnemyHelper = 100;
            int drawnNumber = Random.Range(1, 101);

            foreach (GameObject enemy in LevelHandler.instance.CurrentLevel.enemiesToSpawn)
            {
                if (ChooseEnemy(enemy, ref spawnEnemyHelper, drawnNumber)) break;
            }

            spawnPopulationModifier += 2;

            float modifiers = spawnPopulationModifier - (Time.time - levelStartTime) / 10f;

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
}
