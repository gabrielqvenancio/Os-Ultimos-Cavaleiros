using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    internal static SpawnHandler instance;

    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private int spawnWaitTime, spawnMaxWaitTime, spawnMinWaitTime;

    private List<Queue<GameObject>> enemiesQueue;
    private int spawnPopulationModifier;
    private float levelStartTime;

    private void Awake()
    {
        instance = this;

        enemiesQueue = new List<Queue<GameObject>>();
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            int spawnEnemyAux = 100;
            int drawnNumber = Random.Range(1, 101);

            foreach (GameObject enemy in LevelHandler.instance.CurrentLevel.enemiesToSpawn)
            {
                if (ChooseEnemy(enemy, ref spawnEnemyAux, drawnNumber))
                {
                    break;
                }
            }

            spawnPopulationModifier += 2;
            float modifiers = spawnPopulationModifier - ((Time.time - levelStartTime) / 10f);

            yield return new WaitForSeconds(Mathf.Clamp(spawnWaitTime + modifiers, spawnMinWaitTime, spawnMaxWaitTime));
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
                enemyChoosen.transform.position = enemyAttributes.spawnPoint;
            }
            else
            {
                Instantiate(enemy, enemyAttributes.spawnPoint, enemy.transform.rotation, enemiesParent.transform);
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
        levelStartTime = Time.time;
        spawnPopulationModifier = 0;
        enemiesQueue.Clear();

        for (int i = 0; i < levelToLoad.enemiesToSpawn.Length; i++)
        {
            enemiesQueue.Add(new Queue<GameObject>());
        }

        StartCoroutine(SpawnEnemy());
    }

    internal void StopEnemySpawning()
    {
        StopAllCoroutines();
    }

    internal void EliminateAllEnemies()
    {
        for(int i = 0; i < enemiesParent.transform.childCount; i++)
        {
            Enemy enemy = enemiesParent.transform.GetChild(i).GetComponent<Enemy>();
            enemy.ForceElimination();
        }
    }

    internal void DeleteAllEnemyGameObjects()
    {
        for (int i = 0; i < enemiesParent.transform.childCount; i++)
        { 
            Destroy(enemiesParent.transform.GetChild(i).gameObject);
        }
    }
}
